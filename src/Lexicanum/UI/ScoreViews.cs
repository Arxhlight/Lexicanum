using Lexicanum.Core.Scoring;
using Spectre.Console;

namespace Lexicanum.UI;

/// <summary>
/// Score-specific views: session summary and leaderboard tables.
/// </summary>
public static class ScoreViews
{
    public static void ShowSessionSummary(this IAnsiConsole console, PlayerScore score)
    {
        console.MarkupLine($"[{Theme.Body}]Player: {Markup.Escape(score.PlayerName)}[/]");
        console.WriteLine();

        if (score.FeatureScores.Count == 0)
        {
            console.MarkupLine($"[{Theme.Muted}]No scores recorded this session.[/]");
        }
        else
        {
            var chart = new BarChart()
                .Width(60)
                .Label($"[{Theme.Accent}]Score Breakdown[/]")
                .LeftAlignLabel();

            foreach (var feature in score.FeatureScores.OrderByDescending(f => f.Value))
            {
                chart.AddItem(feature.Key, feature.Value, Color.Green);
            }

            console.Write(chart);
        }

        console.WriteLine();
        console.MarkupLine($"[{Theme.Score}]Total Score: {score.TotalScore}[/]");
        console.WriteLine();
    }

    public static void ShowScoreboard(this IAnsiConsole console, IReadOnlyList<PlayerScore> scores)
    {
        if (scores.Count == 0)
        {
            console.MarkupLine($"[{Theme.Muted}]No scores recorded yet.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderStyle(Theme.MutedStyle)
            .AddColumn("Rank")
            .AddColumn("Player")
            .AddColumn("Score")
            .AddColumn("Date");

        for (int i = 0; i < scores.Count; i++)
        {
            var score = scores[i];
            var rank = i + 1;

            var rankColor = rank switch
            {
                1 => Theme.RankGold,
                2 => Theme.RankSilver,
                3 => Theme.RankBronze,
                _ => Theme.Body
            };

            var rankDisplay = rank switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => $"{rank}th"
            };

            table.AddRow(
                $"[{rankColor}]{rankDisplay}[/]",
                $"[{rankColor}]{Markup.Escape(score.PlayerName)}[/]",
                $"[{rankColor}]{score.TotalScore}[/]",
                $"[{rankColor}]{score.DateOfPlaying:yyyy-MM-dd HH:mm}[/]");
        }

        console.Write(table);
        console.WriteLine();
    }
}
