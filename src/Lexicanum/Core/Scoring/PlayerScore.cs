namespace Lexicanum.Core.Scoring;

/// <summary>
/// A player's score for one session. Serves as both the live session state and the persisted scoreboard record.
/// </summary>
public class PlayerScore
{
    public string PlayerName { get; set; } = "Anonymous";
    public Dictionary<string, int> FeatureScores { get; set; } = new();
    public DateTime DateOfPlaying { get; set; } = DateTime.Now;

    public int TotalScore => FeatureScores.Values.Sum();

    public void AddScore(string featureId, int points)
    {
        if (!FeatureScores.ContainsKey(featureId))
        {
            FeatureScores[featureId] = 0;
        }
        FeatureScores[featureId] += points;
    }
}
