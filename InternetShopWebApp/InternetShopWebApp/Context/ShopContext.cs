using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetShopWebApp.Context
{
    public class ShopContext : IdentityDbContext<User>
    {
        protected readonly IConfiguration Configuration;
        #region Constructors
        //public Context(DbContextOptions<Context> options) : base(options) { }
        public ShopContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public virtual DbSet<ProductModel> Product { get; set; }
        public virtual DbSet<CathegoryModel> Cathegory { get; set; }
        public virtual DbSet<OrderItemModel> OrderItem { get; set; }
        protected override void OnModelCreating(ModelBuilder
        modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductModel>(entity =>
            {
                //entity.Property(e => e.Product_Code).IsRequired();
                entity.HasOne(d => d.ProductOrderItem)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.OrderItem_Code);
            });
            modelBuilder.Entity<CathegoryModel>(entity =>
            {
                entity.HasKey(e => e.Cathegory_ID);
                //entity.HasOne(d => d.Product)
                //.WithMany(p => p.Cathegories)
                //.HasForeignKey(d => d.Cathegory_ID);
            });
            modelBuilder.Entity<OrderItemModel>(entity =>
            {
                entity.Property(e => e.Order_Item_Code).IsRequired();
            });
        }
    }
}
