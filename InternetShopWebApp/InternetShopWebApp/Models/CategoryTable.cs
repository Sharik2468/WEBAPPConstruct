using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models;

public partial class CategoryTable
{
    [Key]
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int ParentId { get; set; }

    public virtual ICollection<CategoryTable> InverseParent { get; } = new List<CategoryTable>();

    public virtual CategoryTable Parent { get; set; } = null!;

    public virtual ICollection<ProductTable> ProductTables { get; } = new List<ProductTable>();
}
