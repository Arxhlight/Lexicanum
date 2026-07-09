using Lexicanum.Core.Scoring;
using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.Scoreboard;

public sealed class ScoreboardScreen : IScreen
{
    private const int LeaderboardSize = 10;

    private readonly ScoreboardView _view;
    private readonly ScoreService _scoreService;

    public ScoreboardScreen(ScoreboardView view, ScoreService scoreService)
    {
        _view = view;
        _scoreService = scoreService;
    }

    public ScreenResult Run(IAnsiConsole console)
    {
        switch (_view)
        {
            case ScoreboardView.Leaderboard:
                console.ShowScreenHeader($"Top {LeaderboardSize} Leaderboard");
                ShowPersistedScores(console, scores => scores
                    .OrderByDescending(s => s.TotalScore)
                    .Take(LeaderboardSize));
                break;
            case ScoreboardView.CurrentSession:
                console.ShowScreenHeader("Session Summary");
                console.ShowSessionSummary(_scoreService.CurrentScore);
                break;
            case ScoreboardView.PlayerHistory:
                var playerName = _scoreService.CurrentScore.PlayerName;
                console.ShowScreenHeader($"Score History - {playerName}");
                ShowPersistedScores(console, scores => scores
                    .Where(s => s.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(s => s.DateOfPlaying));
                break;
        }

        console.WaitForKey();
        return ScreenResult.Pop;
    }

    private void ShowPersistedScores(
        IAnsiConsole console,
        Func<IEnumerable<PlayerScore>, IEnumerable<PlayerScore>> query)
    {
        var scoreboard = _scoreService.LoadScoreboard();

        if (scoreboard.LoadFailed)
        {
            console.ShowError("Existing highscores could not be read; showing a fresh scoreboard.");
        }

        console.ShowScoreboard(query(scoreboard.Scores).ToList());
    }

    public static MenuNode CreateMenuNode(ScoreService scoreService)
    {
        return MenuNode.Branch("Scoreboard", "View scores and leaderboard",
            MenuNode.Leaf("Leaderboard", "View top 10 scores",
                () => new ScoreboardScreen(ScoreboardView.Leaderboard, scoreService)),
            MenuNode.Leaf("Current Session", "View your current session score",
                () => new ScoreboardScreen(ScoreboardView.CurrentSession, scoreService)),
            MenuNode.Leaf("My History", "View your score history",
                () => new ScoreboardScreen(ScoreboardView.PlayerHistory, scoreService)));
    }
}
