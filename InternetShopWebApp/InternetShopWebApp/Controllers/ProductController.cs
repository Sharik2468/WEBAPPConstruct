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

        [HttpGet("GetImage/{id}")]
        public IActionResult GetImage(int id)
        {
            // Загрузите байты изображения из базы данных или другого источника по ID
            byte[] imageBytes = _unitOfWork.ProductRepository.GetByID(id).Image;

            if (imageBytes == null)
            {
                return NotFound();
            }

            return File(imageBytes, "image/jpeg");
        }


        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductTable>> NewProduct(ProductTable Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.ProductRepository.Insert(Product);
            _unitOfWork.Save();
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

        private bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.GetByID(id) != null;
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //var blog = await _context.ProductTables.FindAsync(id);
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