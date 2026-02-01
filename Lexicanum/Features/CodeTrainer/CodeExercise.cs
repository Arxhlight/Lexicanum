using System.Text.RegularExpressions;

namespace Lexicanum.Features.CodeTrainer
{
    public class ValidationResult
    {
        public bool IsCorrect { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }

    public abstract class CodeExercise
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CorrectExample { get; set; } = string.Empty;

        public abstract ValidationResult Validate(string userCode);
    }

    public abstract class ProgrammingLanguage
    {
        public abstract string Name { get; }
        public abstract List<CodeExercise> GetExercises();
    }
}
