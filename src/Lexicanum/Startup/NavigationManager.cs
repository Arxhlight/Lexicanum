using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Services;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Startup;

public class NavigationManager
{
    private readonly IAnsiConsole _console;
    private readonly ScoreService _scoreService;

    public NavigationManager(IAnsiConsole console, ScoreService scoreService)
    {
        _console = console;
        _scoreService = scoreService;
    }

    public void NavigateToCategory(ICategory category)
    {
        while (true)
        {
            _console.ShowScreenHeader(category.Name, _scoreService.GetTotalScore());

            var subCategories = category.SubCategories;
            var options = subCategories.Select(s => $"{s.Name} - {s.Description}").ToList();

            var choice = _console.PromptMenu("Select:", options, "Back");

            if (choice == -1)
            {
                return;
            }

            NavigateToSubCategory(subCategories[choice]);
        }
    }

    public void NavigateToSubCategory(ISubCategory subCategory)
    {
        while (true)
        {
            _console.ShowScreenHeader(subCategory.Name, _scoreService.GetTotalScore());

            var subCategories = subCategory.SubCategories;
            var contentItems = subCategory.ContentItems;

            if (subCategories.Count == 0 && contentItems.Count == 0)
            {
                subCategory.Execute();
                return;
            }

            var options = subCategories.Select(sub => $"{sub.Name} - {sub.Description}")
                .Concat(contentItems.Select(content => content.Title))
                .ToList();

            var choice = _console.PromptMenu("Select:", options, "Back");

            if (choice == -1)
            {
                return;
            }

            if (choice < subCategories.Count)
            {
                NavigateToSubCategory(subCategories[choice]);
            }
            else
            {
                contentItems[choice - subCategories.Count].Display(_console);
            }
        }
    }
}
