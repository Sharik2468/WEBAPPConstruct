using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class OrderItemTable
{
    [Key]
    public int OrderItemCode { get; set; }

    public int? OrderSum { get; set; }

    public int? AmountOrderItem { get; set; }

    public int? ProductCode { get; set; }

    public int? OrderCode { get; set; }

    public int StatusOrderItemTableId { get; set; }

    public virtual OrderTable? OrderCodeNavigation { get; set; }

    public virtual ProductTable? ProductCodeNavigation { get; set; }

    public virtual StatusOrderItemTable StatusOrderItemTable { get; set; } = null!;
}
