namespace Lexicanum.Core.Content;

/// <summary>
/// One lexicon reference page. The body is markdown-like text
/// (## headings, ``` code fences, bullet lines).
/// </summary>
public sealed record LexiconEntry
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Body { get; init; }
}
