using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.UI;

namespace Lexicanum.Features.Quizlet
{
    /// <summary>
    /// Represents a question with shuffled answer options.
    /// Tracks the new position of the correct answer after shuffling.
    /// </summary>
    internal class ShuffledQuestion
    {
        public QuizQuestion Original { get; }
        public string[] ShuffledOptions { get; }
        public int ShuffledCorrectIndex { get; }

        public ShuffledQuestion(QuizQuestion original, Random random)
        {
            Original = original;
            
            // Keep shuffling until we get a different order (if more than 1 option)
            List<(string Option, int OriginalIndex)> optionsWithIndices;
            
            do
            {
                // Create a list of (option, originalIndex) pairs
                optionsWithIndices = original.Options
                    .Select((opt, idx) => (Option: opt, OriginalIndex: idx))
                    .ToList();
                
                // Shuffle using Fisher-Yates
                for (int i = optionsWithIndices.Count - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    (optionsWithIndices[i], optionsWithIndices[j]) = (optionsWithIndices[j], optionsWithIndices[i]);
                }
            } 
            // Re-shuffle if we got the same order as original (only if more than 1 option)
            while (original.Options.Length > 1 && 
                   optionsWithIndices.Select((x, i) => x.OriginalIndex == i).All(same => same));
            
            ShuffledOptions = optionsWithIndices.Select(x => x.Option).ToArray();
            ShuffledCorrectIndex = optionsWithIndices.FindIndex(x => x.OriginalIndex == original.CorrectAnswerIndex);
        }

        public bool CheckAnswer(int answerIndex) => answerIndex == ShuffledCorrectIndex;
        
        public string GetCorrectAnswer() => ShuffledOptions[ShuffledCorrectIndex];
    }

    public class QuizSession
    {
        private const string FeatureName = "Quizlet";
        private const int PointsPerCorrectAnswer = 100;
        
        private readonly ConsoleHelper _console;
        private readonly InputHandler _input;
        private readonly ScoreService _scoreService;
        private readonly List<QuizQuestion> _questions;
        private readonly Random _random;
        private int _sessionScore;
        private int _currentQuestionIndex;
        private int _totalQuestions;

        public string Name { get; }

        public QuizSession(string name, List<QuizQuestion> questions, ConsoleHelper console, InputHandler input, ScoreService scoreService)
        {
            Name = name;
            _questions = questions;
            _console = console;
            _input = input;
            _scoreService = scoreService;
            _random = new Random();
            _sessionScore = 0;
            _currentQuestionIndex = 0;
        }

        public void Start()
        {
            _sessionScore = 0;
            _currentQuestionIndex = 0;

            // Shuffle questions order
            var shuffledQuestions = ShuffleQuestions();
            _totalQuestions = shuffledQuestions.Count;

            _console.ShowNarrator($"Starting Quiz: {Name}");
            _console.ShowInfo($"Total Questions: {_totalQuestions}");
            _console.WaitForInput("Press Enter to begin...");

            foreach (var shuffled in shuffledQuestions)
            {
                _console.ClearScreen();
                DisplayQuestion(shuffled);
                
                var answer = _input.GetMenuChoice(shuffled.ShuffledOptions.Length, 1) - 1;
                
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

                Console.WriteLine();
                _console.ShowInfo($"Explanation: {shuffled.Original.Explanation}");
                _console.WaitForInput();
                
                _currentQuestionIndex++;
            }

            ShowResults();
        }

        /// <summary>
        /// Shuffles the question order and creates ShuffledQuestion instances
        /// with randomized answer options.
        /// </summary>
        private List<ShuffledQuestion> ShuffleQuestions()
        {
            var questionsCopy = _questions.ToList();
            
            // Fisher-Yates shuffle for questions
            for (int i = questionsCopy.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (questionsCopy[i], questionsCopy[j]) = (questionsCopy[j], questionsCopy[i]);
            }
            
            // Create shuffled questions with randomized answer options
            return questionsCopy.Select(q => new ShuffledQuestion(q, _random)).ToList();
        }

        private void DisplayQuestion(ShuffledQuestion shuffled)
        {
            _console.ShowHeader($"Question {_currentQuestionIndex + 1} of {_totalQuestions}");
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(shuffled.Original.Question);
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < shuffled.ShuffledOptions.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  [{i + 1}] {shuffled.ShuffledOptions[i]}");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private void ShowResults()
        {
            _console.ClearScreen();
            _console.ShowHeader("Quiz Complete!");
            
            var percentage = (_sessionScore * 100) / _totalQuestions;
            
            Console.WriteLine();
            _console.ShowInfo($"Your Score: {_sessionScore} / {_totalQuestions} ({percentage}%)");
            _console.ShowInfo($"Points Earned: {_sessionScore * PointsPerCorrectAnswer}");
            Console.WriteLine();

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

            _console.WaitForInput();
        }

        public static Category CreateQuizletCategory(ConsoleHelper console, InputHandler input, ScoreService scoreService)
        {
            var category = new Category("Quizlet", "Test your knowledge with quizzes");

            // Git Quiz
            var gitQuiz = CreateGitQuizSubCategory(console, input, scoreService);
            category.AddSubCategory(gitQuiz);

            // Programming Basics Quiz
            var programmingQuiz = CreateProgrammingQuizSubCategory(console, input, scoreService);
            category.AddSubCategory(programmingQuiz);

            return category;
        }

        private static SubCategory CreateGitQuizSubCategory(ConsoleHelper console, InputHandler input, ScoreService scoreService)
        {
            var questions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "Git Basics",
                    "What command is used to create a new Git repository?",
                    new[] { "git new", "git init", "git create", "git start" },
                    1,
                    "git init initializes a new Git repository in the current directory."
                ),
                new QuizQuestion(
                    "Git Staging",
                    "Which command stages all changes for commit?",
                    new[] { "git commit -a", "git add .", "git stage all", "git push" },
                    1,
                    "git add . stages all changes in the current directory and subdirectories."
                ),
                new QuizQuestion(
                    "Git Branches",
                    "How do you create and switch to a new branch in one command?",
                    new[] { "git branch new-branch", "git switch new-branch", "git checkout -b new-branch", "git new-branch" },
                    2,
                    "git checkout -b creates a new branch and switches to it. Modern alternative: git switch -c"
                )
            };

            var subCategory = new SubCategory("Git Quiz", "Test your Git knowledge", _ =>
            {
                var session = new QuizSession("Git Fundamentals", questions, console, input, scoreService);
                session.Start();
            });

            return subCategory;
        }
        

        private static SubCategory CreateProgrammingQuizSubCategory(ConsoleHelper console, InputHandler input, ScoreService scoreService)
        {
            var questions = new List<QuizQuestion>
            {
                new QuizQuestion(
                    "OOP Basics",
                    "What does OOP stand for?",
                    new[] { "Object-Oriented Programming", "Open-Oriented Protocol", "Objective Operation Process", "Optional Object Pattern" },
                    0,
                    "OOP stands for Object-Oriented Programming, a programming paradigm based on objects."
                ),
                new QuizQuestion(
                    "Data Structures",
                    "What is the time complexity of accessing an element in an array by index?",
                    new[] { "O(n)", "O(log n)", "O(1)", "O(n²)" },
                    2,
                    "Array access by index is O(1) - constant time, as it's a direct memory offset calculation."
                )
            };

            var subCategory = new SubCategory("Programming Basics Quiz", "Test your programming fundamentals", _ =>
            {
                var session = new QuizSession("Programming Fundamentals", questions, console, input, scoreService);
                session.Start();
            });

            return subCategory;
        }
    }
}
