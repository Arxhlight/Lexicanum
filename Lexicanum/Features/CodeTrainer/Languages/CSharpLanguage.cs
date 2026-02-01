using System.Text.RegularExpressions;

namespace Lexicanum.Features.CodeTrainer.Languages
{
    public class CSharpLanguage : ProgrammingLanguage
    {
        public override string Name => "C#";

        public override List<CodeExercise> GetExercises()
        {
            return new List<CodeExercise>
            {
                new CSharpForEachExercise(),
                new CSharpForLoopExercise(),
                new CSharpIfStatementExercise()
            };
        }
    }

    public class CSharpForEachExercise : CodeExercise
    {
        public CSharpForEachExercise()
        {
            Name = "Foreach Loop";
            Description = "Write a foreach loop that iterates over a list with any name.";
            CorrectExample = "foreach (var item in myList)\n{\n    // code here\n}";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"foreach\s*\(\s*var\s+\w+\s+in\s+\w+\s*\)";

            if (Regex.IsMatch(normalized, pattern))
            {
                if (userCode.Contains("{") && userCode.Contains("}"))
                {
                    return new ValidationResult
                    {
                        IsCorrect = true,
                        Feedback = "Well done! You included the curly braces too."
                    };
                }

                return new ValidationResult
                {
                    IsCorrect = true,
                    Feedback = "Correct! Tip: Don't forget curly braces for best practice."
                };
            }

            if (!normalized.Contains("foreach"))
                return new ValidationResult { IsCorrect = false, Feedback = "Don't forget the 'foreach' keyword." };

            if (!normalized.Contains("var"))
                return new ValidationResult { IsCorrect = false, Feedback = "Use 'var' to declare the variable." };

            if (!normalized.Contains("in"))
                return new ValidationResult { IsCorrect = false, Feedback = "Don't forget 'in' between the variable and collection." };

            return new ValidationResult
            {
                IsCorrect = false,
                Feedback = "The syntax isn't quite right. Check parentheses and structure."
            };
        }
    }

    public class CSharpForLoopExercise : CodeExercise
    {
        public CSharpForLoopExercise()
        {
            Name = "For Loop";
            Description = "Write a for loop that counts from 0 to 9.";
            CorrectExample = "for (int i = 0; i < 10; i++)\n{\n    // code here\n}";
        }

        public override ValidationResult Validate(string userCode)
        {
            var normalized = Regex.Replace(userCode, @"\s+", " ").Trim();
            var pattern = @"for\s*\(\s*(int|var)\s+\w+\s*=\s*0\s*;\s*\w+\s*<\s*10\s*;\s*\w+\+\+\s*\)";

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

    public class CSharpIfStatementExercise : CodeExercise
    {
        public CSharpIfStatementExercise()
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
}
