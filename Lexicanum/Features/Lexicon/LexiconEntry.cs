using Lexicanum.Core.Interfaces;
using Lexicanum.UI;

namespace Lexicanum.Features.Lexicon
{
    public class LexiconEntry : IContentItem
    {
        public string Title { get; }
        public string Content { get; }

        public LexiconEntry(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public void Display()
        {
            var console = new ConsoleHelper();
            Console.Clear();
            console.ShowHeader(Title);
            Console.WriteLine();
            
            // Parse and display content with syntax highlighting for code blocks
            DisplayFormattedContent(Content);
            
            Console.WriteLine();
            console.WaitForInput();
        }

        private void DisplayFormattedContent(string content)
        {
            var lines = content.Split('\n');
            bool inCodeBlock = false;

            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("```"))
                {
                    inCodeBlock = !inCodeBlock;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(inCodeBlock ? "┌─ Code ─────────────────────" : "└────────────────────────────");
                    Console.ResetColor();
                    continue;
                }

                if (inCodeBlock)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  {line}");
                    Console.ResetColor();
                }
                else if (line.TrimStart().StartsWith("##"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(line.Replace("##", "►"));
                    Console.ResetColor();
                }
                else if (line.TrimStart().StartsWith("-") || line.TrimStart().StartsWith("•"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"  {line}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(line);
                    Console.ResetColor();
                }
            }
        }
    }
}
