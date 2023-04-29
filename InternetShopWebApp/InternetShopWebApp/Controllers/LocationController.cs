using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using InternetShopWebApp.Repository;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class LocationController : ControllerBase
    {
        //private readonly Context.InternetShopContext _context;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        //public LocationController(Context.InternetShopContext context)
        //{
        //    _context = context;
        //    //if (!_context.Location.Any())
        //    //{
        //    //    _context.Location.Add(new Location
        //    //    {
        //    //        Location_Code = 1,
        //    //        Name = "Nokia",
        //    //        CategoryID = 1,
        //    //        Desctription = "asdasd"
        //    //    });
        //    //    _context.SaveChanges();
        //    //}
        //}

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationTable>>> GetAllLocation()
        {
            return _unitOfWork.LocationRepository.Get().ToList();
        }
        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationTable>> GetLocation(int id)
        {
            var blog = _unitOfWork.LocationRepository.GetByID(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Location
        [HttpPost]
        public async Task<ActionResult<LocationTable>> NewLocation(LocationTable Location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.LocationRepository.Insert(Location);
            _unitOfWork.Save();
            return CreatedAtAction("GetLocation", new { id = Location.LocationCode }, Location);
        }

        // PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, LocationTable Location)
        {
            if (id != Location.LocationCode)
            {
                return BadRequest();
            }
            //_context.Entry(Location).State = EntityState.Modified;
            _unitOfWork.LocationRepository.Update(Location);
            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        private bool LocationExists(int id)
        {
            return _unitOfWork.LocationRepository.GetByID(id) != null;
        }

        // DELETE: api/Location/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            //var blog = await _context.LocationTables.FindAsync(id);
            var location = _unitOfWork.LocationRepository.GetByID(id);
            if (location == null)
            {
                return NotFound();
            }
            _unitOfWork.LocationRepository.Delete(location);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}