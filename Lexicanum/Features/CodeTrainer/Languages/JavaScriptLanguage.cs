using System.Text.RegularExpressions;

namespace Lexicanum.Features.CodeTrainer.Languages
{
    public class JavaScriptLanguage : ProgrammingLanguage
    {
        public override string Name => "JavaScript";

        public override List<CodeExercise> GetExercises()
        {
            return new List<CodeExercise>
            {
                new JavaScriptForEachExercise(),
                new JavaScriptArrowFunctionExercise()
            };
        }
    }

    public class JavaScriptForEachExercise : CodeExercise
    {
        public JavaScriptForEachExercise()
        {
            Name = "forEach Method";
            Description = "Write a forEach that iterates over an array.";
            CorrectExample = "myArray.forEach(item => {\n    // code here\n});";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"\w+\.forEach\s*\(\s*\w+\s*=>";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: array.forEach(item => { ... });"
            };
        }
    }

    public class JavaScriptArrowFunctionExercise : CodeExercise
    {
        public JavaScriptArrowFunctionExercise()
        {
            Name = "Arrow Function";
            Description = "Write an arrow function that takes a parameter and returns it * 2.";
            CorrectExample = "const double = x => x * 2;";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"(const|let|var)\s+\w+\s*=\s*\w+\s*=>\s*\w+\s*\*\s*2";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: const name = x => x * 2;"
            };
        }
    }
}
