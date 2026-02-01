using Lexicanum.Core.Interfaces;

namespace Lexicanum.Core.Services
{
    public class CategoryRegistry
    {
        private readonly Dictionary<string, ICategory> _categories = new();

        public void RegisterCategory(ICategory category)
        {
            _categories[category.Name.ToLowerInvariant()] = category;
        }

        public void UnregisterCategory(string name)
        {
            _categories.Remove(name.ToLowerInvariant());
        }

        public ICategory? GetCategory(string name)
        {
            _categories.TryGetValue(name.ToLowerInvariant(), out var category);
            return category;
        }

        public IEnumerable<ICategory> GetAllCategories()
        {
            return _categories.Values;
        }

        public bool HasCategory(string name)
        {
            return _categories.ContainsKey(name.ToLowerInvariant());
        }

        public int Count => _categories.Count;
    }
}
