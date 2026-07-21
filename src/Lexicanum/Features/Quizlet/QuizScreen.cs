using Lexicanum.Core.Content;
using Lexicanum.Core.Scoring;
using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.Quizlet;

/// <summary>
/// Runs one quiz from start to results: shuffles questions and answers,
/// scores correct picks, and shows the tiered verdict.
/// </summary>
public sealed class QuizScreen : IScreen
{
    private readonly IReadOnlyList<QuizQuestion> _questions;
    private readonly ScoreService _scoreService;

    public string Title { get; }

    public QuizScreen(string title, IReadOnlyList<QuizQuestion> questions, ScoreService scoreService)
    {
        Title = title;
        _questions = questions;
        _scoreService = scoreService;
    }

    public ScreenResult Run(IAnsiConsole console)
    {
        var shuffledQuestions = ShuffleQuestions();
        var totalQuestions = shuffledQuestions.Count;
        var correctAnswers = 0;

        console.ShowNarrator($"Starting Quiz: {Title}");
        console.ShowInfo($"Total Questions: {totalQuestions}");
        console.WaitForKey("Press any key to begin...");

        for (int questionIndex = 0; questionIndex < totalQuestions; questionIndex++)
        {
            var shuffled = shuffledQuestions[questionIndex];

            console.ShowScreenHeader($"Question {questionIndex + 1} of {totalQuestions}", _scoreService.GetTotalScore());
            console.MarkupLine($"[{Theme.Body}]{Markup.Escape(shuffled.Original.Question)}[/]");
            console.WriteLine();

            var answer = console.PromptMenu("Select your answer:", shuffled.ShuffledOptions);

            if (shuffled.CheckAnswer(answer))
            {
                correctAnswers++;
                _scoreService.AddScore(FeatureIds.Quizlet, ScoringRules.PointsPerCorrectAnswer);
                console.ShowSuccess("Correct!");
            }
            else
            {
                console.ShowError($"Wrong! The correct answer was: {shuffled.GetCorrectAnswer()}");
            }

            console.WriteLine();
            console.ShowInfo($"Explanation: {shuffled.Original.Explanation}");
            console.WaitForKey();
        }

        ShowResults(console, correctAnswers, totalQuestions);
        return ScreenResult.Pop;
    }

    private List<ShuffledQuestion> ShuffleQuestions()
    {
        var questionsCopy = _questions.ToArray();
        Random.Shared.Shuffle(questionsCopy);

        return questionsCopy.Select(q => new ShuffledQuestion(q, Random.Shared)).ToList();
    }

    private static void ShowResults(IAnsiConsole console, int correctAnswers, int totalQuestions)
    {
        console.ShowScreenHeader("Quiz Complete!");

        var percentage = totalQuestions > 0 ? (correctAnswers * 100) / totalQuestions : 0;

        console.ShowInfo($"Your Score: {correctAnswers} / {totalQuestions} ({percentage}%)");
        console.ShowInfo($"Points Earned: {correctAnswers * ScoringRules.PointsPerCorrectAnswer}");
        console.WriteLine();

        if (percentage >= ScoringRules.QuizExcellentPercent)
        {
            console.ShowNarrator("Impressive... I suppose even a broken clock is right twice a day.");
        }
        else if (percentage >= ScoringRules.QuizGoodPercent)
        {
            console.ShowNarrator("Not terrible. You might actually learn something yet.");
        }
        else if (percentage >= ScoringRules.QuizPassablePercent)
        {
            console.ShowNarrator("Mediocre at best. I expected nothing and I'm still disappointed.");
        }
        else
        {
            console.ShowNarrator("Pathetic. Did you even try? Perhaps coding isn't for you.");
        }

        console.WaitForKey();
    }

    public static MenuNode CreateMenuNode(IReadOnlyList<ContentPack<QuizQuestion>> quizPacks, ScoreService scoreService)
    {
        return MenuNode.BranchFromPacks("Quizlet", "Test your knowledge with quizzes", quizPacks,
            pack => new QuizScreen(pack.Category, pack.Items, scoreService));
    }
}
