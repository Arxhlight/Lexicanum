using Lexicanum.Core.Content;
using Spectre.Console;

namespace Lexicanum;

internal sealed class Program
{
    private const int ExitCodeNotATerminal = 1;
    private const int ExitCodeInvalidContent = 2;

    private static int Main()
    {
        ApplicationContent content;
        try
        {
            content = ApplicationContent.LoadAndValidate(new EmbeddedContentSource());
        }
        catch (ContentValidationException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return ExitCodeInvalidContent;
        }

        if (Console.IsInputRedirected)
        {
            Console.Error.WriteLine("Lexicanum is an interactive application and requires a terminal.");
            return ExitCodeNotATerminal;
        }

        var app = new LexicanumApp(AnsiConsole.Console, content);

        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            app.HandleInterrupt();
        };

        app.Run();
        return 0;
    }
}
