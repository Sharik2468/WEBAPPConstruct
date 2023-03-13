using InternetShopWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopWebApp.Context
{
    public class Context : DbContext
    {
        #region Constructor
        public Context(DbContextOptions<Context> options) : base(options) { }
        #endregion
        public virtual DbSet<ProductModel> Product { get; set; }
        public virtual DbSet<CathegoryModel> Cathegory { get; set; }
        public virtual DbSet<OrderItemModel> OrderItem { get; set; }
        protected override void OnModelCreating(ModelBuilder
        modelBuilder)
        {
            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.Property(e => e.Product_Code).IsRequired();
            });
            modelBuilder.Entity<CathegoryModel>(entity =>
            {
                entity.HasKey(e => e.Parent_ID);
            });
            modelBuilder.Entity<OrderItemModel>(entity =>
            {
                entity.Property(e => e.Order_Item_Code).IsRequired();
            });
        }
    }
}
