using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Startup;

public class WelcomeScreen
{
    private const string AsciiArt = @"
    ╔═══════════════════════════════════════════════════════════════════════════════════╗
    ║                                                                                   ║
    ║     ██╗     ███████╗██╗  ██╗██╗ ██████╗ █████╗ ███╗   ██╗██╗   ██╗███╗   ███╗     ║
    ║     ██║     ██╔════╝╚██╗██╔╝██║██╔════╝██╔══██╗████╗  ██║██║   ██║████╗ ████║     ║
    ║     ██║     █████╗   ╚███╔╝ ██║██║     ███████║██╔██╗ ██║██║   ██║██╔████╔██║     ║
    ║     ██║     ██╔══╝   ██╔██╗ ██║██║     ██╔══██║██║╚██╗██║██║   ██║██║╚██╔╝██║     ║
    ║     ███████╗███████╗██╔╝ ██╗██║╚██████╗██║  ██║██║ ╚████║╚██████╔╝██║ ╚═╝ ██║     ║
    ║     ╚══════╝╚══════╝╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚═╝     ╚═╝     ║
    ║                                                                                   ║
    ║  ~ Do you have what it takes to get your name scribed onto the black grimoire ~   ║
    ║                                                                                   ║
    ╚═══════════════════════════════════════════════════════════════════════════════════╝
";

    private static readonly string[] MoxyMessages =
    {
        "Ah, another brave soul enters the Lexicanum...",
        "Oh great, YOU again. Ready to embarrass yourself?",
        "Welcome, mortal. The ancient knowledge awaits your fumbling attempts.",
        "So you think you can code? Let's test that theory, shall we?",
        "The Lexicanum welcomes you... reluctantly.",
        "Another day, another developer thinking they know it all.",
        "Enter, if you dare. The code awaits no one.",
        "Behold! A wild programmer appears. Let's see what you've got."
    };

    private readonly IAnsiConsole _console;

    public WelcomeScreen(IAnsiConsole console)
    {
        _console = console;
    }

    public void Show()
    {
        _console.Clear();
        _console.Write(new Text(AsciiArt, Theme.NarratorStyle));
        _console.WriteLine();
        _console.ShowNarrator(MoxyMessages[Random.Shared.Next(MoxyMessages.Length)]);
        _console.WriteLine();
    }

    public string GetPlayerName()
    {
        var name = _console.Prompt(
            new TextPrompt<string>($"[{Theme.Narrator}]>> State your name, seeker of knowledge:[/]")
                .AllowEmpty());

        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Anonymous Coder";
            _console.ShowNarrator("Too shy to give your name? Fine, I'll call you 'Anonymous Coder'.");
        }
        else
        {
            _console.ShowNarrator($"Welcome, {name}. Try not to disappoint me too much.");
        }

        _console.WriteLine();
        _console.WaitForKey();

        return name;
    }
}
