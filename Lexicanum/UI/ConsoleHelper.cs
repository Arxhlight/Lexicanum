namespace Lexicanum.UI
{
    public class ConsoleHelper
    {
        public void ShowHeader(string text)
        {
            var width = Math.Max(text.Length + 4, 50);
            var padding = (width - text.Length - 2) / 2;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', width) + "╗");
            Console.WriteLine("║" + new string(' ', padding) + text +
                              new string(' ', width - padding - text.Length) + "║");
            Console.WriteLine("╚" + new string('═', width) + "╝");
            Console.ResetColor();
        }

        public void ShowContentPage(string title, string content)
        {
            Console.Clear();
            ShowHeader(title);
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(content);
            Console.ResetColor();
            
            Console.WriteLine();
            WaitForInput();
        }

        public void ShowBox(string title, string content)
        {
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var maxLength = Math.Max(lines.Max(l => l.Length), title.Length) + 4;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔" + new string('═', maxLength) + "╗");
            Console.WriteLine("║ " + title.PadRight(maxLength - 1) + "║");
            Console.WriteLine("╟" + new string('─', maxLength) + "╢");

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var line in lines)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("║ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(line.PadRight(maxLength - 2));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║");
            }

            Console.WriteLine("╚" + new string('═', maxLength) + "╝");
            Console.ResetColor();
        }

        public void ShowNarrator(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($">> {message}");
            Console.ResetColor();
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[ERROR] {message}");
            Console.ResetColor();
        }

        public void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[OK] {message}");
            Console.ResetColor();
        }

        public void ShowInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WaitForInput(string prompt = "Press Enter to continue...")
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(prompt);
            Console.ResetColor();
            Console.ReadLine();
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}
