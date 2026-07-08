using Lexicanum.Core.Services;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Startup;

public class MenuSystem
{
    private readonly CategoryRegistry _registry;
    private readonly IAnsiConsole _console;
    private readonly NavigationManager _navigation;

    public MenuSystem(CategoryRegistry registry, IAnsiConsole console, NavigationManager navigation)
    {
        _registry = registry;
        _console = console;
        _navigation = navigation;
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            _console.ShowScreenHeader("LEXICANUM - Main Menu");

            var categories = _registry.GetAllCategories().ToList();
            var options = categories.Select(c => c.Name).ToList();

            var choice = _console.PromptMenu("Select a module:", options, "Exit");

            if (choice == -1)
            {
                _console.ShowNarrator("Leaving so soon? I expected as much from you.");
                _console.WaitForKey();
                return;
            }

            _navigation.NavigateToCategory(categories[choice]);
        }
    }
}
