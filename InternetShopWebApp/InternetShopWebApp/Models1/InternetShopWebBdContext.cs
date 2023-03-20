using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InternetShopWebApp.Models1;

public partial class InternetShopWebBdContext : DbContext
{
    public InternetShopWebBdContext()
    {
    }

    public InternetShopWebBdContext(DbContextOptions<InternetShopWebBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cathegory> Cathegories { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-T0L4JP9;Database=InternetShopWebBD;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cathegory>(entity =>
        {
            entity.ToTable("Cathegory");

            entity.Property(e => e.CathegoryId).HasColumnName("Cathegory_ID");
            entity.Property(e => e.CathegoryName).HasColumnName("Cathegory_Name");
            entity.Property(e => e.ParentId).HasColumnName("Parent_ID");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemCode);

            entity.ToTable("OrderItem");

            entity.Property(e => e.OrderItemCode).HasColumnName("Order_Item_Code");
            entity.Property(e => e.AmountOrderItem).HasColumnName("Amount_Order_Item");
            entity.Property(e => e.OrderCode).HasColumnName("Order_Code");
            entity.Property(e => e.OrderSum).HasColumnName("Order_Sum");
            entity.Property(e => e.ProductCode).HasColumnName("Product_Code");
            entity.Property(e => e.ResultSum).HasColumnName("Result_Sum");
            entity.Property(e => e.StatusOrderItemTableId).HasColumnName("Status_Order_Item_Table_ID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductCode);

            entity.ToTable("Product");

            entity.HasIndex(e => e.OrderItemCode, "IX_Product_OrderItem_Code");

            entity.Property(e => e.ProductCode).HasColumnName("Product_Code");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.OrderItemCode).HasColumnName("OrderItem_Code");

            entity.HasOne(d => d.OrderItemCodeNavigation).WithMany(p => p.Products).HasForeignKey(d => d.OrderItemCode);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
