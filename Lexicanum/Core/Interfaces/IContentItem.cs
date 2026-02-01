namespace Lexicanum.Core.Interfaces
{
    public interface IContentItem
    {
        string Title { get; }
        string Content { get; }
        void Display();
    }
}
