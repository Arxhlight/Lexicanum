using System.Text.Json;
using Lexicanum.Core.Models;

namespace Lexicanum.Core.Services;

/// <summary>
/// Manages the current session score and persists finished sessions to a JSON scoreboard file.
/// </summary>
public class ScoreService
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string _scoreFilePath;
    private readonly PlayerScore _currentScore;

    public PlayerScore CurrentScore => _currentScore;

    public ScoreService(string scoreFilePath = "JSON/highscores.json")
    {
        _scoreFilePath = scoreFilePath;
        _currentScore = new PlayerScore();
    }

    public void SetPlayerName(string name)
    {
        _currentScore.PlayerName = name;
    }

    public void AddScore(string featureName, int points)
    {
        _currentScore.AddScore(featureName, points);
    }

    public int GetTotalScore()
    {
        return _currentScore.TotalScore;
    }

    /// <summary>
    /// Appends the current session score to the scoreboard file.
    /// </summary>
    /// <exception cref="IOException">The scoreboard file could not be written.</exception>
    /// <exception cref="UnauthorizedAccessException">The scoreboard file or directory is not writable.</exception>
    public void SaveScore()
    {
        _currentScore.DateOfPlaying = DateTime.Now;

        var scores = LoadScoreboard();
        scores.Add(_currentScore);

        var json = JsonSerializer.Serialize(scores, SerializerOptions);

        var directory = Path.GetDirectoryName(_scoreFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_scoreFilePath, json);
    }

    /// <summary>
    /// Loads all persisted scores. A missing, unreadable, or corrupt file yields an empty scoreboard.
    /// </summary>
    public List<PlayerScore> LoadScoreboard()
    {
        if (!File.Exists(_scoreFilePath))
        {
            return new List<PlayerScore>();
        }

        try
        {
            var json = File.ReadAllText(_scoreFilePath);
            return JsonSerializer.Deserialize<List<PlayerScore>>(json) ?? new List<PlayerScore>();
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            return new List<PlayerScore>();
        }
    }

    public List<PlayerScore> GetTopScores(int count = 10)
    {
        return LoadScoreboard()
            .OrderByDescending(s => s.TotalScore)
            .Take(count)
            .ToList();
    }

    public List<PlayerScore> GetPlayerHistory(string playerName)
    {
        return LoadScoreboard()
            .Where(s => s.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(s => s.DateOfPlaying)
            .ToList();
    }
}
