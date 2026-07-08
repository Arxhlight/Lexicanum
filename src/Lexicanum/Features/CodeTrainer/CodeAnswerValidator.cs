using System.Text.RegularExpressions;
using Lexicanum.Core.Content;

namespace Lexicanum.Features.CodeTrainer;

/// <summary>
/// Validates player code against an exercise's answer pattern.
/// Whitespace is collapsed first so formatting never affects correctness.
/// </summary>
public static class CodeAnswerValidator
{
    public static bool IsCorrect(CodeExercise exercise, string userCode)
    {
        var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
        return Regex.IsMatch(normalized, exercise.AnswerPattern);
    }
}
