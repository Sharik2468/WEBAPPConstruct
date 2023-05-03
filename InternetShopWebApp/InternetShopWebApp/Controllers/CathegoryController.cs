using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using InternetShopWebApp.Repository;
using InternetShopWebApp.Context;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]

    public class CathegoryController : ControllerBase
    {

        private readonly UnitOfWork _unitOfWork;

        public CathegoryController(UnitOfWork newUnitOfWork)
        {
            _unitOfWork = newUnitOfWork;
        }


        // GET: api/Cathegorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryTable>>> GetAllCathegory()
        {
            //return await _context.CategoryTables.ToListAsync();
            var category = from s in _unitOfWork.CategoryRepository.Get() select s;
            return category.ToList();
        }
        // GET: api/Cathegorys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryTable>> GetCathegory(int id)
        {
            //var blog = await _context.CategoryTables.FindAsync(id);
            var category = _unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        // POST: api/Cathegory
        [HttpPost]
        public async Task<ActionResult<CategoryTable>> NewCathegory(CategoryTable Cathegory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_context.CategoryTables.Add(Cathegory);
            _unitOfWork.CategoryRepository.Insert(Cathegory);
            //await _context.SaveChangesAsync();
            _unitOfWork.Save();
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
            //_context.Entry(Cathegory).State = EntityState.Modified;
            _unitOfWork.CategoryRepository.Update(Cathegory);
            try
            {
                //await _context.SaveChangesAsync();
                _unitOfWork.Save();
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
            //return _context.CategoryTables.Any(e => e.CategoryId == id);
            return _unitOfWork.CategoryRepository.GetByID(id) != null;
        }

        // DELETE: api/Cathegory/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCathegory(int id)
        {
            var cat = _unitOfWork.CategoryRepository.GetByID(id);
            if (cat == null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Delete(cat);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}