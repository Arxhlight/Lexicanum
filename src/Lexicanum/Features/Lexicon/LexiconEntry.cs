using Lexicanum.Core.Interfaces;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.Lexicon;

public class LexiconEntry : IContentItem
{
    public string Title { get; }
    public string Content { get; }

    public LexiconEntry(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public void Display(IAnsiConsole console)
    {
        console.ShowScreenHeader(Title);
        DisplayFormattedContent(console, Content);
        console.WriteLine();
        console.WaitForKey();
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
