namespace Lexicanum.Core.Scoring;

public interface IScoreStore
{
    ScoreboardLoadResult Load();

    /// <exception cref="IOException">The scoreboard file could not be written.</exception>
    /// <exception cref="UnauthorizedAccessException">The scoreboard location is not writable.</exception>
    void Append(PlayerScore score);
}
