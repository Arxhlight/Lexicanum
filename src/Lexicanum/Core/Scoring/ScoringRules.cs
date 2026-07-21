namespace Lexicanum.Core.Scoring;

/// <summary>
/// Every scoring constant in one place: points per answer and the thresholds
/// behind the narrator's verdicts.
/// </summary>
public static class ScoringRules
{
    public const int PointsPerCorrectAnswer = 100;

    public const int QuizExcellentPercent = 90;
    public const int QuizGoodPercent = 70;
    public const int QuizPassablePercent = 50;

    public const int CodeExcellentScore = 300;
    public const int CodeGoodScore = 100;
}
