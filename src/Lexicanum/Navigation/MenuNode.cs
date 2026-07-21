using Lexicanum.Core.Content;

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

    /// <summary>
    /// The one convention for turning content packs into navigation:
    /// each pack becomes a leaf whose screen the factory creates.
    /// </summary>
    public static MenuNode BranchFromPacks<T>(
        string title,
        string? description,
        IReadOnlyList<ContentPack<T>> packs,
        Func<ContentPack<T>, IScreen> screenFactory)
    {
        var leaves = packs
            .Select(pack => Leaf(pack.Category, pack.Description, () => screenFactory(pack)))
            .ToArray();

        return Branch(title, description, leaves);
    }
}
