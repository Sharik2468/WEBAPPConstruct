using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]

    public class CathegoryController : ControllerBase
    {
        private readonly Context.InternetShopContext _context;
        public CathegoryController(Context.InternetShopContext context)
        {
            _context = context;
            //if (!_context.Cathegory.Any())
            //{
            //    _context.Cathegory.Add(new CategoryModel
            //    {
            //        Category_ID = 1,
            //        Category_Name = "Smartphones",
            //        Parent_ID = 1
            //    });
            //    _context.SaveChanges();
            //}
        }

        // GET: api/Cathegorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryTable>>> GetAllCathegory()
        {
            return await _context.CategoryTables.ToListAsync();
        }
        // GET: api/Cathegorys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryTable>> GetCathegory(int id)
        {
            var blog = await _context.CategoryTables.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Cathegory
        [HttpPost]
        public async Task<ActionResult<CategoryTable>> NewCathegory(CategoryTable Cathegory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.CategoryTables.Add(Cathegory);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCathegory", new { id = Cathegory.CategoryId }, Cathegory);
        }

        // PUT: api/Cathegory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCathegory(int id, CategoryTable Cathegory)
        {
            if (id != Cathegory.CategoryId)
            {
                return BadRequest();
            }
            _context.Entry(Cathegory).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CathegoryExists(id))
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

        private bool CathegoryExists(int id)
        {
            return _context.CategoryTables.Any(e => e.CategoryId == id);
        }

        // DELETE: api/Cathegory/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCathegory(int id)
        {
            var blog = await _context.CategoryTables.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.CategoryTables.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}