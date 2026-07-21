namespace Lexicanum.Core.Content;

/// <summary>
/// All content the application ships with, loaded and validated once at startup.
/// </summary>
public sealed record ApplicationContent(
    IReadOnlyList<ContentPack<LexiconEntry>> LexiconPacks,
    IReadOnlyList<ContentPack<QuizQuestion>> QuizPacks,
    IReadOnlyList<ContentPack<CodeExercise>> ExercisePacks)
{
    public static ApplicationContent LoadAndValidate(IContentSource source)
    {
        var lexiconPacks = source.LoadPacks<LexiconEntry>("lexicon");
        var quizPacks = source.LoadPacks<QuizQuestion>("quizzes");
        var exercisePacks = source.LoadPacks<CodeExercise>("exercises");

        ContentValidator.Validate(lexiconPacks, quizPacks, exercisePacks);

        return new ApplicationContent(lexiconPacks, quizPacks, exercisePacks);
    }
}
