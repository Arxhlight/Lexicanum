using Lexicanum.Core.Scoring;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Navigation;

/// <summary>
/// Renders any <see cref="MenuNode"/> as an arrow-key menu. Branch children push
/// another MenuScreen; leaf children push the screen their factory creates.
/// </summary>
public sealed class MenuScreen : IScreen
{
    private readonly MenuNode _node;
    private readonly ScoreService _scoreService;
    private readonly bool _isRoot;

    public string Title => _node.Title;

    public MenuScreen(MenuNode node, ScoreService scoreService, bool isRoot = false)
    {
        _node = node;
        _scoreService = scoreService;
        _isRoot = isRoot;
    }

    public ScreenResult Run(IAnsiConsole console)
    {
        console.ShowScreenHeader(_node.Title, _isRoot ? null : _scoreService.GetTotalScore());

        var options = _node.Children
            .Select(child => child.Description is null ? child.Title : $"{child.Title} - {child.Description}")
            .ToList();

        var choice = console.PromptMenu("Select:", options, _isRoot ? "Exit" : "Back");

        if (choice == -1)
        {
            return _isRoot ? ScreenResult.Exit : ScreenResult.Pop;
        }

        var selected = _node.Children[choice];

        return selected.ScreenFactory is not null
            ? ScreenResult.Push(selected.ScreenFactory())
            : ScreenResult.Push(new MenuScreen(selected, _scoreService));
    }
}
