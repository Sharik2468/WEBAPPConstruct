using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopWebApp.Services
{
    public class ProductService
    {
        private UnitOfWork _unitOfWork;
        private OrderServices _orderService;

        public ProductService(OrderServices newOrderService, UnitOfWork newUnitOfWork)
        {
            _orderService = newOrderService;
            _unitOfWork = newUnitOfWork;
        }
        public List<ProductTable> GetProductsByOrderItemID(int ClientID)
        {
            var orderItems = _orderService.GetAllActiveOrderItemsByClientId(ClientID);

            var products = _unitOfWork.ProductRepository.Get();
            List<ProductTable> resultProducts = new List<ProductTable>();
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
            try
            {
                var allCategory = _unitOfWork.CategoryRepository.Get();
                var category = allCategory.FirstOrDefault(a => a.CategoryName == CategoryName);
                var allProducts = _unitOfWork.ProductRepository.Get();

                return (List<ProductTable>)(category == null ? allProducts :
                    allProducts.Where(a => a.CategoryId == category.CategoryId).ToList());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ProductTable> GetProductSearch(string SearchName, string CategoryName)
        {
            List<ProductTable> productsBySearchName;
            if (SearchName != null) productsBySearchName = GetProductBySearchName(SearchName);
            else productsBySearchName = (List<ProductTable>)_unitOfWork.ProductRepository.Get();

            List<ProductTable> productsByCategoryName;
            if (CategoryName != null) productsByCategoryName = GetProductsByCategory(CategoryName);
            else productsByCategoryName = (List<ProductTable>)_unitOfWork.ProductRepository.Get();

            if (productsBySearchName == null) return null;
            if (productsByCategoryName == null) return null;

            return productsByCategoryName.Intersect(productsBySearchName).ToList();
        }

        #region Services

        public ActionResult<IEnumerable<ProductTable>> GetAllProductService()
        {
            return _unitOfWork.ProductRepository.Get().ToList();
        }
        public ProductTable GetProductByIDService(int id)
        {
            return _unitOfWork.ProductRepository.GetByID(id);
        }
        /// <summary>
        /// Возвращает фотографию продукта
        /// </summary>
        /// <param name="id">Код продукта</param>
        /// <returns></returns>
        public byte[] GetImageService(int id)
        {
            // Загрузите байты изображения из базы данных или другого источника по ID
            return _unitOfWork.ProductRepository.GetByID(id) != null ?
                                _unitOfWork.ProductRepository.GetByID(id).Image :
                                null;
        }
        public bool NewProductService(ProductTable Product)
        {
            try
            {
                var allproducts = _unitOfWork.ProductRepository.Get();
                int maxIndex = allproducts.Max(a => a.ProductCode);
                Product.ProductCode = maxIndex + 1;
                _unitOfWork.ProductRepository.Insert(Product);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void PutProductService(ProductTable Product)
        {
            _unitOfWork.ProductRepository.Update(Product);
            _unitOfWork.Save();
        }
        /// <summary>
        /// Устанавливает новое знанчение количества продуктов на складе
        /// </summary>
        /// <param name="id">Код продукта</param>
        /// <param name="amount">Количество продукта</param>
        public void PutNewAmountService(int id, int amount)
        {
            var product = _unitOfWork.ProductRepository.GetByID(id);
            product.NumberInStock = amount;
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Save();
        }
        public bool ProductExistService(int id)
        {
            return _unitOfWork.ProductRepository.GetByID(id) != null;
        }
        public bool DeleteSaveProductService(ProductTable product)
        {
            try
            {
                _unitOfWork.ProductRepository.Delete(product);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion
    }
}
