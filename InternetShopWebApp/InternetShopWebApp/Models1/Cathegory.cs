using System;
using System.Collections.Generic;

namespace InternetShopWebApp.Models1;

public partial class Cathegory
{
    public int CathegoryId { get; set; }

    public string CathegoryName { get; set; } = null!;

    public int ParentId { get; set; }
}
