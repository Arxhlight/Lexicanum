using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Models;

namespace Lexicanum.Core.Services
{
    public class ContentLoader
    {
        private readonly CategoryRegistry _registry;

        public ContentLoader(CategoryRegistry registry)
        {
            _registry = registry;
        }

        public void LoadCategory(ICategory category)
        {
            _registry.RegisterCategory(category);
        }

        public void LoadCategories(IEnumerable<ICategory> categories)
        {
            foreach (var category in categories)
            {
                _registry.RegisterCategory(category);
            }
        }

        public void UnloadCategory(string name)
        {
            _registry.UnregisterCategory(name);
        }
    }
}
