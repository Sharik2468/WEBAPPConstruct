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
    public class OrderController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderController(UnitOfWork newUnitOfWork)
        {
            _unitOfWork = newUnitOfWork;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllOrder()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").ToList();
        }
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderTable>> GetOrder(int id)
        {
            var blog = _unitOfWork.OrderRepository.GetByID(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<OrderTable>> NewOrder(OrderTable Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.OrderRepository.Insert(Order);
            _unitOfWork.Save();
            return CreatedAtAction("GetOrder", new { id = Order.OrderCode }, Order);
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderTable Order)
        {
            if (id != Order.OrderCode)
            {
                return BadRequest();
            }
            //_context.Entry(Order).State = EntityState.Modified;
            _unitOfWork.OrderRepository.Update(Order);
            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        private bool OrderExists(int id)
        {
            return _unitOfWork.OrderRepository.GetByID(id) != null;
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            //var blog = await _context.OrderTables.FindAsync(id);
            var Order = _unitOfWork.OrderRepository.GetByID(id);
            if (Order == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderRepository.Delete(Order);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}