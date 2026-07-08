using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.Lexicon;

/// <summary>
/// Renders one lexicon reference page, styling its markdown-like content
/// (## headings, ``` code fences, bullet lines).
/// </summary>
public sealed class LexiconEntryScreen : IScreen
{
    public string Title { get; }
    public string Content { get; }

    public LexiconEntryScreen(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public ScreenResult Run(IAnsiConsole console)
    {
        console.ShowScreenHeader(Title);
        DisplayFormattedContent(console, Content);
        console.WriteLine();
        console.WaitForKey();
        return ScreenResult.Pop;
    }

    private static void DisplayFormattedContent(IAnsiConsole console, string content)
    {
        var lines = content.Split('\n');
        bool inCodeBlock = false;

        foreach (var line in lines)
        {
            if (line.TrimStart().StartsWith("```", StringComparison.Ordinal))
            {
                inCodeBlock = !inCodeBlock;
                console.Write(new Text(inCodeBlock ? "┌─ Code ─────────────────────" : "└────────────────────────────", Theme.MutedStyle));
                console.WriteLine();
                continue;
            }

            if (inCodeBlock)
            {
                console.Write(new Text($"  {line}", Theme.CodeBlockStyle));
            }
            else if (line.TrimStart().StartsWith("##", StringComparison.Ordinal))
            {
                console.Write(new Text(line.Replace("##", "►"), Theme.AccentStyle));
            }
            else if (line.TrimStart().StartsWith('-') || line.TrimStart().StartsWith('•'))
            {
                console.Write(new Text($"  {line}", Theme.BodyStyle));
            }
            else
            {
                console.Write(new Text(line, Theme.MutedStyle));
            }

            console.WriteLine();
        }
    }
}
