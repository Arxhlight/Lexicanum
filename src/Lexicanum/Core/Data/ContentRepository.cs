using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Services;
using Lexicanum.Features.CodeTrainer;
using Lexicanum.Features.Lexicon;
using Lexicanum.Features.Quizlet;
using Lexicanum.Features.Scoreboard;
using Lexicanum.UI;

namespace Lexicanum.Core.Data;

/// <summary>
/// Central registration point for all content categories. Extending the application
/// with a new category means adding one factory call here.
/// </summary>
public static class ContentRepository
{
    public static IEnumerable<ICategory> GetAllCategories(ConsoleHelper console, InputHandler input,
        ScoreService scoreService, ScoreRenderer scoreRenderer)
    {
        yield return LexiconViewer.CreateLexiconCategory();
        yield return QuizSession.CreateQuizletCategory(console, input, scoreService);
        yield return LiveCodeSession.CreateCodeTrainerCategory(console, input, scoreService);
        yield return ScoreboardViewer.CreateScoreboardCategory(scoreService, scoreRenderer, console, input);
    }
}
