using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class StatusOrderItemTable
{
    [Key]
    public int StatusOrderItemId { get; set; }

    public string? StatusOrderItemTable1 { get; set; }

    public virtual ICollection<OrderItemTable> OrderItemTables { get; } = new List<OrderItemTable>();
}
