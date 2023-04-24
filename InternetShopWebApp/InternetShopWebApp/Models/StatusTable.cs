using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class StatusTable
{
    [Key]
    public int StatusCode { get; set; }

    public string? OrderStatus { get; set; }

    public virtual ICollection<OrderTable> OrderTables { get; } = new List<OrderTable>();
}
