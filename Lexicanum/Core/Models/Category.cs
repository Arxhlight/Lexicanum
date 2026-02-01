using Lexicanum.Core.Interfaces;

namespace Lexicanum.Core.Models
{
    public class Category : ICategory
    {
        public string Name { get; }
        public string Description { get; }
        public List<ISubCategory> SubCategories { get; } = new();

        private readonly Action<Category>? _onExecute;

        public Category(string name, string description, Action<Category>? onExecute = null)
        {
            Name = name;
            Description = description;
            _onExecute = onExecute;
        }

        public void AddSubCategory(ISubCategory subCategory)
        {
            SubCategories.Add(subCategory);
        }

        public void RemoveSubCategory(string name)
        {
            var subCategory = SubCategories.FirstOrDefault(s => s.Name == name);
            if (subCategory != null)
            {
                SubCategories.Remove(subCategory);
            }
        }

        public void Execute()
        {
            _onExecute?.Invoke(this);
        }
    }
}
