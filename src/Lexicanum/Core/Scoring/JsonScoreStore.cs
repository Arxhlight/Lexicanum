using System.Text.Json;

namespace Lexicanum.Core.Scoring;

/// <summary>
/// Persists the scoreboard as a JSON file at an injected path.
/// Writes go through a temp file and an atomic move so a crash mid-write
/// never corrupts the existing scoreboard.
/// </summary>
public sealed class JsonScoreStore : IScoreStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _filePath;

    public JsonScoreStore(string filePath)
    {
        _filePath = filePath;
    }

    public ScoreboardLoadResult Load()
    {
        if (!File.Exists(_filePath))
        {
            return new ScoreboardLoadResult(new List<PlayerScore>(), LoadFailed: false);
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            var scores = JsonSerializer.Deserialize<List<PlayerScore>>(json, SerializerOptions) ?? new List<PlayerScore>();
            return new ScoreboardLoadResult(scores, LoadFailed: false);
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            return new ScoreboardLoadResult(new List<PlayerScore>(), LoadFailed: true);
        }
    }

    public void Append(PlayerScore score)
    {
        var scores = Load().Scores;
        scores.Add(score);

        var json = JsonSerializer.Serialize(scores, SerializerOptions);

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var tempPath = _filePath + ".tmp";
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }
}
