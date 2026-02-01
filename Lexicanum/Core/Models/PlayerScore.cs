namespace Lexicanum.Core.Models
{
    /// <summary>
    /// Represents a player's score data - used for both current session and persisted records.
    /// </summary>
    public class PlayerScore
    {
        public string PlayerName { get; set; } = "Anonymous";
        public Dictionary<string, int> FeatureScores { get; set; } = new();
        public DateTime DateOfPlaying { get; set; } = DateTime.Now;

        /// <summary>
        /// Total score computed from all feature scores.
        /// </summary>
        public int TotalScore => FeatureScores.Values.Sum();

        public PlayerScore() { }

        public PlayerScore(string playerName)
        {
            PlayerName = playerName;
        }

        /// <summary>
        /// Add points to a specific feature.
        /// </summary>
        public void AddScore(string featureName, int points)
        {
            if (!FeatureScores.ContainsKey(featureName))
            {
                FeatureScores[featureName] = 0;
            }
            FeatureScores[featureName] += points;
        }

        /// <summary>
        /// Remove points from a specific feature.
        /// </summary>
        public void RemoveScore(string featureName, int points)
        {
            if (FeatureScores.ContainsKey(featureName))
            {
                FeatureScores[featureName] = Math.Max(0, FeatureScores[featureName] - points);
            }
        }

        /// <summary>
        /// Get score for a specific feature.
        /// </summary>
        public int GetFeatureScore(string featureName)
        {
            return FeatureScores.GetValueOrDefault(featureName, 0);
        }

        /// <summary>
        /// Reset all scores.
        /// </summary>
        public void Reset()
        {
            FeatureScores.Clear();
        }
    }
}
