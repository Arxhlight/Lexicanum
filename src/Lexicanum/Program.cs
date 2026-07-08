using Lexicanum.Core.Content;
using Lexicanum.Core.Services;
using Lexicanum.Features.CodeTrainer;
using Lexicanum.Features.Lexicon;
using Lexicanum.Features.Quizlet;
using Lexicanum.Features.Scoreboard;
using Lexicanum.Features.Welcome;
using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum;

internal sealed class Program
{
    private static int Main()
    {
        ApplicationContent content;
        try
        {
            content = ApplicationContent.LoadAndValidate(new EmbeddedContentSource());
        }
        catch (ContentValidationException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 2;
        }

        if (Console.IsInputRedirected)
        {
            Console.Error.WriteLine("Lexicanum is an interactive application and requires a terminal.");
            return 1;
        }

        var app = new LexicanumApp(AnsiConsole.Console, content);

        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            app.HandleInterrupt();
        };

        app.Run();
        return 0;
    }
}

/// <summary>
/// All content the application ships with, loaded and validated once at startup.
/// </summary>
public sealed record ApplicationContent(
    IReadOnlyList<ContentPack<LexiconEntry>> LexiconPacks,
    IReadOnlyList<ContentPack<QuizQuestion>> QuizPacks,
    IReadOnlyList<ContentPack<CodeExercise>> ExercisePacks)
{
    public static ApplicationContent LoadAndValidate(IContentSource source)
    {
        var lexiconPacks = source.LoadPacks<LexiconEntry>("lexicon");
        var quizPacks = source.LoadPacks<QuizQuestion>("quizzes");
        var exercisePacks = source.LoadPacks<CodeExercise>("exercises");

        ContentValidator.Validate(lexiconPacks, quizPacks, exercisePacks);

        return new ApplicationContent(lexiconPacks, quizPacks, exercisePacks);
    }
}

/// <summary>
/// The composition root: wires the console, services, content, and menu tree together
/// and owns the application lifecycle (welcome, main loop, save, farewell).
/// </summary>
public class LexicanumApp
{
    private readonly IAnsiConsole _console;
    private readonly ApplicationContent _content;
    private readonly ScoreService _scoreService;

    public LexicanumApp(IAnsiConsole console, ApplicationContent content)
    {
        _console = console;
        _content = content;
        _scoreService = new ScoreService();
    }

    public void Run()
    {
        var welcome = new WelcomeScreen(_console);
        welcome.Show();
        _scoreService.SetPlayerName(welcome.GetPlayerName());

        var rootMenu = MenuNode.Branch("LEXICANUM - Main Menu", null,
            LexiconMenu.CreateMenuNode(_content.LexiconPacks),
            QuizScreen.CreateMenuNode(_content.QuizPacks, _scoreService),
            CodeTrainerScreen.CreateMenuNode(_content.ExercisePacks, _scoreService),
            ScoreboardScreen.CreateMenuNode(_scoreService));

        var navigator = new ScreenNavigator(_console);
        navigator.Run(new MenuScreen(rootMenu, _scoreService, isRoot: true));

        SaveScoreSafely();
        ShowFarewell();
    }

    /// <summary>
    /// Ctrl+C handler: persists the session before terminating with a clean exit code.
    /// </summary>
    public void HandleInterrupt()
    {
        SaveScoreSafely();
        _console.WriteLine();
        _console.ShowNarrator("Fleeing mid-session? Your score is saved. The shame is yours to keep.");
        Environment.Exit(0);
    }

    private void ShowFarewell()
    {
        _console.ShowScreenHeader("Session Summary");
        _console.ShowSessionSummary(_scoreService.CurrentScore);
        _console.ShowNarrator($"Until next time, {_scoreService.CurrentScore.PlayerName}. Try not to forget everything you learned.");
        _console.WaitForKey();
    }

    private void SaveScoreSafely()
    {
        try
        {
            _scoreService.SaveScore();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            _console.ShowError($"Could not save your score: {ex.Message}");
        }
    }
}
