using Lexicanum.Core.Data;
using Lexicanum.Core.Services;
using Lexicanum.Startup;
using Lexicanum.UI;

namespace Lexicanum;

internal sealed class Program
{
    private static void Main()
    {
        var app = new LexicanumApp();
        app.Run();
    }
}

public class LexicanumApp
{
    private readonly ConsoleHelper _console;
    private readonly MenuRenderer _renderer;
    private readonly InputHandler _input;
    private readonly CategoryRegistry _registry;
    private readonly NavigationManager _navigation;
    private readonly MenuSystem _menuSystem;
    private readonly WelcomeScreen _welcomeScreen;
    private readonly ScoreService _scoreService;
    private readonly ScoreRenderer _scoreRenderer;

    private string _playerName = "Anonymous";

    public LexicanumApp()
    {
        _console = new ConsoleHelper();
        _renderer = new MenuRenderer(_console);
        _input = new InputHandler(_console);
        _scoreRenderer = new ScoreRenderer(_console);
        _registry = new CategoryRegistry();
        _scoreService = new ScoreService();
        _navigation = new NavigationManager(_console, _renderer, _input, _scoreService, _scoreRenderer);
        _menuSystem = new MenuSystem(_registry, _console, _renderer, _input, _navigation);
        _welcomeScreen = new WelcomeScreen(_console);

        LoadCategories();
    }

    private void LoadCategories()
    {
        var loader = new ContentLoader(_registry);
        var categories = ContentRepository.GetAllCategories(_console, _input, _scoreService, _scoreRenderer);
        loader.LoadCategories(categories);
    }

    public void Run()
    {
        _welcomeScreen.Show();
        _playerName = _welcomeScreen.GetPlayerName();
        _scoreService.SetPlayerName(_playerName);

        _menuSystem.ShowMainMenu();

        SaveScoreSafely();

        _console.ClearScreen();
        _scoreRenderer.RenderSessionSummary(_scoreService.CurrentScore);
        _console.ShowNarrator($"Until next time, {_playerName}. Try not to forget everything you learned.");
        _console.WaitForInput();
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
