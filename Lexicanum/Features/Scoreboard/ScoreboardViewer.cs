using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.UI;

namespace Lexicanum.Features.Scoreboard
{
    /// <summary>
    /// Feature for viewing scoreboard, personal scores, and score history.
    /// </summary>
    public class ScoreboardViewer
    {
        private readonly ScoreService _scoreService;
        private readonly ScoreRenderer _renderer;
        private readonly ConsoleHelper _console;
        private readonly InputHandler _input;

        public ScoreboardViewer(ScoreService scoreService, ScoreRenderer renderer, ConsoleHelper console, InputHandler input)
        {
            _scoreService = scoreService;
            _renderer = renderer;
            _console = console;
            _input = input;
        }

        public void ShowScoreboard()
        {
            _console.ClearScreen();
            var topScores = _scoreService.GetTopScores(10);
            _renderer.RenderScoreboard(topScores, "Top 10 Leaderboard");
            _console.WaitForInput();
        }

        public void ShowCurrentSession()
        {
            _console.ClearScreen();
            _renderer.RenderSessionSummary(_scoreService.CurrentScore);
            _console.WaitForInput();
        }

        public void ShowPlayerHistory()
        {
            _console.ClearScreen();
            var playerName = _scoreService.CurrentScore.PlayerName;
            var history = _scoreService.GetPlayerHistory(playerName);
            
            _renderer.RenderScoreboard(history, $"Score History - {playerName}");
            _console.WaitForInput();
        }

        /// <summary>
        /// Creates the Scoreboard category for the main menu.
        /// </summary>
        public static Category CreateScoreboardCategory(ScoreService scoreService, ScoreRenderer renderer, ConsoleHelper console, InputHandler input)
        {
            var viewer = new ScoreboardViewer(scoreService, renderer, console, input);
            var category = new Category("Scoreboard", "View scores and leaderboard");

            // Top Scores
            var topScoresSub = new SubCategory("Leaderboard", "View top 10 scores", _ =>
            {
                viewer.ShowScoreboard();
            });
            category.AddSubCategory(topScoresSub);

            // Current Session
            var currentSessionSub = new SubCategory("Current Session", "View your current session score", _ =>
            {
                viewer.ShowCurrentSession();
            });
            category.AddSubCategory(currentSessionSub);

            // Player History
            var historySub = new SubCategory("My History", "View your score history", _ =>
            {
                viewer.ShowPlayerHistory();
            });
            category.AddSubCategory(historySub);

            return category;
        }
    }
}
