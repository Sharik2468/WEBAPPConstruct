using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models
{
    public class OrderItemModel
    {
        public OrderItemModel()
        {
            Products = new HashSet<ProductModel>();
        }

        
        [Key]
        public int Order_Item_Code { get; set; }
        public Nullable<int> Order_Sum { get; set; }
        public Nullable<int> Amount_Order_Item { get; set; }
        public Nullable<int> Product_Code { get; set; }
        public Nullable<int> Order_Code { get; set; }
        public int Status_Order_Item_Table_ID { get; set; }
        public virtual ICollection<ProductModel> Products { get; set; }
    }
}
