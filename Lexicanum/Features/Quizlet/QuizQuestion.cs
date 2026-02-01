using Lexicanum.Core.Interfaces;

namespace Lexicanum.Features.Quizlet
{
    public class QuizQuestion : IContentItem
    {
        public string Title { get; }
        public string Content { get; }
        public string Question { get; }
        public string[] Options { get; }
        public int CorrectAnswerIndex { get; }
        public string Explanation { get; }

        public QuizQuestion(string title, string question, string[] options, int correctAnswerIndex, string explanation)
        {
            Title = title;
            Content = question;
            Question = question;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            Explanation = explanation;
        }

        public void Display()
        {
            // Quiz questions are displayed through QuizSession
            Console.WriteLine($"Question: {Question}");
        }

        public bool CheckAnswer(int answerIndex)
        {
            return answerIndex == CorrectAnswerIndex;
        }
    }
}
