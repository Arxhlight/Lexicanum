using Lexicanum.Core.Data;
using Lexicanum.Core.Services;
using Lexicanum.Startup;
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

public class LexicanumApp
{
    private readonly IAnsiConsole _console;
    private readonly CategoryRegistry _registry;
    private readonly MenuSystem _menuSystem;
    private readonly WelcomeScreen _welcomeScreen;
    private readonly ScoreService _scoreService;

    public LexicanumApp(IAnsiConsole console)
    {
        _console = console;
        _registry = new CategoryRegistry();
        _scoreService = new ScoreService();
        _menuSystem = new MenuSystem(_registry, console, new NavigationManager(console, _scoreService));
        _welcomeScreen = new WelcomeScreen(console);

        LoadCategories();
    }

    private void LoadCategories()
    {
        var loader = new ContentLoader(_registry);
        loader.LoadCategories(ContentRepository.GetAllCategories(_console, _scoreService));
    }

    public void Run()
    {
        _welcomeScreen.Show();
        _scoreService.SetPlayerName(_welcomeScreen.GetPlayerName());

        _menuSystem.ShowMainMenu();

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
