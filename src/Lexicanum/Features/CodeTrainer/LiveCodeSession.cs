using Lexicanum.Core.Models;
using Lexicanum.Core.Services;
using Lexicanum.Features.CodeTrainer.Languages;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.CodeTrainer;

public class LiveCodeSession
{
    private const string FeatureName = "CodeTraining";
    private const int PointsPerCorrectAnswer = 100;

    private readonly IAnsiConsole _console;
    private readonly ScoreService _scoreService;

    private bool _hardmode;
    private bool _hardmodeOver;
    private int _sessionScore;

    public LiveCodeSession(IAnsiConsole console, ScoreService scoreService)
    {
        _console = console;
        _scoreService = scoreService;
    }

    public void Start(ProgrammingLanguage language)
    {
        _sessionScore = 0;
        _hardmodeOver = false;

        _console.ShowScreenHeader($"Live Code Training - {language.Name}", _scoreService.GetTotalScore());

        _hardmode = _console.Confirm("Enable Hardmode? (One mistake and you're out)", defaultValue: false);

        if (_hardmode)
        {
            _console.ShowNarrator("Hardmode activated. No pressure... actually, lots of pressure.");
        }
        else
        {
            _console.ShowNarrator("Playing it safe, I see. Typical.");
        }

        _console.WaitForKey();

        foreach (var exercise in language.GetExercises())
        {
            if (_hardmodeOver)
            {
                break;
            }

            RunExercise(exercise);
        }

        ShowSessionResults();
    }

    private void RunExercise(CodeExercise exercise)
    {
        _console.ShowScreenHeader(exercise.Name, _scoreService.GetTotalScore());
        _console.MarkupLine($"[{Theme.Body}]{Markup.Escape(exercise.Description)}[/]");
        _console.WriteLine();

        _console.ShowInfo("Write your code below. Submit with an empty line:");
        _console.WriteLine();

        var userCode = _console.ReadMultilineInput();
        var result = exercise.Validate(userCode);

        _console.WriteLine();

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
            _console.WaitForKey();
        }
    }

    private void ShowCorrectAnswer(CodeExercise exercise, string userCode)
    {
        _console.WriteLine();
        _console.ShowBox("Correct Example", exercise.CorrectExample);

        if (!string.IsNullOrWhiteSpace(userCode))
        {
            _console.WriteLine();
            ShowComparison(userCode.Trim(), exercise.CorrectExample);
        }
    }

    private void ShowComparison(string userCode, string correctCode)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderStyle(Theme.MutedStyle)
            .AddColumn("Your Answer")
            .AddColumn("Correct Answer");

        table.AddRow(
            new Text(userCode, Theme.WrongAnswerStyle),
            new Text(correctCode, Theme.CorrectAnswerStyle));

        _console.Write(table);
    }

    private void ShowSessionResults()
    {
        _console.ShowScreenHeader("Session Complete", _scoreService.GetTotalScore());

        _console.ShowInfo($"Session Score: {_sessionScore}");
        _console.WriteLine();

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

        _console.WaitForKey();
    }

    public static Category CreateCodeTrainerCategory(IAnsiConsole console, ScoreService scoreService)
    {
        var session = new LiveCodeSession(console, scoreService);
        var category = new Category("Live Code Training", "Practice writing code in real-time");

        category.AddSubCategory(new SubCategory("C#", "Practice C# syntax and patterns", _ =>
        {
            session.Start(new CSharpLanguage());
        }));

        category.AddSubCategory(new SubCategory("C++", "Practice C++ syntax and patterns", _ =>
        {
            session.Start(new CPlusPlusLanguage());
        }));

        category.AddSubCategory(new SubCategory("JavaScript", "Practice JavaScript syntax and patterns", _ =>
        {
            session.Start(new JavaScriptLanguage());
        }));

        category.AddSubCategory(new SubCategory("Python", "Practice Python syntax and patterns", _ =>
        {
            session.Start(new PythonLanguage());
        }));

        return category;
    }
}
