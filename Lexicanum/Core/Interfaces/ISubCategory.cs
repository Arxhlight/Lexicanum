namespace Lexicanum.Core.Interfaces
{
    public interface ISubCategory
    {
        string Name { get; }
        string Description { get; }
        List<IContentItem> ContentItems { get; }
        List<ISubCategory> SubCategories { get; }
        void Execute();
    }
}
