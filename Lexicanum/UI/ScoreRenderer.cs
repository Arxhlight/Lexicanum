using Lexicanum.Core.Models;

namespace Lexicanum.UI
{
    /// <summary>
    /// Handles all score-related display rendering.
    /// </summary>
    public class ScoreRenderer
    {
        private readonly ConsoleHelper _console;

        public ScoreRenderer(ConsoleHelper console)
        {
            _console = console;
        }

        /// <summary>
        /// Render the score in the top-right corner of the console.
        /// Call this before rendering any screen content.
        /// </summary>
        public void RenderScoreCorner(int score)
        {
            var scoreText = $"Score: {score}";
            var left = Console.WindowWidth - scoreText.Length - 2;
            
            if (left > 0)
            {
                var currentLeft = Console.CursorLeft;
                var currentTop = Console.CursorTop;

                Console.SetCursorPosition(left, 0);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(scoreText);
                Console.ResetColor();

                Console.SetCursorPosition(currentLeft, currentTop);
            }
        }

        /// <summary>
        /// Render a full session summary showing score breakdown by feature.
        /// </summary>
        public void RenderSessionSummary(PlayerScore score)
        {
            _console.ShowHeader("Session Summary");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  Player: {score.PlayerName}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Score Breakdown:");
            Console.ForegroundColor = ConsoleColor.White;

            if (score.FeatureScores.Count == 0)
            {
                Console.WriteLine("    No scores recorded this session.");
            }
            else
            {
                foreach (var feature in score.FeatureScores)
                {
                    Console.WriteLine($"    {feature.Key}: {feature.Value}");
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  Total Score: {score.TotalScore}");
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Render the full scoreboard/leaderboard.
        /// </summary>
        public void RenderScoreboard(List<PlayerScore> scores, string title = "Scoreboard")
        {
            _console.ShowHeader(title);
            Console.WriteLine();

            if (scores.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  No scores recorded yet.");
                Console.ResetColor();
                return;
            }

            // Header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  {"Rank",-6}{"Player",-20}{"Score",-10}{"Date",-20}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  " + new string('-', 54));
            Console.ResetColor();

            // Scores
            for (int i = 0; i < scores.Count; i++)
            {
                var s = scores[i];
                var rank = i + 1;

                // Highlight top 3
                Console.ForegroundColor = rank switch
                {
                    1 => ConsoleColor.Yellow,
                    2 => ConsoleColor.Gray,
                    3 => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.White
                };

                var rankDisplay = rank switch
                {
                    1 => "1st",
                    2 => "2nd",
                    3 => "3rd",
                    _ => $"{rank}th"
                };

                Console.WriteLine($"  {rankDisplay,-6}{s.PlayerName,-20}{s.TotalScore,-10}{s.DateOfPlaying:yyyy-MM-dd HH:mm}");
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Render detailed score view for a single player score entry.
        /// </summary>
        public void RenderDetailedScore(PlayerScore score)
        {
            _console.ShowHeader($"Score Details - {score.PlayerName}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  Date: {score.DateOfPlaying:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"  Total Score: {score.TotalScore}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Feature Breakdown:");
            Console.ResetColor();

            if (score.FeatureScores.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    No feature scores recorded.");
                Console.ResetColor();
            }
            else
            {
                foreach (var feature in score.FeatureScores.OrderByDescending(f => f.Value))
                {
                    var bar = new string('█', Math.Min(feature.Value / 10, 30));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"    {feature.Key,-20}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(bar);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" {feature.Value}");
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
