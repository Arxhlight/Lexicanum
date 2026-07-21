using System.Text;
using Spectre.Console;

namespace Lexicanum.UI;

/// <summary>
/// All shared rendering and prompt helpers, as extensions over the injected
/// <see cref="IAnsiConsole"/> so every caller stays testable with a substitute console.
/// </summary>
public static class ConsoleViews
{
    public static void ShowScreenHeader(this IAnsiConsole console, string title, int? score = null)
    {
        console.Clear();

        if (score is not null)
        {
            console.Write(Align.Right(new Markup($"[{Theme.Score}]Score: {score}[/]")));
        }

        console.Write(new Rule($"[{Theme.Accent}]{Markup.Escape(title)}[/]") { Style = Theme.AccentStyle });
        console.WriteLine();
    }

    public static void ShowNarrator(this IAnsiConsole console, string message)
    {
        console.MarkupLine($"[{Theme.Narrator}]>> {Markup.Escape(message)}[/]");
    }

    public static void ShowError(this IAnsiConsole console, string message)
    {
        console.MarkupLine($"[{Theme.Error}][[ERROR]] {Markup.Escape(message)}[/]");
    }

    public static void ShowSuccess(this IAnsiConsole console, string message)
    {
        console.MarkupLine($"[{Theme.Success}][[OK]] {Markup.Escape(message)}[/]");
    }

    public static void ShowInfo(this IAnsiConsole console, string message)
    {
        console.MarkupLine($"[{Theme.Info}]{Markup.Escape(message)}[/]");
    }

    public static void ShowBox(this IAnsiConsole console, string title, string content)
    {
        var panel = new Panel(new Text(content, Theme.BodyStyle))
        {
            Header = new PanelHeader(Markup.Escape(title)),
            Border = BoxBorder.Double,
            BorderStyle = Theme.NarratorStyle,
            Padding = new Padding(1, 0, 1, 0)
        };
        console.Write(panel);
    }

    public static void WaitForKey(this IAnsiConsole console, string prompt = "Press any key to continue...")
    {
        console.MarkupLine($"[{Theme.Muted}]{Markup.Escape(prompt)}[/]");
        console.Input.ReadKey(intercept: true);
    }

    /// <summary>
    /// Arrow-key menu over <paramref name="options"/>. Returns the selected option's index,
    /// or -1 when <paramref name="exitLabel"/> is provided and chosen.
    /// </summary>
    public static int PromptMenu(this IAnsiConsole console, string title, IReadOnlyList<string> options, string? exitLabel = null)
    {
        var prompt = new SelectionPrompt<int>()
            .Title($"[{Theme.Muted}]{Markup.Escape(title)}[/]")
            .PageSize(15)
            .MoreChoicesText($"[{Theme.Muted}](Move up and down to reveal more)[/]")
            .HighlightStyle(Theme.AccentStyle)
            .UseConverter(index => index == -1
                ? $"[{Theme.Narrator}]{Markup.Escape(exitLabel ?? string.Empty)}[/]"
                : Markup.Escape(options[index]))
            .AddChoices(Enumerable.Range(0, options.Count));

        if (exitLabel is not null)
        {
            prompt.AddChoice(-1);
        }

        return console.Prompt(prompt);
    }

    /// <summary>
    /// Reads lines until an empty line is submitted. Lines are joined with newlines.
    /// </summary>
    public static string ReadMultilineInput(this IAnsiConsole console)
    {
        var builder = new StringBuilder();

        while (true)
        {
            var line = console.Prompt(new TextPrompt<string>($"[{Theme.Muted}]>[/]")
                .PromptStyle(Theme.InputStyle)
                .AllowEmpty());

            if (string.IsNullOrWhiteSpace(line))
            {
                return builder.ToString();
            }

            builder.AppendLine(line);
        }
    }
}
