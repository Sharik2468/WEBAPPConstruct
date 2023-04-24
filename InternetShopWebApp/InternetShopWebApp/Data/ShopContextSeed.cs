using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection.Metadata;

using InternetShopWebApp.Context;
using InternetShopWebApp.Models;

namespace InternetShopWebApp.Data
{
    public class ShopContextSeed
    {
        public static async Task SeedAsync(InternetShopContext context)
        {
            //try
            //{
            //    context.Database.EnsureCreated();
            //    if (context.Product.Any())
            //    {
            //        return;
            //    }

            //    var cathegories = new CategoryModel[]
            //    {
            //        new CategoryModel {
            //        Category_Name = "Smartphones",
            //        Parent_ID=1
            //        },

            //        new CategoryModel
            //        {
            //        Category_Name = "Consoles",
            //        Parent_ID=2
            //        }
            //    };
            //    foreach (CategoryModel cathegory in cathegories)
            //    {
            //        context.Cathegory.Add(cathegory);
            //    }
            //    await context.SaveChangesAsync();

            //    var orderitems = new OrderItemModel[]
            //    {
            //        new OrderItemModel {
            //        Order_Sum = 100,
            //        Amount_Order_Item = 1,
            //        Product_Code = 1,
            //        Order_Code = 1,
            //        Status_Order_Item_Table_ID = 1
            //        },

            //        new OrderItemModel
            //        {
            //        Order_Sum = 100,
            //        Amount_Order_Item = 1,
            //        Product_Code = 2,
            //        Order_Code = 1,
            //        Status_Order_Item_Table_ID = 1
            //        }
            //    };
            //    foreach (OrderItemModel orderitem in orderitems)
            //    {
            //        context.OrderItem.Add(orderitem);
            //    }
            //    await context.SaveChangesAsync();

            //    var products = new Product[]
            //    {
            //        new Product {
            //        Name = "Nokia",
            //        CategoryID = 1,
            //        //OrderItem_Code=1,
            //        Desctription = "asdasd" },

            //        new Product {
            //        Name = "Samsung",
            //        CategoryID = 1,
            //        //OrderItem_Code=2,
            //        Desctription = "safasgag" },
            //    };
            //    foreach (Product product in products)
            //    {
            //        context.Product.Add(product);
            //    }
            //    await context.SaveChangesAsync();
            //}
            //catch
            //{
            //    throw;
            //}
        }
    }
}


// ... .Include(p=>p.Post). ...