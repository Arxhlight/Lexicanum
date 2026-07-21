namespace Lexicanum.Core.Scoring;

/// <summary>
/// The current session's score plus queries over the persisted scoreboard.
/// Persistence itself is delegated to the injected <see cref="IScoreStore"/>.
/// </summary>
public class ScoreService
{
    private readonly IScoreStore _store;
    private readonly PlayerScore _currentScore = new();

    public PlayerScore CurrentScore => _currentScore;

    public ScoreService(IScoreStore store)
    {
        _store = store;
    }

    public void SetPlayerName(string name)
    {
        _currentScore.PlayerName = name;
    }

    public void AddScore(string featureId, int points)
    {
        _currentScore.AddScore(featureId, points);
    }

    public int GetTotalScore()
    {
        return _currentScore.TotalScore;
    }

    /// <inheritdoc cref="IScoreStore.Append"/>
    public void SaveScore()
    {
        _currentScore.DateOfPlaying = DateTime.Now;
        _store.Append(_currentScore);
    }

    public ScoreboardLoadResult LoadScoreboard()
    {
        return _store.Load();
    }
}
