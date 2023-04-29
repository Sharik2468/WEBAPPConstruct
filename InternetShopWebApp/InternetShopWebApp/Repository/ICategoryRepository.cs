using InternetShopWebApp.Models;

namespace InternetShopWebApp.Repository
{
    public interface ICategoryRepository : IDisposable
    {
        IEnumerable<CategoryTable> GetCategories();
        CategoryTable GetCategoryByID(int categoryId);
        void InsertCategory(CategoryTable category);
        void DeleteCategory(int categoryID);
        void UpdateCategory(CategoryTable category);
        Task Save();
    }
}
