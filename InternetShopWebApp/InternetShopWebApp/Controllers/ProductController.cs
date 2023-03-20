using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Context.ShopContext _context;
        public ProductController(Context.ShopContext context)
        {
            _context = context;
            if (!_context.Product.Any())
            {
                _context.Product.Add(new ProductModel
                {
                    Product_Code = 1,
                    Name = "Nokia",
                    CategoryID = 1,
                    Description = "asdasd"
                });
                _context.SaveChanges();
            }
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProduct()
        {
            return await _context.Product.ToListAsync();
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var blog = await _context.Product.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductModel>> NewProduct(ProductModel Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = Product.Product_Code }, Product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductModel Product)
        {
            if (id != Product.Product_Code)
            {
                return BadRequest();
            }
            _context.Entry(Product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
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
            return _context.Product.Any(e => e.Product_Code == id);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var blog = await _context.Product.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Product.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}