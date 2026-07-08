using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.Scoreboard;

public class ScoreboardViewer
{
    private readonly ScoreService _scoreService;
    private readonly IAnsiConsole _console;

    public ScoreboardViewer(ScoreService scoreService, IAnsiConsole console)
    {
        _scoreService = scoreService;
        _console = console;
    }

    public void ShowScoreboard()
    {
        _console.ShowScreenHeader("Top 10 Leaderboard");
        _console.ShowScoreboard(_scoreService.GetTopScores(10));
        _console.WaitForKey();
    }

    public void ShowCurrentSession()
    {
        _console.ShowScreenHeader("Session Summary");
        _console.ShowSessionSummary(_scoreService.CurrentScore);
        _console.WaitForKey();
    }

    public void ShowPlayerHistory()
    {
        var playerName = _scoreService.CurrentScore.PlayerName;

        _console.ShowScreenHeader($"Score History - {playerName}");
        _console.ShowScoreboard(_scoreService.GetPlayerHistory(playerName));
        _console.WaitForKey();
    }

    public static Category CreateScoreboardCategory(ScoreService scoreService, IAnsiConsole console)
    {
        var viewer = new ScoreboardViewer(scoreService, console);
        var category = new Category("Scoreboard", "View scores and leaderboard");

        category.AddSubCategory(new SubCategory("Leaderboard", "View top 10 scores", _ =>
        {
            viewer.ShowScoreboard();
        }));

        category.AddSubCategory(new SubCategory("Current Session", "View your current session score", _ =>
        {
            viewer.ShowCurrentSession();
        }));

        category.AddSubCategory(new SubCategory("My History", "View your score history", _ =>
        {
            viewer.ShowPlayerHistory();
        }));

        return category;
    }
}
