using Lexicanum.Core.Interfaces;
using Lexicanum.UI;
using Spectre.Console;

namespace Lexicanum.Core.Models;

public class ContentItem : IContentItem
{
    public string Title { get; }
    public string Content { get; }

    public ContentItem(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public void Display(IAnsiConsole console)
    {
        console.ShowContentPage(Title, Content);
    }
}
