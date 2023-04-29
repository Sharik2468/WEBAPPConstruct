using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace InternetShopWebApp.Controllers
{
    public class LocationController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();


        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationTable>>> GetAllLocation()
        {
            //return await _context.LocationTables.ToListAsync();
            var Location = from s in _unitOfWork.LocationRepository.Get() select s;
            return Location.ToList();
        }
        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationTable>> GetLocation(int id)
        {
            //var blog = await _context.LocationTables.FindAsync(id);
            var Location = _unitOfWork.LocationRepository.GetByID(id);
            if (Location == null)
            {
                return NotFound();
            }
            return Location;
        }

        // POST: api/Location
        [HttpPost]
        public async Task<ActionResult<LocationTable>> NewLocation(LocationTable Location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_context.LocationTables.Add(Location);
            _unitOfWork.LocationRepository.Insert(Location);
            //await _context.SaveChangesAsync();
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
                //await _context.SaveChangesAsync();
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
            //return _context.LocationTables.Any(e => e.LocationId == id);
            return _unitOfWork.LocationRepository.GetByID(id) != null;
        }

        // DELETE: api/Location/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var cat = _unitOfWork.LocationRepository.GetByID(id);
            if (cat == null)
            {
                return NotFound();
            }
            _unitOfWork.LocationRepository.Delete(cat);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}
