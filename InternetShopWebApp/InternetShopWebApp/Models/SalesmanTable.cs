using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class SalesmanTable
{
    [Key]
    public int SalesmanCode { get; set; }

    public string? SalemanName { get; set; }

    public string? SalesmanSurname { get; set; }

    public long? TelephoneNumber { get; set; }

    public string? Password { get; set; }

    public int? LocationCode { get; set; }

    public virtual LocationTable? LocationCodeNavigation { get; set; }

    public virtual ICollection<OrderTable> OrderTables { get; } = new List<OrderTable>();
}
