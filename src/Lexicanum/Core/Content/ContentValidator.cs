using System.Text.RegularExpressions;

namespace Lexicanum.Core.Content;

/// <summary>
/// Enforces the content contract at startup: non-empty packs, unique ids per area,
/// answer indexes in range, and compilable answer patterns. Violations throw
/// <see cref="ContentValidationException"/> so bad content can never ship silently.
/// </summary>
public static class ContentValidator
{
    public static void Validate(
        IReadOnlyList<ContentPack<LexiconEntry>> lexiconPacks,
        IReadOnlyList<ContentPack<QuizQuestion>> quizPacks,
        IReadOnlyList<ContentPack<CodeExercise>> exercisePacks)
    {
        ValidatePacks("lexicon", lexiconPacks, entry => entry.Id, ValidateLexiconEntry);
        ValidatePacks("quizzes", quizPacks, question => question.Id, ValidateQuizQuestion);
        ValidatePacks("exercises", exercisePacks, exercise => exercise.Id, ValidateCodeExercise);
    }

    private static void ValidatePacks<T>(
        string area,
        IReadOnlyList<ContentPack<T>> packs,
        Func<T, string> idSelector,
        Action<string, T> validateItem)
    {
        if (packs.Count == 0)
        {
            throw new ContentValidationException(area, "no content packs found");
        }

        var seenIds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var pack in packs)
        {
            var packName = $"{area}/{pack.Category}";

            if (string.IsNullOrWhiteSpace(pack.Category))
            {
                throw new ContentValidationException(packName, "category must not be empty");
            }

            if (pack.Items.Count == 0)
            {
                throw new ContentValidationException(packName, "pack has no items");
            }

            foreach (var item in pack.Items)
            {
                var id = idSelector(item);

                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ContentValidationException(packName, "item has an empty id");
                }

                if (!seenIds.Add(id))
                {
                    throw new ContentValidationException(packName, $"duplicate id '{id}'");
                }

                validateItem(packName, item);
            }
        }
    }

    private static void ValidateLexiconEntry(string packName, LexiconEntry entry)
    {
        RequireNonEmpty(packName, entry.Id, entry.Title, nameof(entry.Title));
        RequireNonEmpty(packName, entry.Id, entry.Body, nameof(entry.Body));
    }

    private static void ValidateQuizQuestion(string packName, QuizQuestion question)
    {
        RequireNonEmpty(packName, question.Id, question.Question, nameof(question.Question));
        RequireNonEmpty(packName, question.Id, question.Explanation, nameof(question.Explanation));

        if (question.Options.Count < 2)
        {
            throw new ContentValidationException(packName, $"'{question.Id}' needs at least 2 options");
        }

        if (question.CorrectIndex < 0 || question.CorrectIndex >= question.Options.Count)
        {
            throw new ContentValidationException(packName,
                $"'{question.Id}' correctIndex {question.CorrectIndex} is out of range for {question.Options.Count} options");
        }
    }

    private static void ValidateCodeExercise(string packName, CodeExercise exercise)
    {
        RequireNonEmpty(packName, exercise.Id, exercise.Name, nameof(exercise.Name));
        RequireNonEmpty(packName, exercise.Id, exercise.Description, nameof(exercise.Description));
        RequireNonEmpty(packName, exercise.Id, exercise.CorrectExample, nameof(exercise.CorrectExample));
        RequireNonEmpty(packName, exercise.Id, exercise.Feedback, nameof(exercise.Feedback));

        try
        {
            _ = new Regex(exercise.AnswerPattern);
        }
        catch (ArgumentException ex)
        {
            throw new ContentValidationException(packName, $"'{exercise.Id}' answerPattern does not compile: {ex.Message}");
        }
    }

    private static void RequireNonEmpty(string packName, string itemId, string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ContentValidationException(packName, $"'{itemId}' field {fieldName} must not be empty");
        }
    }
}
