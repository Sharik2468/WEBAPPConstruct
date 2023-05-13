using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using InternetShopWebApp.Repository;
using InternetShopWebApp.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService ;
        public ProductController(ProductService newProductService)
        {
            _productService = newProductService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTable>>> GetAllProduct()
        {
            return _productService.GetAllProductService();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTable>> GetProduct(int id)
        {
            ProductTable product = _productService.GetProductByIDService(id);
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
            byte[] imageBytes = _productService.GetImageService(id);

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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_productService.NewProductService(Product)) return BadRequest(ModelState);
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
            try
            {
                _productService.PutProductService(Product);
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
            try
            {
                _productService.PutNewAmountService(id, amount); 
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
            return _productService.ProductExistService(id);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productService.GetProductByIDService(id);
            if (product == null)
            {
                return NotFound();
            }
            if (!_productService.DeleteSaveProductService(product)) return BadRequest();
            return NoContent();
        }
    }
}