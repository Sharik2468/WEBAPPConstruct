using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CathegoryController : ControllerBase
    {
        private readonly Context.Context _context;
        public CathegoryController(Context.Context context)
        {
            _context = context;
            if (!_context.Cathegory.Any())
            {
                _context.Cathegory.Add(new CathegoryModel
                {
                    Category_ID = 1,
                    Category_Name = "Smartphones",
                    Parent_ID = 1
                });
                _context.SaveChanges();
            }
        }

        // GET: api/Cathegorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CathegoryModel>>> GetAllCathegory()
        {
            return await _context.Cathegory.ToListAsync();
        }
        // GET: api/Cathegorys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CathegoryModel>> GetCathegory(int id)
        {
            var blog = await _context.Cathegory.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Cathegory
        [HttpPost]
        public async Task<ActionResult<CathegoryModel>> NewCathegory(CathegoryModel Cathegory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Cathegory.Add(Cathegory);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCathegory", new { id = Cathegory.Category_ID }, Cathegory);
        }

        // PUT: api/Cathegory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCathegory(int id, CathegoryModel Cathegory)
        {
            if (id != Cathegory.Category_ID)
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
            return _context.Cathegory.Any(e => e.Category_ID == id);
        }

        // DELETE: api/Cathegory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCathegory(int id)
        {
            var blog = await _context.Cathegory.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Cathegory.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}