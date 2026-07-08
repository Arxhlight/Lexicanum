namespace Lexicanum.Core.Content;

/// <summary>
/// One live-coding exercise. <see cref="AnswerPattern"/> is a regex matched against
/// the player's whitespace-normalized code.
/// </summary>
public sealed record CodeExercise
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string CorrectExample { get; init; }
    public required string AnswerPattern { get; init; }
    public required string Feedback { get; init; }
    public string? SuccessFeedback { get; init; }
}
