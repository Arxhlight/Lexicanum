using Lexicanum.Core.Interfaces;

namespace Lexicanum.Core.Models;

public class SubCategory : ISubCategory
{
    public string Name { get; }
    public string Description { get; }
    public List<IContentItem> ContentItems { get; } = new();
    public List<ISubCategory> SubCategories { get; } = new();

    private readonly Action<SubCategory>? _onExecute;

    public SubCategory(string name, string description, Action<SubCategory>? onExecute = null)
    {
        Name = name;
        Description = description;
        _onExecute = onExecute;
    }

    public void AddSubCategory(ISubCategory subCategory)
    {
        SubCategories.Add(subCategory);
    }

    public void AddContentItem(IContentItem item)
    {
        ContentItems.Add(item);
    }

    public void Execute()
    {
        _onExecute?.Invoke(this);
    }
}
