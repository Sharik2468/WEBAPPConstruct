using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class OrderTable
{
    [Key]
    public int OrderCode { get; set; }

    public DateTime? OrderFullfillment { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? ClientCode { get; set; }

    public int? SalesmanCode { get; set; }

    public int? StatusCode { get; set; }

    public virtual ClientTable? ClientCodeNavigation { get; set; }

    public virtual ICollection<OrderItemTable> OrderItemTables { get; } = new List<OrderItemTable>();

    public virtual SalesmanTable? SalesmanCodeNavigation { get; set; }

    public virtual StatusTable? StatusCodeNavigation { get; set; }
}
