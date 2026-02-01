namespace Lexicanum.UI
{
    public class MenuRenderer
    {
        private readonly ConsoleHelper _console;

        public MenuRenderer(ConsoleHelper console)
        {
            _console = console;
        }


        public void RenderSimple(string title, List<string> options, int exitIndex = 0, string exitLabel = "Back")
        {
            _console.ShowHeader(title);
            Console.WriteLine();

            for (int i = 0; i < options.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"  [{i + 1}] ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(options[i]);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("─────────────────────────────");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"  [{exitIndex}] {exitLabel}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
