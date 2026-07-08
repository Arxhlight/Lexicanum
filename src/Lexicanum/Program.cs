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
        if (Console.IsInputRedirected)
        {
            Console.Error.WriteLine("Lexicanum is an interactive application and requires a terminal.");
            return 1;
        }

        var app = new LexicanumApp(AnsiConsole.Console);

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
/// The composition root: wires the console, services, and menu tree together
/// and owns the application lifecycle (welcome, main loop, save, farewell).
/// </summary>
public class LexicanumApp
{
    private readonly IAnsiConsole _console;
    private readonly ScoreService _scoreService;

    public LexicanumApp(IAnsiConsole console)
    {
        _console = console;
        _scoreService = new ScoreService();
    }

    public void Run()
    {
        var welcome = new WelcomeScreen(_console);
        welcome.Show();
        _scoreService.SetPlayerName(welcome.GetPlayerName());

        var rootMenu = MenuNode.Branch("LEXICANUM - Main Menu", null,
            LexiconContent.CreateMenuNode(),
            QuizScreen.CreateMenuNode(_scoreService),
            CodeTrainerScreen.CreateMenuNode(_scoreService),
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
