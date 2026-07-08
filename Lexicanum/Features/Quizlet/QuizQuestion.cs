namespace Lexicanum.Features.Quizlet;

public class QuizQuestion
{
    public string Question { get; }
    public string[] Options { get; }
    public int CorrectAnswerIndex { get; }
    public string Explanation { get; }

    public QuizQuestion(string question, string[] options, int correctAnswerIndex, string explanation)
    {
        Question = question;
        Options = options;
        CorrectAnswerIndex = correctAnswerIndex;
        Explanation = explanation;
    }
}
