namespace Lexicanum.Core.Content;

/// <summary>
/// Loads content packs for one area ("lexicon", "quizzes", "exercises").
/// The seam exists so tests can load the exact same data the application ships with.
/// </summary>
public interface IContentSource
{
    IReadOnlyList<ContentPack<T>> LoadPacks<T>(string area);
}
