using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models
{
    public class CathegoryModel
    {
        public CathegoryModel()
        {
        }

        [Key]
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public int Parent_ID { get; set; }
        public virtual CathegoryModel ParentCathegory { get; set; }
    }
}
