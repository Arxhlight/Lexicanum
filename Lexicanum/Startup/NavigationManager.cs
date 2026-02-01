using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Services;
using Lexicanum.UI;

namespace Lexicanum.Startup
{
    public class NavigationManager
    {
        private readonly ConsoleHelper _console;
        private readonly MenuRenderer _renderer;
        private readonly InputHandler _input;
        private readonly ScoreService _scoreService;
        private readonly ScoreRenderer _scoreRenderer;
        private readonly Stack<string> _navigationStack = new();

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
            _navigationStack.Push(category.Name);

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
                    _navigationStack.Pop();
                    return;
                }

                var selectedSubCategory = subCategories[choice - 1];
                NavigateToSubCategory(selectedSubCategory);
            }
        }

        public void NavigateToSubCategory(ISubCategory subCategory)
        {
            _navigationStack.Push(subCategory.Name);

            while (true)
            {
                _console.ClearScreen();
                _scoreRenderer.RenderScoreCorner(_scoreService.GetTotalScore());

                var subCategories = subCategory.SubCategories;
                var contentItems = subCategory.ContentItems;
                
                // If no subcategories and no content items, execute custom action
                if (subCategories.Count == 0 && contentItems.Count == 0)
                {
                    subCategory.Execute();
                    _navigationStack.Pop();
                    return;
                }

                // Build combined options list: subcategories first, then content items
                var options = new List<string>();
                
                // Add subcategories
                foreach (var sub in subCategories)
                {
                    options.Add($"{sub.Name} - {sub.Description}");
                }
                
                // Add content items
                foreach (var content in contentItems)
                {
                    options.Add($"{content.Title}");
                }

                _renderer.RenderSimple($"{subCategory.Name}", options, 0, "Back");

                var choice = _input.GetMenuChoice(options.Count);

                if (choice == 0)
                {
                    _navigationStack.Pop();
                    return;
                }

                // Determine if selection is a subcategory or content item
                if (choice <= subCategories.Count)
                {
                    // Selected a nested subcategory - recurse
                    var selectedSubCategory = subCategories[choice - 1];
                    NavigateToSubCategory(selectedSubCategory);
                }
                else
                {
                    // Selected a content item
                    var contentIndex = choice - subCategories.Count - 1;
                    var selectedContent = contentItems[contentIndex];
                    DisplayContent(selectedContent);
                }
            }
        }

        public void DisplayContent(IContentItem content)
        {
            content.Display();
        }

        public string GetCurrentPath()
        {
            return string.Join(" > ", _navigationStack.Reverse());
        }
    }
}
