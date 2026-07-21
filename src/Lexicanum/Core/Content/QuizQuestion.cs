namespace Lexicanum.Core.Content;

public sealed record QuizQuestion
{
    public required string Id { get; init; }
    public required string Question { get; init; }
    public required IReadOnlyList<string> Options { get; init; }
    public required int CorrectIndex { get; init; }
    public required string Explanation { get; init; }
}
