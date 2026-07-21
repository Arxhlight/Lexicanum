using Lexicanum.Core.Scoring;

namespace Lexicanum.Tests;

/// <summary>
/// The persistence contract: a session score written by <see cref="JsonScoreStore"/>
/// round-trips completely, missing files start fresh, and a corrupt file is reported
/// instead of crashing or being silently swallowed.
/// </summary>
public sealed class PersistenceContractTests : IDisposable
{
    private readonly TempDirectory _tempDirectory = new();
    private readonly string _scoreFilePath;

    public PersistenceContractTests()
    {
        _scoreFilePath = _tempDirectory.FilePath("nested", "highscores.json");
    }

    public void Dispose()
    {
        _tempDirectory.Dispose();
    }

    [Fact]
    public void SavedScore_RoundTripsCompletely()
    {
        var store = new JsonScoreStore(_scoreFilePath);

        var score = new PlayerScore { PlayerName = "Moxy" };
        score.AddScore(FeatureIds.Quizlet, 300);
        score.AddScore(FeatureIds.CodeTrainer, 200);
        score.DateOfPlaying = new DateTime(2026, 7, 8, 12, 30, 0);

        store.Append(score);
        var result = new JsonScoreStore(_scoreFilePath).Load();

        Assert.False(result.LoadFailed);
        var loaded = Assert.Single(result.Scores);
        Assert.Equal("Moxy", loaded.PlayerName);
        Assert.Equal(300, loaded.FeatureScores[FeatureIds.Quizlet]);
        Assert.Equal(200, loaded.FeatureScores[FeatureIds.CodeTrainer]);
        Assert.Equal(500, loaded.TotalScore);
        Assert.Equal(new DateTime(2026, 7, 8, 12, 30, 0), loaded.DateOfPlaying);
    }

    [Fact]
    public void MissingFile_StartsFresh()
    {
        var result = new JsonScoreStore(_scoreFilePath).Load();

        Assert.False(result.LoadFailed);
        Assert.Empty(result.Scores);
    }

    [Fact]
    public void CorruptFile_IsReportedAndStartsFresh()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_scoreFilePath)!);
        File.WriteAllText(_scoreFilePath, "{ this is not json ]");

        var result = new JsonScoreStore(_scoreFilePath).Load();

        Assert.True(result.LoadFailed);
        Assert.Empty(result.Scores);
    }

    [Fact]
    public void AppendingToExistingScoreboard_PreservesEarlierSessions()
    {
        var store = new JsonScoreStore(_scoreFilePath);

        store.Append(new PlayerScore { PlayerName = "First" });
        store.Append(new PlayerScore { PlayerName = "Second" });

        var result = store.Load();

        Assert.False(result.LoadFailed);
        Assert.Equal(new[] { "First", "Second" }, result.Scores.Select(s => s.PlayerName));
    }
}
