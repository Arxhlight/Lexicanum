using System.Text.RegularExpressions;

namespace Lexicanum.Features.CodeTrainer.Languages
{
    public class CPlusPlusLanguage : ProgrammingLanguage
    {
        public override string Name => "C++";

        public override List<CodeExercise> GetExercises()
        {
            return new List<CodeExercise>
            {
                new CPlusPlusForLoopExercise(),
                new CPlusPlusRangeBasedForLoopExercise(),
                new CPlusPlusIfStatementExercise(),
                new CPlusPlusPointerDeclarationExercise()
            };
        }
    }

    public class CPlusPlusForLoopExercise : CodeExercise
    {
        public CPlusPlusForLoopExercise()
        {
            Name = "For Loop";
            Description = "Write a for loop that counts from 0 to 9.";
            CorrectExample = "for (int i = 0; i < 10; i++)\n{\n    // code here\n}";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"for\s*\(\s*int\s+\w+\s*=\s*0\s*;\s*\w+\s*<\s*10\s*;\s*\w+\+\+\s*\)";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Remember: initialization (int i = 0), condition (i < 10), and increment (i++)."
            };
        }
    }

    public class CPlusPlusRangeBasedForLoopExercise : CodeExercise
    {
        public CPlusPlusRangeBasedForLoopExercise()
        {
            Name = "Range-Based For Loop";
            Description = "Write a range-based for loop that iterates over a vector.";
            CorrectExample = "for (auto item : myVector)\n{\n    // code here\n}";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"for\s*\(\s*(auto|int|const\s+auto)\s+\w+\s*:\s*\w+\s*\)";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: for (auto item : container)"
            };
        }
    }

    public class CPlusPlusIfStatementExercise : CodeExercise
    {
        public CPlusPlusIfStatementExercise()
        {
            Name = "If Statement";
            Description = "Write an if statement that checks if a variable 'x' is greater than 5.";
            CorrectExample = "if (x > 5)\n{\n    // code here\n}";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"if\s*\(\s*\w+\s*>\s*5\s*\)";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: if (variable > 5)"
            };
        }
    }

    public class CPlusPlusPointerDeclarationExercise : CodeExercise
    {
        public CPlusPlusPointerDeclarationExercise()
        {
            Name = "Pointer Declaration";
            Description = "Declare an int pointer that points to a variable 'x'.";
            CorrectExample = "int* ptr = &x;";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"int\s*\*\s*\w+\s*=\s*&\s*\w+";

            if (Regex.IsMatch(normalized, pattern))
            {
                return new ValidationResult { IsCorrect = true };
            }

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "Syntax: int* ptr = &variable;"
            };
        }
    }
}
