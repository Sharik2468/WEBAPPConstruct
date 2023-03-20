using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models
{
    public class CathegoryModel
    {
        public CathegoryModel()
        {
        }

        [Key]
        public int Cathegory_ID { get; set; }
        public string Cathegory_Name { get; set; }
        public int Parent_ID { get; set; }
        //public ProductModel Product { get; set; }
        //public virtual CathegoryModel ParentCathegory { get; set; }
      
    }
}
