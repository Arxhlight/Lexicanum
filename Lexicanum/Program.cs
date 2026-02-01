using Lexicanum.Core.Data;
using Lexicanum.Core.Services;
using Lexicanum.Startup;
using Lexicanum.UI;

namespace Lexicanum
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new LexicanumApp();
            app.Run();
        }
    }

    /// <summary>
    /// Main application class that orchestrates all components.
    /// </summary>
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
            // Initialize UI components
            _console = new ConsoleHelper();
            _renderer = new MenuRenderer(_console);
            _input = new InputHandler(_console);
            _scoreRenderer = new ScoreRenderer(_console);

            // Initialize core services
            _registry = new CategoryRegistry();
            _scoreService = new ScoreService();
            _navigation = new NavigationManager(_console, _renderer, _input, _scoreService, _scoreRenderer);
            _menuSystem = new MenuSystem(_registry, _console, _renderer, _input, _navigation);
            _welcomeScreen = new WelcomeScreen(_console);

            // Load all categories from the content repository
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
            // Show welcome screen and get player name
            _welcomeScreen.Show();
            _playerName = _welcomeScreen.GetPlayerName();
            _scoreService.SetPlayerName(_playerName);

            // Start the main menu loop
            _menuSystem.ShowMainMenu();

            // Save score and show exit message
            _scoreService.SaveScore();
            
            _console.ClearScreen();
            _scoreRenderer.RenderSessionSummary(_scoreService.CurrentScore);
            _console.ShowNarrator($"Until next time, {_playerName}. Try not to forget everything you learned.");
            _console.WaitForInput();
        }
    }
}