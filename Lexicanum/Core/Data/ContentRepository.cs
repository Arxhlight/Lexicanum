using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Services;
using Lexicanum.Features.CodeTrainer;
using Lexicanum.Features.Lexicon;
using Lexicanum.Features.Quizlet;
using Lexicanum.Features.Scoreboard;
using Lexicanum.UI;

namespace Lexicanum.Core.Data
{
    /// <summary>
    /// Central repository for all content categories.
    /// Add new categories here to extend the application.
    /// </summary>
    public static class ContentRepository
    {
        /// <summary>
        /// Gets all available categories for the application.
        /// To add a new category, simply add it to the list returned by this method.
        /// </summary>
        public static IEnumerable<ICategory> GetAllCategories(ConsoleHelper console, InputHandler input, 
            ScoreService scoreService, ScoreRenderer scoreRenderer)
        {
            // Return all categories - add new ones here
            yield return LexiconViewer.CreateLexiconCategory();
            yield return QuizSession.CreateQuizletCategory(console, input, scoreService);
            yield return LiveCodeSession.CreateCodeTrainerCategory(console, input, scoreService);
            yield return ScoreboardViewer.CreateScoreboardCategory(scoreService, scoreRenderer, console, input);
            
            // To add a new category:
            // yield return YourNewFeature.CreateCategory(console, input, scoreService, scoreRenderer);
        }
    }
}
