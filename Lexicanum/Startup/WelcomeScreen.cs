using Lexicanum.UI;

namespace Lexicanum.Startup
{
    public class WelcomeScreen
    {
        private readonly ConsoleHelper _consoleHelper;
        private readonly string[] _moxyMessages = new[]
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

        public WelcomeScreen(ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }

        public void Show()
        {
            _consoleHelper.ClearScreen();
            ShowAsciiArt();
            ShowMoxyWelcome();
        }

        private void ShowAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
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
");
            Console.ResetColor();
        }

        private void ShowMoxyWelcome()
        {

            var random = new Random();
            var message = _moxyMessages[random.Next(_moxyMessages.Length)];
            
            Console.WriteLine();
            _consoleHelper.ShowNarrator(message);
            Console.WriteLine();
        }

        public string GetPlayerName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">> State your name, seeker of knowledge: ");
            Console.ResetColor();
            
            var name = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Anonymous Coder";
                _consoleHelper.ShowNarrator("Too shy to give your name? Fine, I'll call you 'Anonymous Coder'.");
            }
            else
            {
                _consoleHelper.ShowNarrator($"Welcome, {name}. Try not to disappoint me too much.");
            }

            Console.WriteLine();
            _consoleHelper.WaitForInput();
            
            return name;
        }
    }
}
