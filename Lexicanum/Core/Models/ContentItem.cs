using Lexicanum.Core.Interfaces;
using Lexicanum.UI;

namespace Lexicanum.Core.Models
{
    public class ContentItem : IContentItem
    {
        public string Title { get; }
        public string Content { get; }

        public ContentItem(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public void Display()
        {
            var helper = new ConsoleHelper();
            helper.ShowContentPage(Title, Content);
        }
    }
}
