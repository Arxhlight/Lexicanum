namespace Lexicanum.UI
{
    public class InputHandler
    {
        private readonly ConsoleHelper _console;

        public InputHandler(ConsoleHelper console)
        {
            _console = console;
        }

        public int GetMenuChoice(int maxOption, int minOption = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">> Your choice: ");
            Console.ResetColor();

            while (true)
            {
                var input = Console.ReadLine();
                
                if (int.TryParse(input, out int choice) && choice >= minOption && choice <= maxOption)
                {
                    return choice;
                }

                _console.ShowError($"Invalid choice. Please enter a number between {minOption} and {maxOption}.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(">> Your choice: ");
                Console.ResetColor();
            }
        }

        public string GetTextInput(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($">> {prompt}: ");
            Console.ResetColor();
            
            return Console.ReadLine() ?? string.Empty;
        }

        public bool GetYesNoInput(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($">> {prompt} (y/n): ");
            Console.ResetColor();

            while (true)
            {
                var input = Console.ReadLine()?.ToLower();
                
                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;

                _console.ShowError("Please enter 'y' or 'n'.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($">> {prompt} (y/n): ");
                Console.ResetColor();
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
    }
}
