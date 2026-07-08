namespace Lexicanum.Core.Models;

/// <summary>
/// A player's score for one session. Serves as both the live session state and the persisted scoreboard record.
/// </summary>
public class PlayerScore
{
    public string PlayerName { get; set; } = "Anonymous";
    public Dictionary<string, int> FeatureScores { get; set; } = new();
    public DateTime DateOfPlaying { get; set; } = DateTime.Now;

    public int TotalScore => FeatureScores.Values.Sum();

    public PlayerScore() { }

    public PlayerScore(string playerName)
    {
        PlayerName = playerName;
    }

    public void AddScore(string featureName, int points)
    {
        if (!FeatureScores.ContainsKey(featureName))
        {
            FeatureScores[featureName] = 0;
        }
        FeatureScores[featureName] += points;
    }
}
