using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class ProductTable
{
    [Key]
    public int ProductCode { get; set; }

    public string? NameProduct { get; set; }

    public int? MarketPriceProduct { get; set; }

    public int? PurchasePriceProduct { get; set; }

    public DateTime? DateOfManufactureProduct { get; set; }

    public int? BestBeforeDateProduct { get; set; }

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public int? NumberInStock { get; set; }

    public virtual CategoryTable? Category { get; set; }

    public virtual ICollection<OrderItemTable> OrderItemTables { get; } = new List<OrderItemTable>();
}
