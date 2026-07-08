namespace Lexicanum.UI;

public class InputHandler
{
    private readonly ConsoleHelper _console;

    public InputHandler(ConsoleHelper console)
    {
        _console = console;
    }

    public int GetMenuChoice(int maxOption, int minOption = 0)
    {
        WriteChoicePrompt();

        while (true)
        {
            var input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= minOption && choice <= maxOption)
            {
                return choice;
            }

            _console.ShowError($"Invalid choice. Please enter a number between {minOption} and {maxOption}.");
            WriteChoicePrompt();
        }
    }

    public bool GetYesNoInput(string prompt)
    {
        WritePrompt($"{prompt} (y/n)");

        while (true)
        {
            var input = Console.ReadLine()?.ToLowerInvariant();

            if (input is "y" or "yes")
            {
                return true;
            }
            if (input is "n" or "no")
            {
                return false;
            }

            _console.ShowError("Please enter 'y' or 'n'.");
            WritePrompt($"{prompt} (y/n)");
        }
    }

    public string ReadMultilineInput()
    {
        var sb = new System.Text.StringBuilder();
        string? line;

        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            sb.AppendLine(line);
        }

        return sb.ToString();
    }

    private static void WriteChoicePrompt()
    {
        WritePrompt("Your choice");
    }

    private static void WritePrompt(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($">> {prompt}: ");
        Console.ResetColor();
    }
}
