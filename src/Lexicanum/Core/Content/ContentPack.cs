namespace Lexicanum.Core.Content;

/// <summary>
/// One content JSON file: a named group of items that becomes a branch (or single leaf)
/// in the menu tree.
/// </summary>
public sealed record ContentPack<T>
{
    public required string Category { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<T> Items { get; init; }
}
