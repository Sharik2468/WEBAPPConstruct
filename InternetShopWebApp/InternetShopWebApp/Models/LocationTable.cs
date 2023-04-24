using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class LocationTable
{
    [Key]
    public int LocationCode { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ClientTable> ClientTables { get; } = new List<ClientTable>();

    public virtual ICollection<SalesmanTable> SalesmanTables { get; } = new List<SalesmanTable>();
}
