using System.ComponentModel.DataAnnotations;


namespace InternetShopWebApp.Models
{
    public class ProductModel
    {

        public ProductModel() 
        {
            Cathegories = new HashSet<CathegoryModel>();
        }

        [Key]
        public int Product_Code { get; set; }
        public int NumberInStock { get; set; }
        public int CategoryID { get; set; }
        public System.DateTime DateOfManufacture { get; set; }
        public string Description { get; set; }
        public int PurchasePrice { get; set; }
        public int MarketPrice { get; set; }
        public float BestBeforeDate { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CathegoryModel> Cathegories { get; set; }
    }
}
