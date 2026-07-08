namespace Lexicanum.Navigation;

/// <summary>
/// One node of the menu tree. A branch has children; a leaf has a screen factory.
/// The whole application menu is data built from these nodes.
/// </summary>
public sealed record MenuNode(
    string Title,
    string? Description,
    IReadOnlyList<MenuNode> Children,
    Func<IScreen>? ScreenFactory)
{
    public static MenuNode Branch(string title, string? description, params MenuNode[] children)
    {
        return new MenuNode(title, description, children, null);
    }

    public static MenuNode Leaf(string title, string? description, Func<IScreen> screenFactory)
    {
        return new MenuNode(title, description, Array.Empty<MenuNode>(), screenFactory);
    }
}
