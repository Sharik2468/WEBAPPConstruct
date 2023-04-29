using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using InternetShopWebApp.Repository;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly Context.InternetShopContext _context;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        //public ProductController(Context.InternetShopContext context)
        //{
        //    _context = context;
        //    //if (!_context.Product.Any())
        //    //{
        //    //    _context.Product.Add(new Product
        //    //    {
        //    //        Product_Code = 1,
        //    //        Name = "Nokia",
        //    //        CategoryID = 1,
        //    //        Desctription = "asdasd"
        //    //    });
        //    //    _context.SaveChanges();
        //    //}
        //}

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
            var blog = _unitOfWork.ProductRepository.GetByID(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductTable>> NewProduct(ProductTable Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.ProductTables.Add(Product);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = Product.ProductCode }, Product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductTable Product)
        {
            if (id != Product.ProductCode)
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
            return _context.ProductTables.Any(e => e.ProductCode == id);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var blog = await _context.ProductTables.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.ProductTables.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}