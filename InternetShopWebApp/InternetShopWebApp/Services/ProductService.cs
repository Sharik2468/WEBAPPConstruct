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

        public List<ProductTable> GetProductsByCategory(string CategoryName)
        {
            var allCategory = _unitOfWork.CategoryRepository.Get();
            var category = allCategory.FirstOrDefault(a => a.CategoryName == CategoryName);
            var allProducts = _unitOfWork.ProductRepository.Get();

            return allProducts.Where(a => a.CategoryId == category.CategoryId).ToList();
        }

        public List<ProductTable> GetProductSerach(string SearchName, string CategoryName)
        {
            List<ProductTable> productsBySearchName;
            if (SearchName != null) productsBySearchName = GetProductBySearchName(SearchName);
            else productsBySearchName = (List<ProductTable>)_unitOfWork.ProductRepository.Get();

            List<ProductTable> productsByCategoryName;
            if (SearchName != null) productsByCategoryName = GetProductsByCategory(CategoryName);
            else productsByCategoryName = (List<ProductTable>)_unitOfWork.ProductRepository.Get();

            return productsByCategoryName.Intersect(productsBySearchName).ToList();
        }
    }
}
