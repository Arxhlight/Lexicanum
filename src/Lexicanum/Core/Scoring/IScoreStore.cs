namespace Lexicanum.Core.Scoring;

/// <summary>
/// Result of reading the scoreboard. <see cref="LoadFailed"/> is true when an existing
/// file could not be read (corrupt or locked) and the scoreboard started fresh.
/// </summary>
public sealed record ScoreboardLoadResult(List<PlayerScore> Scores, bool LoadFailed);

public interface IScoreStore
{
    ScoreboardLoadResult Load();

    /// <exception cref="IOException">The scoreboard file could not be written.</exception>
    /// <exception cref="UnauthorizedAccessException">The scoreboard location is not writable.</exception>
    void Append(PlayerScore score);
}
