using Lexicanum.Core.Interfaces;

namespace Lexicanum.Core.Services;

public class ContentLoader
{
    private readonly CategoryRegistry _registry;

    public ContentLoader(CategoryRegistry registry)
    {
        _registry = registry;
    }

    public void LoadCategories(IEnumerable<ICategory> categories)
    {
        foreach (var category in categories)
        {
            _registry.RegisterCategory(category);
        }
    }
}
