using Lexicanum.Core.Models;

namespace Lexicanum.UI;

public class ScoreRenderer
{
    private readonly ConsoleHelper _console;

    public ScoreRenderer(ConsoleHelper console)
    {
        _console = console;
    }

    /// <summary>
    /// Renders the score in the top-right corner. Call before rendering any screen content.
    /// Does nothing when output is redirected or the window is too narrow, because
    /// cursor positioning is unavailable there.
    /// </summary>
    public void RenderScoreCorner(int score)
    {
        if (Console.IsOutputRedirected)
        {
            return;
        }

        var scoreText = $"Score: {score}";
        var left = Console.WindowWidth - scoreText.Length - 2;

        if (left <= 0)
        {
            return;
        }

        var currentLeft = Console.CursorLeft;
        var currentTop = Console.CursorTop;

        Console.SetCursorPosition(left, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(scoreText);
        Console.ResetColor();

        Console.SetCursorPosition(currentLeft, currentTop);
    }

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

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  {"Rank",-6}{"Player",-20}{"Score",-10}{"Date",-20}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  " + new string('-', 54));
        Console.ResetColor();

        for (int i = 0; i < scores.Count; i++)
        {
            var s = scores[i];
            var rank = i + 1;

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
}
