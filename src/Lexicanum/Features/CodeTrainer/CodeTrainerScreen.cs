using Lexicanum.Core.Content;
using Lexicanum.Core.Scoring;
using Lexicanum.Navigation;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Features.CodeTrainer;

/// <summary>
/// Runs a live-coding session for one language pack: the player types code for each
/// exercise, gets it validated, and optionally plays hardmode (one mistake ends the run).
/// </summary>
public sealed class CodeTrainerScreen : IScreen
{
    private readonly ContentPack<CodeExercise> _pack;
    private readonly ScoreService _scoreService;

    public string Title => $"Live Code Training - {_pack.Category}";

    public CodeTrainerScreen(ContentPack<CodeExercise> pack, ScoreService scoreService)
    {
        _pack = pack;
        _scoreService = scoreService;
    }

    public ScreenResult Run(IAnsiConsole console)
    {
        console.ShowScreenHeader(Title, _scoreService.GetTotalScore());

        var hardmode = console.Confirm("Enable Hardmode? (One mistake and you're out)", defaultValue: false);
        var hardmodeOver = false;
        var sessionScore = 0;

        console.ShowNarrator(hardmode
            ? "Hardmode activated. No pressure... actually, lots of pressure."
            : "Playing it safe, I see. Typical.");
        console.WaitForKey();

        foreach (var exercise in _pack.Items)
        {
            if (hardmodeOver)
            {
                break;
            }

            sessionScore += RunExercise(console, exercise, hardmode, out hardmodeOver);
        }

        ShowSessionResults(console, sessionScore, hardmodeOver);
        return ScreenResult.Pop;
    }

    private int RunExercise(IAnsiConsole console, CodeExercise exercise, bool hardmode, out bool hardmodeOver)
    {
        hardmodeOver = false;

        console.ShowScreenHeader(exercise.Name, _scoreService.GetTotalScore());
        console.MarkupLine($"[{Theme.Body}]{Markup.Escape(exercise.Description)}[/]");
        console.WriteLine();

        console.ShowInfo("Write your code below. Submit with an empty line:");
        console.WriteLine();

        var userCode = console.ReadMultilineInput();

        console.WriteLine();

        if (CodeAnswerValidator.IsCorrect(exercise, userCode))
        {
            _scoreService.AddScore(FeatureIds.CodeTrainer, ScoringRules.PointsPerCorrectAnswer);
            console.ShowSuccess("Correct! ...I suppose even you can get lucky sometimes.");

            if (!string.IsNullOrEmpty(exercise.SuccessFeedback))
            {
                console.ShowInfo(exercise.SuccessFeedback);
            }

            console.WaitForKey();
            return ScoringRules.PointsPerCorrectAnswer;
        }

        if (hardmode)
        {
            console.ShowNarrator("WRONG! Hardmode doesn't forgive. Game Over.");
            hardmodeOver = true;
        }
        else
        {
            console.ShowError("Wrong! Study more, code less... or maybe code more, I don't know.");
        }

        console.ShowInfo(exercise.Feedback);
        ShowCorrectAnswer(console, exercise, userCode);

        if (!hardmodeOver)
        {
            console.WaitForKey();
        }

        return 0;
    }

    private static void ShowCorrectAnswer(IAnsiConsole console, CodeExercise exercise, string userCode)
    {
        console.WriteLine();
        console.ShowBox("Correct Example", exercise.CorrectExample);

        if (string.IsNullOrWhiteSpace(userCode))
        {
            return;
        }

        console.WriteLine();

        var comparison = new Table()
            .Border(TableBorder.Rounded)
            .BorderStyle(Theme.MutedStyle)
            .AddColumn("Your Answer")
            .AddColumn("Correct Answer");

        comparison.AddRow(
            new Text(userCode.Trim(), Theme.WrongAnswerStyle),
            new Text(exercise.CorrectExample, Theme.CorrectAnswerStyle));

        console.Write(comparison);
    }

    private void ShowSessionResults(IAnsiConsole console, int sessionScore, bool hardmodeOver)
    {
        console.ShowScreenHeader("Session Complete", _scoreService.GetTotalScore());

        console.ShowInfo($"Session Score: {sessionScore}");
        console.WriteLine();

        if (hardmodeOver)
        {
            console.ShowNarrator("Failed in hardmode. The shame will follow you forever.");
        }
        else if (sessionScore > ScoringRules.CodeExcellentScore)
        {
            console.ShowNarrator("Impressive score. Don't let it go to your head.");
        }
        else if (sessionScore > ScoringRules.CodeGoodScore)
        {
            console.ShowNarrator("Acceptable. Room for improvement... lots of room.");
        }
        else
        {
            console.ShowNarrator("I've seen better. Much, much better.");
        }

        console.WaitForKey();
    }

    public static MenuNode CreateMenuNode(IReadOnlyList<ContentPack<CodeExercise>> exercisePacks, ScoreService scoreService)
    {
        var languageLeaves = exercisePacks
            .Select(pack => MenuNode.Leaf(pack.Category, pack.Description,
                () => new CodeTrainerScreen(pack, scoreService)))
            .ToArray();

        return MenuNode.Branch("Live Code Training", "Practice writing code in real-time", languageLeaves);
    }
}
