using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.UI;

namespace Lexicanum.Startup
{
    public class MenuSystem
    {
        private readonly CategoryRegistry _registry;
        private readonly ConsoleHelper _console;
        private readonly MenuRenderer _renderer;
        private readonly InputHandler _input;
        private readonly NavigationManager _navigation;

        public MenuSystem(CategoryRegistry registry, ConsoleHelper console, MenuRenderer renderer, 
            InputHandler input, NavigationManager navigation)
        {
            _registry = registry;
            _console = console;
            _renderer = renderer;
            _input = input;
            _navigation = navigation;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                _console.ClearScreen();
                
                var categories = _registry.GetAllCategories().ToList();
                var options = categories.Select(c => c.Name).ToList();
                
                _renderer.RenderSimple("LEXICANUM - Main Menu", options, 0, "Exit");
                
                var choice = _input.GetMenuChoice(categories.Count);
                
                if (choice == 0)
                {
                    _console.ShowNarrator("Leaving so soon? I expected as much from you.");
                    _console.WaitForInput();
                    break;
                }

                var selectedCategory = categories[choice - 1];
                _navigation.NavigateToCategory(selectedCategory);
            }
        }
    }
}
