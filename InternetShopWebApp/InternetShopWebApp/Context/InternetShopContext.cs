using System;
using System.Collections.Generic;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetShopWebApp.Context;

public partial class InternetShopContext : IdentityDbContext<User>
{
    protected readonly IConfiguration Configuration;
    public InternetShopContext()
    {
    }

    public InternetShopContext(DbContextOptions<InternetShopContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    
    //#region Constructors
    ////public Context(DbContextOptions<Context> options) : base(options) { }
    //public InternetShopContext(IConfiguration configuration)
    //{
    //    Configuration = configuration;
    //}
    //#endregion

    public virtual DbSet<CategoryTable> CategoryTables { get; set; }

    public virtual DbSet<ClientTable> ClientTables { get; set; }

    public virtual DbSet<LocationTable> LocationTables { get; set; }

    public virtual DbSet<OrderItemTable> OrderItemTables { get; set; }

    public virtual DbSet<OrderTable> OrderTables { get; set; }

    public virtual DbSet<ProductTable> ProductTables { get; set; }

    public virtual DbSet<SalesmanTable> SalesmanTables { get; set; }

    public virtual DbSet<StatusOrderItemTable> StatusOrderItemTables { get; set; }

    public virtual DbSet<StatusTable> StatusTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-T0L4JP9;Initial Catalog=Internet_Shop;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CategoryTable>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("Category_Table");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("Category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("Category_Name");
            entity.Property(e => e.ParentId).HasColumnName("Parent_ID");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Category_Table_Category_Table");
        });

        modelBuilder.Entity<ClientTable>(entity =>
        {
            entity.HasKey(e => e.ClientCode);

            entity.ToTable("Client_Table");

            entity.Property(e => e.ClientCode)
                .ValueGeneratedNever()
                .HasColumnName("Client_Code");
            entity.Property(e => e.LocationCode).HasColumnName("Location_Code");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.TelephoneNumber).HasColumnName("Telephone_Number");

            entity.HasOne(d => d.LocationCodeNavigation).WithMany(p => p.ClientTables)
                .HasForeignKey(d => d.LocationCode)
                .HasConstraintName("FK_Client_Table_Location_Table");
        });

        modelBuilder.Entity<LocationTable>(entity =>
        {
            entity.HasKey(e => e.LocationCode);

            entity.ToTable("Location_Table");

            entity.Property(e => e.LocationCode)
                .ValueGeneratedNever()
                .HasColumnName("Location_Code");
        });

        modelBuilder.Entity<OrderItemTable>(entity =>
        {
            entity.HasKey(e => e.OrderItemCode);

            entity.ToTable("Order_Item_Table");

            entity.Property(e => e.OrderItemCode)
                .ValueGeneratedNever()
                .HasColumnName("Order_Item_Code");
            entity.Property(e => e.AmountOrderItem).HasColumnName("Amount_Order_Item");
            entity.Property(e => e.OrderCode).HasColumnName("Order_Code");
            entity.Property(e => e.OrderSum).HasColumnName("Order_Sum");
            entity.Property(e => e.ProductCode).HasColumnName("Product_Code");
            entity.Property(e => e.StatusOrderItemTableId).HasColumnName("Status_Order_Item_Table_ID");

            entity.HasOne(d => d.OrderCodeNavigation).WithMany(p => p.OrderItemTables)
                .HasForeignKey(d => d.OrderCode)
                .HasConstraintName("FK_Order_Item_Table_Order_Table");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.OrderItemTables)
                .HasForeignKey(d => d.ProductCode)
                .HasConstraintName("FK_Order_Item_Table_Product_Table");

            entity.HasOne(d => d.StatusOrderItemTable).WithMany(p => p.OrderItemTables)
                .HasForeignKey(d => d.StatusOrderItemTableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Item_Table_Status_Order_Item_Table");
        });

        modelBuilder.Entity<OrderTable>(entity =>
        {
            entity.HasKey(e => e.OrderCode);

            entity.ToTable("Order_Table");

            entity.Property(e => e.OrderCode)
                .ValueGeneratedNever()
                .HasColumnName("Order_Code");
            entity.Property(e => e.ClientCode).HasColumnName("Client_Code");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderFullfillment)
                .HasColumnType("date")
                .HasColumnName("Order_Fullfillment");
            entity.Property(e => e.SalesmanCode).HasColumnName("Salesman_Code");
            entity.Property(e => e.StatusCode).HasColumnName("Status_Code");

            entity.HasOne(d => d.ClientCodeNavigation).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.ClientCode)
                .HasConstraintName("FK_Order_Table_Client_Table");

            entity.HasOne(d => d.SalesmanCodeNavigation).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.SalesmanCode)
                .HasConstraintName("FK_Order_Table_Salesman_Table");

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.OrderTables)
                .HasForeignKey(d => d.StatusCode)
                .HasConstraintName("FK_Order_Table_Delivery_Table");
        });

        modelBuilder.Entity<ProductTable>(entity =>
        {
            entity.HasKey(e => e.ProductCode);

            entity.ToTable("Product_Table");

            entity.Property(e => e.ProductCode)
                .ValueGeneratedNever()
                .HasColumnName("Product_Code");
            entity.Property(e => e.BestBeforeDateProduct).HasColumnName("Best_Before_Date_Product");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.DateOfManufactureProduct)
                .HasColumnType("date")
                .HasColumnName("Date_of_Manufacture_Product");
            entity.Property(e => e.MarketPriceProduct).HasColumnName("Market_Price_Product");
            entity.Property(e => e.NameProduct).HasColumnName("Name_Product");
            entity.Property(e => e.NumberInStock).HasColumnName("Number_in_Stock");
            entity.Property(e => e.PurchasePriceProduct).HasColumnName("Purchase_Price_Product");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductTables)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Table_Category_Table");
        });

        modelBuilder.Entity<SalesmanTable>(entity =>
        {
            entity.HasKey(e => e.SalesmanCode);

            entity.ToTable("Salesman_Table");

            entity.Property(e => e.SalesmanCode)
                .ValueGeneratedNever()
                .HasColumnName("Salesman_Code");
            entity.Property(e => e.LocationCode).HasColumnName("Location_Code");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.SalemanName).HasColumnName("Saleman_Name");
            entity.Property(e => e.SalesmanSurname).HasColumnName("Salesman_Surname");
            entity.Property(e => e.TelephoneNumber).HasColumnName("Telephone_Number");

            entity.HasOne(d => d.LocationCodeNavigation).WithMany(p => p.SalesmanTables)
                .HasForeignKey(d => d.LocationCode)
                .HasConstraintName("FK_Salesman_Table_Location_Table");
        });

        modelBuilder.Entity<StatusOrderItemTable>(entity =>
        {
            entity.HasKey(e => e.StatusOrderItemId);

            entity.ToTable("Status_Order_Item_Table");

            entity.Property(e => e.StatusOrderItemId)
                .ValueGeneratedNever()
                .HasColumnName("Status_Order_Item_ID");
            entity.Property(e => e.StatusOrderItemTable1)
                .HasMaxLength(50)
                .HasColumnName("Status_Order_Item_Table");
        });

        modelBuilder.Entity<StatusTable>(entity =>
        {
            entity.HasKey(e => e.StatusCode).HasName("PK_Delivery_Table");

            entity.ToTable("Status_Table");

            entity.Property(e => e.StatusCode)
                .ValueGeneratedNever()
                .HasColumnName("Status_Code");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Order_Status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
