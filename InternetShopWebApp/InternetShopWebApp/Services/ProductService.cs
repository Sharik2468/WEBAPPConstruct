using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;

namespace InternetShopWebApp.Services
{
    public class ProductService
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        private OrderServices _orderService = new OrderServices();

        public List<ProductTable> GetProductsByOrderItemID(int ClientID)
        {
            var orderItems = _orderService.GetAllActiveOrderItemsByClientId(ClientID);

            var products = _unitOfWork.ProductRepository.Get();
            List<ProductTable> resultProducts= new List<ProductTable>();
            foreach (var item in orderItems)
            {
                resultProducts.Add(products.FirstOrDefault(a => a.ProductCode == item.ProductCode));
            }

            return resultProducts;
        }

        public List<ProductTable> GetProductBySearchName(string SearchName)
        {
            var allProducts = _unitOfWork.ProductRepository.Get();

            List<ProductTable> resultProducts = new List<ProductTable>();
            foreach (var prod in allProducts)
            {
                if (prod.NameProduct.Contains(SearchName) || prod.Description.Contains(SearchName))
                    resultProducts.Add(prod);
            }

            return resultProducts;
        }


    }
}
