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

    /// <summary>
    /// Appends a score to the scoreboard. When the existing file is unreadable it is
    /// preserved as "<c>.corrupt</c>" instead of being silently replaced.
    /// </summary>
    public void Append(PlayerScore score)
    {
        var loadResult = Load();

        if (loadResult.LoadFailed)
        {
            PreserveUnreadableFile();
        }

        var scores = loadResult.Scores;
        scores.Add(score);

        var json = JsonSerializer.Serialize(scores, SerializerOptions);

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var tempPath = $"{_filePath}.{Path.GetRandomFileName()}.tmp";
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }

    private void PreserveUnreadableFile()
    {
        if (File.Exists(_filePath))
        {
            File.Move(_filePath, _filePath + ".corrupt", overwrite: true);
        }
    }
}
