using Lexicanum.Core.Content;
using Lexicanum.Features.CodeTrainer;

namespace Lexicanum.Tests;

/// <summary>
/// The content contract: every embedded content file the application ships with
/// deserializes strictly and satisfies <see cref="ContentValidator"/>.
/// Runs against the real assembly resources — no fixtures, no mocks.
/// </summary>
public class ContentContractTests
{
    [Fact]
    public void AllEmbeddedContent_LoadsAndValidates()
    {
        var content = ApplicationContent.LoadAndValidate(new EmbeddedContentSource());

        Assert.NotEmpty(content.LexiconPacks);
        Assert.NotEmpty(content.QuizPacks);
        Assert.NotEmpty(content.ExercisePacks);
    }

    [Fact]
    public void EveryEmbeddedContentResource_BelongsToAKnownArea()
    {
        var knownAreaPrefixes = new[]
        {
            "Lexicanum.Content.lexicon.",
            "Lexicanum.Content.quizzes.",
            "Lexicanum.Content.exercises."
        };

        var contentResources = typeof(EmbeddedContentSource).Assembly
            .GetManifestResourceNames()
            .Where(name => name.StartsWith("Lexicanum.Content.", StringComparison.Ordinal));

        Assert.All(contentResources, name =>
            Assert.True(
                knownAreaPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.Ordinal)),
                $"Resource '{name}' is not in a known content area and would be silently ignored."));
    }

    [Fact]
    public void EveryCodeExercise_CorrectExample_PassesItsOwnValidator()
    {
        var content = ApplicationContent.LoadAndValidate(new EmbeddedContentSource());

        foreach (var pack in content.ExercisePacks)
        {
            foreach (var exercise in pack.Items)
            {
                Assert.True(
                    CodeAnswerValidator.IsCorrect(exercise, exercise.CorrectExample),
                    $"Exercise '{exercise.Id}' in pack '{pack.Category}': its own correctExample does not match its answerPattern.");
            }
        }
    }
}
