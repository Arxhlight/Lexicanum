using Lexicanum.Core.Content;

namespace Lexicanum.Features.Quizlet;

/// <summary>
/// A question whose answer options have been shuffled, tracking the correct answer's new position.
/// </summary>
internal sealed class ShuffledQuestion
{
    public QuizQuestion Original { get; }
    public string[] ShuffledOptions { get; }
    public int ShuffledCorrectIndex { get; }

    public ShuffledQuestion(QuizQuestion original, Random random)
    {
        Original = original;

        var optionsWithIndices = ShuffleUntilOrderChanges(original.Options, random);

        ShuffledOptions = optionsWithIndices.Select(x => x.Option).ToArray();
        ShuffledCorrectIndex = Array.FindIndex(optionsWithIndices, x => x.OriginalIndex == original.CorrectIndex);
    }

    public bool CheckAnswer(int answerIndex) => answerIndex == ShuffledCorrectIndex;

    public string GetCorrectAnswer() => ShuffledOptions[ShuffledCorrectIndex];

    /// <summary>
    /// Shuffles until the result differs from the original order so a shuffled
    /// question never presents its options exactly as authored.
    /// </summary>
    private static (string Option, int OriginalIndex)[] ShuffleUntilOrderChanges(IReadOnlyList<string> options, Random random)
    {
        var optionsWithIndices = options
            .Select((option, index) => (Option: option, OriginalIndex: index))
            .ToArray();

        if (optionsWithIndices.Length < 2)
        {
            return optionsWithIndices;
        }

        do
        {
            random.Shuffle(optionsWithIndices);
        }
        while (IsOriginalOrder(optionsWithIndices));

        return optionsWithIndices;
    }

    private static bool IsOriginalOrder((string Option, int OriginalIndex)[] optionsWithIndices)
    {
        return optionsWithIndices.Select((x, i) => x.OriginalIndex == i).All(same => same);
    }
}
