using Lexicanum.Core.Scoring;
using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.Scoreboard;

public enum ScoreboardView
{
    Leaderboard,
    CurrentSession,
    PlayerHistory
}

public sealed class ScoreboardScreen : IScreen
{
    private readonly ScoreboardView _view;
    private readonly ScoreService _scoreService;

    public string Title => _view switch
    {
        ScoreboardView.Leaderboard => "Top 10 Leaderboard",
        ScoreboardView.CurrentSession => "Session Summary",
        ScoreboardView.PlayerHistory => $"Score History - {_scoreService.CurrentScore.PlayerName}",
        _ => "Scoreboard"
    };

    public ScoreboardScreen(ScoreboardView view, ScoreService scoreService)
    {
        _view = view;
        _scoreService = scoreService;
    }

    public ScreenResult Run(IAnsiConsole console)
    {
        console.ShowScreenHeader(Title);

        if (_view != ScoreboardView.CurrentSession && _scoreService.LoadScoreboard().LoadFailed)
        {
            console.ShowError("Existing highscores could not be read; showing a fresh scoreboard.");
        }

        switch (_view)
        {
            case ScoreboardView.Leaderboard:
                console.ShowScoreboard(_scoreService.GetTopScores(10));
                break;
            case ScoreboardView.CurrentSession:
                console.ShowSessionSummary(_scoreService.CurrentScore);
                break;
            case ScoreboardView.PlayerHistory:
                console.ShowScoreboard(_scoreService.GetPlayerHistory(_scoreService.CurrentScore.PlayerName));
                break;
        }

        console.WaitForKey();
        return ScreenResult.Pop;
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
