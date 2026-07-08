using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.UI;
using Spectre.Console;

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
        ShuffledCorrectIndex = optionsWithIndices.FindIndex(x => x.OriginalIndex == original.CorrectAnswerIndex);
    }

    public bool CheckAnswer(int answerIndex) => answerIndex == ShuffledCorrectIndex;

    public string GetCorrectAnswer() => ShuffledOptions[ShuffledCorrectIndex];

    /// <summary>
    /// Fisher-Yates shuffle, repeated until the result differs from the original order
    /// so a shuffled question never presents its options unshuffled.
    /// </summary>
    private static List<(string Option, int OriginalIndex)> ShuffleUntilOrderChanges(string[] options, Random random)
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
        while (options.Length > 1 &&
               optionsWithIndices.Select((x, i) => x.OriginalIndex == i).All(same => same));

        return optionsWithIndices;
    }
}

public class QuizSession
{
    private const string FeatureName = "Quizlet";
    private const int PointsPerCorrectAnswer = 100;

    private readonly IAnsiConsole _console;
    private readonly ScoreService _scoreService;
    private readonly List<QuizQuestion> _questions;
    private readonly Random _random;
    private int _sessionScore;

    public string Name { get; }

    public QuizSession(string name, List<QuizQuestion> questions, IAnsiConsole console, ScoreService scoreService)
    {
        Name = name;
        _questions = questions;
        _console = console;
        _scoreService = scoreService;
        _random = new Random();
    }

    public void Start()
    {
        _sessionScore = 0;

        var shuffledQuestions = ShuffleQuestions();
        var totalQuestions = shuffledQuestions.Count;

        _console.ShowNarrator($"Starting Quiz: {Name}");
        _console.ShowInfo($"Total Questions: {totalQuestions}");
        _console.WaitForKey("Press any key to begin...");

        for (int questionIndex = 0; questionIndex < totalQuestions; questionIndex++)
        {
            var shuffled = shuffledQuestions[questionIndex];

            _console.ShowScreenHeader($"Question {questionIndex + 1} of {totalQuestions}", _scoreService.GetTotalScore());
            _console.MarkupLine($"[{Theme.Body}]{Markup.Escape(shuffled.Original.Question)}[/]");
            _console.WriteLine();

            var answer = _console.PromptMenu("Select your answer:", shuffled.ShuffledOptions);

            if (shuffled.CheckAnswer(answer))
            {
                _sessionScore++;
                _scoreService.AddScore(FeatureName, PointsPerCorrectAnswer);
                _console.ShowSuccess("Correct!");
            }
            else
            {
                _console.ShowError($"Wrong! The correct answer was: {shuffled.GetCorrectAnswer()}");
            }

            _console.WriteLine();
            _console.ShowInfo($"Explanation: {shuffled.Original.Explanation}");
            _console.WaitForKey();
        }

        ShowResults(totalQuestions);
    }

    private List<ShuffledQuestion> ShuffleQuestions()
    {
        var questionsCopy = _questions.ToList();

        for (int i = questionsCopy.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (questionsCopy[i], questionsCopy[j]) = (questionsCopy[j], questionsCopy[i]);
        }

        return questionsCopy.Select(q => new ShuffledQuestion(q, _random)).ToList();
    }

    private void ShowResults(int totalQuestions)
    {
        _console.ShowScreenHeader("Quiz Complete!");

        var percentage = totalQuestions > 0 ? (_sessionScore * 100) / totalQuestions : 0;

        _console.ShowInfo($"Your Score: {_sessionScore} / {totalQuestions} ({percentage}%)");
        _console.ShowInfo($"Points Earned: {_sessionScore * PointsPerCorrectAnswer}");
        _console.WriteLine();

        if (percentage >= 90)
        {
            _console.ShowNarrator("Impressive... I suppose even a broken clock is right twice a day.");
        }
        else if (percentage >= 70)
        {
            _console.ShowNarrator("Not terrible. You might actually learn something yet.");
        }
        else if (percentage >= 50)
        {
            _console.ShowNarrator("Mediocre at best. I expected nothing and I'm still disappointed.");
        }
        else
        {
            _console.ShowNarrator("Pathetic. Did you even try? Perhaps coding isn't for you.");
        }

        _console.WaitForKey();
    }

    public static Category CreateQuizletCategory(IAnsiConsole console, ScoreService scoreService)
    {
        var category = new Category("Quizlet", "Test your knowledge with quizzes");

        category.AddSubCategory(CreateGitQuizSubCategory(console, scoreService));
        category.AddSubCategory(CreateProgrammingQuizSubCategory(console, scoreService));

        return category;
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

    private static SubCategory CreateGitQuizSubCategory(IAnsiConsole console, ScoreService scoreService)
    {
        return new SubCategory("Git Quiz", "Test your Git knowledge", _ =>
        {
            var session = new QuizSession("Git Fundamentals", GitQuizQuestions, console, scoreService);
            session.Start();
        });
    }

    private static SubCategory CreateProgrammingQuizSubCategory(IAnsiConsole console, ScoreService scoreService)
    {
        return new SubCategory("Programming Basics Quiz", "Test your programming fundamentals", _ =>
        {
            var session = new QuizSession("Programming Fundamentals", ProgrammingQuizQuestions, console, scoreService);
            session.Start();
        });
    }
}
