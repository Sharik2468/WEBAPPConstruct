using System;
using System.Collections.Generic;

namespace InternetShopWebApp.Models1;

public partial class Product
{
    public int ProductCode { get; set; }

    public int OrderItemCode { get; set; }

    public int NumberInStock { get; set; }

    public int CategoryId { get; set; }

    public DateTime DateOfManufacture { get; set; }

    public string Description { get; set; } = null!;

    public int PurchasePrice { get; set; }

    public int MarketPrice { get; set; }

    public float BestBeforeDate { get; set; }

    public string Name { get; set; } = null!;

    public virtual OrderItem OrderItemCodeNavigation { get; set; } = null!;
}
