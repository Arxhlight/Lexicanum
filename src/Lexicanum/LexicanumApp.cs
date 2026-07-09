using Lexicanum.Core.Content;
using Lexicanum.Core.Scoring;
using Lexicanum.Features.CodeTrainer;
using Lexicanum.Features.Lexicon;
using Lexicanum.Features.Quizlet;
using Lexicanum.Features.Scoreboard;
using Lexicanum.Features.Welcome;
using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum;

/// <summary>
/// The composition root: wires the console, services, content, and menu tree together
/// and owns the application lifecycle (welcome, main loop, save, farewell).
/// </summary>
public class LexicanumApp
{
    private readonly IAnsiConsole _console;
    private readonly ApplicationContent _content;
    private readonly ScoreService _scoreService;
    private int _sessionPersisted;

    public LexicanumApp(IAnsiConsole console, ApplicationContent content)
        : this(console, content, new ScoreService(new JsonScoreStore(DefaultScoreFilePath())))
    {
    }

    public LexicanumApp(IAnsiConsole console, ApplicationContent content, ScoreService scoreService)
    {
        _console = console;
        _content = content;
        _scoreService = scoreService;
    }

    private static string DefaultScoreFilePath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Lexicanum",
            "highscores.json");
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
    /// Ctrl+C handler: persists the session when there is a score worth keeping,
    /// then terminates with a clean exit code. Runs on a threadpool thread, so
    /// saving is guarded to happen at most once per session.
    /// </summary>
    public void HandleInterrupt()
    {
        _console.WriteLine();

        if (_scoreService.CurrentScore.TotalScore > 0)
        {
            SaveScoreSafely();
            _console.ShowNarrator("Fleeing mid-session? Your score is saved. The shame is yours to keep.");
        }
        else
        {
            _console.ShowNarrator("Leaving with nothing to show for it? Figures.");
        }

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
        if (Interlocked.Exchange(ref _sessionPersisted, 1) == 1)
        {
            return;
        }

        try
        {
            _scoreService.SaveScore();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            _console.ShowError($"Could not save your score: {ex.Message}");
            Interlocked.Exchange(ref _sessionPersisted, 0);
        }
    }
}
