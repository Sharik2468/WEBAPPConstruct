using InternetShopWebApp.Context;
using InternetShopWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopWebApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private InternetShopContext context;

        public CategoryRepository(InternetShopContext context)
        {
            this.context = context;
        }

        public IEnumerable<CategoryTable> GetCategories()
        {
            return context.CategoryTables.ToList();
        }

        public CategoryTable GetCategoryByID(int id)
        {
            return context.CategoryTables.Find(id);
        }

        public void InsertCategory(CategoryTable Category)
        {
            context.CategoryTables.Add(Category);
        }

        public void DeleteCategory(int CategoryID)
        {
            CategoryTable Category = context.CategoryTables.Find(CategoryID);
            context.CategoryTables.Remove(Category);
        }

        public void UpdateCategory(CategoryTable Category)
        {
            context.Entry(Category).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
