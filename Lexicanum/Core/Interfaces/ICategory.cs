namespace Lexicanum.Core.Interfaces
{
    public interface ICategory
    {
        string Name { get; }
        string Description { get; }
        List<ISubCategory> SubCategories { get; }
        void Execute();
    }
}
