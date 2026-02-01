using System.Text.RegularExpressions;

namespace Lexicanum.Features.CodeTrainer.Languages
{
    public class PythonLanguage : ProgrammingLanguage
    {
        public override string Name => "Python";

        public override List<CodeExercise> GetExercises()
        {
            return new List<CodeExercise>
            {
                new PythonForLoopExercise(),
                new PythonListComprehensionExercise()
            };
        }
    }

    public class PythonForLoopExercise : CodeExercise
    {
        public PythonForLoopExercise()
        {
            Name = "For Loop";
            Description = "Write a for loop that iterates over a list.";
            CorrectExample = "for item in my_list:\n    # code here";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = userCode.Trim();
            var pattern = @"for\s+\w+\s+in\s+\w+:";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: for item in list:"
            };
        }
    }

    public class PythonListComprehensionExercise : CodeExercise
    {
        public PythonListComprehensionExercise()
        {
            Name = "List Comprehension";
            Description = "Write a list comprehension that creates a list of squares from 0-9.";
            CorrectExample = "squares = [x**2 for x in range(10)]";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"\w+\s*=\s*\[\s*\w+\*\*2\s+for\s+\w+\s+in\s+range\s*\(\s*10\s*\)\s*\]";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: [x**2 for x in range(10)]"
            };
        }
    }
}
