using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.Features.CodeTrainer.Languages;
using Lexicanum.UI;

namespace Lexicanum.Features.CodeTrainer
{
    public class LiveCodeSession
    {
        private const string FeatureName = "CodeTraining";
        private const int PointsPerCorrectAnswer = 100;
        
        private readonly ConsoleHelper _console;
        private readonly InputHandler _input;
        private readonly ScoreService _scoreService;
        
        private bool _hardmode;
        private bool _hardmodeOver;
        private int _sessionScore;

        public LiveCodeSession(ConsoleHelper console, InputHandler input, ScoreService scoreService)
        {
            _console = console;
            _input = input;
            _scoreService = scoreService;
        }

        public void Start(ProgrammingLanguage language)
        {
            _sessionScore = 0;
            _hardmodeOver = false;

            // Ask for hardmode
            _console.ClearScreen();
            _console.ShowHeader($"Live Code Training - {language.Name}");
            
            _hardmode = _input.GetYesNoInput("Enable Hardmode? (One mistake and you're out)");
            
            if (_hardmode)
            {
                _console.ShowNarrator("Hardmode activated. No pressure... actually, lots of pressure.");
            }
            else
            {
                _console.ShowNarrator("Playing it safe, I see. Typical.");
            }
            
            _console.WaitForInput();

            var exercises = language.GetExercises();

            foreach (var exercise in exercises)
            {
                if (_hardmodeOver) break;
                RunExercise(exercise);
            }

            ShowSessionResults();
        }

        private void RunExercise(CodeExercise exercise)
        {
            _console.ClearScreen();
            _console.ShowHeader(exercise.Name);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(exercise.Description);
            Console.ResetColor();
            Console.WriteLine();

            _console.ShowInfo("Write your code below. Submit with an empty line (press Enter twice):");
            Console.WriteLine();
            
            var userCode = _input.ReadMultilineInput();
            var result = exercise.Validate(userCode);

            Console.WriteLine();

            if (result.IsCorrect)
            {
                _sessionScore += PointsPerCorrectAnswer;
                _scoreService.AddScore(FeatureName, PointsPerCorrectAnswer);
                _console.ShowSuccess("Correct! ...I suppose even you can get lucky sometimes.");
                
                if (!string.IsNullOrEmpty(result.Feedback))
                {
                    _console.ShowInfo(result.Feedback);
                }
            }
            else
            {
                if (_hardmode)
                {
                    _console.ShowNarrator("WRONG! Hardmode doesn't forgive. Game Over.");
                    _hardmodeOver = true;
                }
                else
                {
                    _console.ShowError("Wrong! Study more, code less... or maybe code more, I don't know.");
                }

                if (!string.IsNullOrEmpty(result.Feedback))
                {
                    _console.ShowInfo(result.Feedback);
                }

                ShowCorrectAnswer(exercise, userCode);
            }

            if (!_hardmodeOver)
            {
                _console.WaitForInput();
            }
        }

        private void ShowCorrectAnswer(CodeExercise exercise, string userCode)
        {
            Console.WriteLine();
            _console.ShowBox("Correct Example", exercise.CorrectExample);

            if (!string.IsNullOrWhiteSpace(userCode))
            {
                Console.WriteLine();
                ShowComparison("Your Answer", userCode.Trim(), "Correct Answer", exercise.CorrectExample);
            }
        }

        private void ShowComparison(string leftTitle, string leftCode, string rightTitle, string rightCode)
        {
            var leftLines = leftCode.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rightLines = rightCode.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var maxLines = Math.Max(leftLines.Length, rightLines.Length);
            var columnWidth = 40;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"═ " + leftTitle + " ".PadRight(columnWidth)}{"═ " + rightTitle}");
            Console.ResetColor();

            for (int i = 0; i < maxLines; i++)
            {
                var leftLine = i < leftLines.Length ? leftLines[i] : "";
                var rightLine = i < rightLines.Length ? rightLines[i] : "";

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(leftLine.PadRight(columnWidth));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(rightLine);
                Console.ResetColor();
            }
        }

        private void ShowSessionResults()
        {
            _console.ClearScreen();
            _console.ShowHeader("Session Complete");
            
            Console.WriteLine();
            _console.ShowInfo($"Session Score: {_sessionScore}");
            Console.WriteLine();

            if (_hardmodeOver)
            {
                _console.ShowNarrator("Failed in hardmode. The shame will follow you forever.");
            }
            else if (_sessionScore > 300)
            {
                _console.ShowNarrator("Impressive score. Don't let it go to your head.");
            }
            else if (_sessionScore > 100)
            {
                _console.ShowNarrator("Acceptable. Room for improvement... lots of room.");
            }
            else
            {
                _console.ShowNarrator("I've seen better. Much, much better.");
            }

            _console.WaitForInput();
        }

        public static Category CreateCodeTrainerCategory(ConsoleHelper console, InputHandler input, ScoreService scoreService)
        {
            var session = new LiveCodeSession(console, input, scoreService);
            var category = new Category("Live Code Training", "Practice writing code in real-time");

            // C# SubCategory
            var csharpSub = new SubCategory("C#", "Practice C# syntax and patterns", _ =>
            {
                session.Start(new CSharpLanguage());
            });
            category.AddSubCategory(csharpSub);

            // C++ SubCategory
            var cppSub = new SubCategory("C++", "Practice C++ syntax and patterns", _ =>
            {
                session.Start(new CPlusPlusLanguage());
            });
            category.AddSubCategory(cppSub);

            // JavaScript SubCategory
            var jsSub = new SubCategory("JavaScript", "Practice JavaScript syntax and patterns", _ =>
            {
                session.Start(new JavaScriptLanguage());
            });
            category.AddSubCategory(jsSub);

            // Python SubCategory
            var pythonSub = new SubCategory("Python", "Practice Python syntax and patterns", _ =>
            {
                session.Start(new PythonLanguage());
            });
            category.AddSubCategory(pythonSub);

            return category;
        }
    }
}
