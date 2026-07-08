using Lexicanum.Core.Interfaces;

namespace Lexicanum.Core.Services;

public class CategoryRegistry
{
    private readonly Dictionary<string, ICategory> _categories = new();

    public void RegisterCategory(ICategory category)
    {
        _categories[category.Name.ToLowerInvariant()] = category;
    }

    public IEnumerable<ICategory> GetAllCategories()
    {
        return _categories.Values;
    }
}
