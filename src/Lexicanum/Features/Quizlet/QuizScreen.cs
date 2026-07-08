using Lexicanum.Core.Services;
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
    private const string FeatureName = "Quizlet";
    private const int PointsPerCorrectAnswer = 100;

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
                _scoreService.AddScore(FeatureName, PointsPerCorrectAnswer);
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
        var questionsCopy = _questions.ToList();

        for (int i = questionsCopy.Count - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);
            (questionsCopy[i], questionsCopy[j]) = (questionsCopy[j], questionsCopy[i]);
        }

        return questionsCopy.Select(q => new ShuffledQuestion(q, Random.Shared)).ToList();
    }

    private static void ShowResults(IAnsiConsole console, int correctAnswers, int totalQuestions)
    {
        console.ShowScreenHeader("Quiz Complete!");

        var percentage = totalQuestions > 0 ? (correctAnswers * 100) / totalQuestions : 0;

        console.ShowInfo($"Your Score: {correctAnswers} / {totalQuestions} ({percentage}%)");
        console.ShowInfo($"Points Earned: {correctAnswers * PointsPerCorrectAnswer}");
        console.WriteLine();

        if (percentage >= 90)
        {
            console.ShowNarrator("Impressive... I suppose even a broken clock is right twice a day.");
        }
        else if (percentage >= 70)
        {
            console.ShowNarrator("Not terrible. You might actually learn something yet.");
        }
        else if (percentage >= 50)
        {
            console.ShowNarrator("Mediocre at best. I expected nothing and I'm still disappointed.");
        }
        else
        {
            console.ShowNarrator("Pathetic. Did you even try? Perhaps coding isn't for you.");
        }

        console.WaitForKey();
    }

    private static readonly List<QuizQuestion> GitQuizQuestions = new()
    {
        new QuizQuestion(
            "What command is used to create a new Git repository?",
            new[] { "git new", "git init", "git create", "git start" },
            1,
            "git init initializes a new Git repository in the current directory."
        ),
        new QuizQuestion(
            "Which command stages all changes for commit?",
            new[] { "git commit -a", "git add .", "git stage all", "git push" },
            1,
            "git add . stages all changes in the current directory and subdirectories."
        ),
        new QuizQuestion(
            "How do you create and switch to a new branch in one command?",
            new[] { "git branch new-branch", "git switch new-branch", "git checkout -b new-branch", "git new-branch" },
            2,
            "git checkout -b creates a new branch and switches to it. Modern alternative: git switch -c"
        )
    };

    private static readonly List<QuizQuestion> ProgrammingQuizQuestions = new()
    {
        new QuizQuestion(
            "What does OOP stand for?",
            new[] { "Object-Oriented Programming", "Open-Oriented Protocol", "Objective Operation Process", "Optional Object Pattern" },
            0,
            "OOP stands for Object-Oriented Programming, a programming paradigm based on objects."
        ),
        new QuizQuestion(
            "What is the time complexity of accessing an element in an array by index?",
            new[] { "O(n)", "O(log n)", "O(1)", "O(n²)" },
            2,
            "Array access by index is O(1) - constant time, as it's a direct memory offset calculation."
        )
    };

    public static MenuNode CreateMenuNode(ScoreService scoreService)
    {
        return MenuNode.Branch("Quizlet", "Test your knowledge with quizzes",
            MenuNode.Leaf("Git Quiz", "Test your Git knowledge",
                () => new QuizScreen("Git Fundamentals", GitQuizQuestions, scoreService)),
            MenuNode.Leaf("Programming Basics Quiz", "Test your programming fundamentals",
                () => new QuizScreen("Programming Fundamentals", ProgrammingQuizQuestions, scoreService)));
    }
}
