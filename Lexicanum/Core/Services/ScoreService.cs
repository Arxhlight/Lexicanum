using System.Text.Json;
using Lexicanum.Core.Models;

namespace Lexicanum.Core.Services
{
    /// <summary>
    /// Manages the current session score and persistence to JSON.
    /// </summary>
    public class ScoreService
    {
        private readonly string _scoreFilePath;
        private PlayerScore _currentScore;

        public PlayerScore CurrentScore => _currentScore;

        public ScoreService(string scoreFilePath = "JSON/highscores.json")
        {
            _scoreFilePath = scoreFilePath;
            _currentScore = new PlayerScore();
        }

        /// <summary>
        /// Set the player name for the current session.
        /// </summary>
        public void SetPlayerName(string name)
        {
            _currentScore.PlayerName = name;
        }

        /// <summary>
        /// Add points to a feature for the current session.
        /// </summary>
        public void AddScore(string featureName, int points)
        {
            _currentScore.AddScore(featureName, points);
        }

        /// <summary>
        /// Remove points from a feature for the current session.
        /// </summary>
        public void RemoveScore(string featureName, int points)
        {
            _currentScore.RemoveScore(featureName, points);
        }

        /// <summary>
        /// Get the total score for the current session.
        /// </summary>
        public int GetTotalScore()
        {
            return _currentScore.TotalScore;
        }

        /// <summary>
        /// Get score for a specific feature in the current session.
        /// </summary>
        public int GetFeatureScore(string featureName)
        {
            return _currentScore.GetFeatureScore(featureName);
        }

        /// <summary>
        /// Reset the current session score.
        /// </summary>
        public void ResetSession()
        {
            var playerName = _currentScore.PlayerName;
            _currentScore = new PlayerScore(playerName);
        }

        /// <summary>
        /// Save the current session score to the JSON file.
        /// </summary>
        public void SaveScore()
        {
            _currentScore.DateOfPlaying = DateTime.Now;
            
            var scores = LoadScoreboard();
            scores.Add(_currentScore);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(scores, options);
            
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_scoreFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(_scoreFilePath, json);
        }

        /// <summary>
        /// Load all scores from the JSON file.
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
            catch
            {
                return new List<PlayerScore>();
            }
        }

        /// <summary>
        /// Get top scores sorted by total score descending.
        /// </summary>
        public List<PlayerScore> GetTopScores(int count = 10)
        {
            return LoadScoreboard()
                .OrderByDescending(s => s.TotalScore)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Get score history for a specific player.
        /// </summary>
        public List<PlayerScore> GetPlayerHistory(string playerName)
        {
            return LoadScoreboard()
                .Where(s => s.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.DateOfPlaying)
                .ToList();
        }
    }
}
