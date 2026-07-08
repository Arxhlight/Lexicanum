using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Services;
using Lexicanum.UI;

namespace Lexicanum.Startup;

public class NavigationManager
{
    private readonly ConsoleHelper _console;
    private readonly MenuRenderer _renderer;
    private readonly InputHandler _input;
    private readonly ScoreService _scoreService;
    private readonly ScoreRenderer _scoreRenderer;

    public NavigationManager(ConsoleHelper console, MenuRenderer renderer, InputHandler input,
        ScoreService scoreService, ScoreRenderer scoreRenderer)
    {
        _console = console;
        _renderer = renderer;
        _input = input;
        _scoreService = scoreService;
        _scoreRenderer = scoreRenderer;
    }

    public void NavigateToCategory(ICategory category)
    {
        while (true)
        {
            _console.ClearScreen();
            _scoreRenderer.RenderScoreCorner(_scoreService.GetTotalScore());

            var subCategories = category.SubCategories;
            var options = subCategories.Select(s => $"{s.Name} - {s.Description}").ToList();

            _renderer.RenderSimple($"{category.Name}", options, 0, "Back");

            var choice = _input.GetMenuChoice(subCategories.Count);

            if (choice == 0)
            {
                return;
            }

            NavigateToSubCategory(subCategories[choice - 1]);
        }
    }

    public void NavigateToSubCategory(ISubCategory subCategory)
    {
        while (true)
        {
            _console.ClearScreen();
            _scoreRenderer.RenderScoreCorner(_scoreService.GetTotalScore());

            var subCategories = subCategory.SubCategories;
            var contentItems = subCategory.ContentItems;

            if (subCategories.Count == 0 && contentItems.Count == 0)
            {
                subCategory.Execute();
                return;
            }

            var options = new List<string>();

            foreach (var sub in subCategories)
            {
                options.Add($"{sub.Name} - {sub.Description}");
            }

            foreach (var content in contentItems)
            {
                options.Add($"{content.Title}");
            }

            _renderer.RenderSimple($"{subCategory.Name}", options, 0, "Back");

            var choice = _input.GetMenuChoice(options.Count);

            if (choice == 0)
            {
                return;
            }

            if (choice <= subCategories.Count)
            {
                NavigateToSubCategory(subCategories[choice - 1]);
            }
            else
            {
                var contentIndex = choice - subCategories.Count - 1;
                contentItems[contentIndex].Display();
            }
        }
    }
}
