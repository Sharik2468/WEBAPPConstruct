using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using InternetShopWebApp.Repository;
using InternetShopWebApp.Services;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductService _productService = new ProductService();
        public ProductController(UnitOfWork newUnitOfWork)
        {
            _unitOfWork = newUnitOfWork;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTable>>> GetAllProduct()
        {
            return _unitOfWork.ProductRepository.Get().ToList();
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTable>> GetProduct(int id)
        {
            var product = _unitOfWork.ProductRepository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // GET: api/Products/Search/Бытовая техника.Телевизор
        [HttpGet("Search/{SearchWord}")]
        public async Task<ActionResult<IEnumerable<ProductTable>>> GetProductBySearch(string SearchWord)
        {
            string[] words = SearchWord.Split(new char[] { '.' });
            // new char[] - массив символов-разделителей. Как меня поправили в 
            // комментариях, в данном случае достаточно написать text.Split(':')

            string first = words[0] == null ? null : words[0];
            string second = words[1] == null ? null : words[1];

            var product = _productService.GetProductSearch(second, first);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // GET: api/Products/categorySearch/Бытовая техника
        [HttpGet("categorySearch/{CategoryName}")]
        public async Task<ActionResult<IEnumerable<ProductTable>>> GetProductByCategory(string CategoryName)
        {
            var product = _productService.GetProductsByCategory(CategoryName);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // GET: api/Products/categorySearch/Бытовая техника
        [HttpGet("keywordSearch/{keyword}")]
        public async Task<ActionResult<IEnumerable<ProductTable>>> GetProductByKeyword(string keyword)
        {
            var product = _productService.GetProductBySearchName(keyword);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpGet("GetImage/{id}")]
        public IActionResult GetImage(int id)
        {
            // Загрузите байты изображения из базы данных или другого источника по ID
            byte[] imageBytes = _unitOfWork.ProductRepository.GetByID(id) != null ?
                                _unitOfWork.ProductRepository.GetByID(id).Image :
                                null;

            if (imageBytes == null)
            {
                return NotFound();
            }

            return File(imageBytes, "image/jpeg");
        }


        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ProductTable>> NewProduct(ProductTable Product)
        {
            var allproducts = _unitOfWork.ProductRepository.Get();
            int maxIndex = allproducts.Max(a => a.ProductCode);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product.ProductCode = maxIndex + 1;
            _unitOfWork.ProductRepository.Insert(Product);
            _unitOfWork.Save();
            return CreatedAtAction("GetProduct", new { id = Product.ProductCode }, Product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutProduct(int id, ProductTable Product)
        {
            if (id != Product.ProductCode)
            {
                return BadRequest();
            }
            //_context.Entry(Product).State = EntityState.Modified;
            _unitOfWork.ProductRepository.Update(Product);
            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // PUT: api/Product/5
        [HttpPut("{id}/{amount}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutNewAmountProduct(int id, int amount)
        {
            //_context.Entry(Product).State = EntityState.Modified;
            //_unitOfWork.ProductRepository.Update(Product);
            var product = _unitOfWork.ProductRepository.GetByID(id);
            product.NumberInStock = amount;
            _unitOfWork.ProductRepository.Update(product);
            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.GetByID(id) != null;
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //var product = await _context.ProductTables.FindAsync(id);
            var product = _unitOfWork.ProductRepository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}