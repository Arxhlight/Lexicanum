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
        ShuffledCorrectIndex = optionsWithIndices.FindIndex(x => x.OriginalIndex == original.CorrectIndex);
    }

    public bool CheckAnswer(int answerIndex) => answerIndex == ShuffledCorrectIndex;

    public string GetCorrectAnswer() => ShuffledOptions[ShuffledCorrectIndex];

    /// <summary>
    /// Fisher-Yates shuffle, repeated until the result differs from the original order
    /// so a shuffled question never presents its options unshuffled.
    /// </summary>
    private static List<(string Option, int OriginalIndex)> ShuffleUntilOrderChanges(IReadOnlyList<string> options, Random random)
    {
        List<(string Option, int OriginalIndex)> optionsWithIndices;

        do
        {
            optionsWithIndices = options
                .Select((opt, idx) => (Option: opt, OriginalIndex: idx))
                .ToList();

            for (int i = optionsWithIndices.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (optionsWithIndices[i], optionsWithIndices[j]) = (optionsWithIndices[j], optionsWithIndices[i]);
            }
        }
        while (options.Count > 1 &&
               optionsWithIndices.Select((x, i) => x.OriginalIndex == i).All(same => same));

        return optionsWithIndices;
    }
}
