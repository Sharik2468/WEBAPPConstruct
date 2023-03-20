using System;
using System.Collections.Generic;

namespace InternetShopWebApp.Models1;

public partial class OrderItem
{
    public int OrderItemCode { get; set; }

    public int? OrderSum { get; set; }

    public int? AmountOrderItem { get; set; }

    public int? ProductCode { get; set; }

    public int? OrderCode { get; set; }

    public int StatusOrderItemTableId { get; set; }

    public int? ResultSum { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
