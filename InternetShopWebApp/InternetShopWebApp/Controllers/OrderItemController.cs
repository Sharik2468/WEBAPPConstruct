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
    public class OrderItemController : ControllerBase
    {
        private readonly Context.ShopContext _context;
        public OrderItemController(Context.ShopContext context)
        {
            _context = context;
            if (!_context.OrderItem.Any())
            {
                _context.OrderItem.Add(new OrderItemModel
                {
                    Order_Item_Code = 1,
                    Order_Sum = 100,
                    Amount_Order_Item = 1,
                    Product_Code = 1,
                    Order_Code = 1,
                    Status_Order_Item_Table_ID = 1
                });
                _context.SaveChanges();
            }
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemModel>>> GetAllOrderItem()
        {
            return await _context.OrderItem.Include(p=>p.Products).ToListAsync();
        }
        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemModel>> GetOrderItem(int id)
        {
            var blog = await _context.OrderItem.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/OrderItem
        [HttpPost]
        public async Task<ActionResult<OrderItemModel>> NewOrderItem(OrderItemModel OrderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.OrderItem.Add(OrderItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrderItem", new { id = OrderItem.Order_Item_Code }, OrderItem);
        }

        // PUT: api/OrderItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItemModel OrderItem)
        {
            if (id != OrderItem.Order_Item_Code)
            {
                return BadRequest();
            }
            _context.Entry(OrderItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.Order_Item_Code == id);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var blog = await _context.OrderItem.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.OrderItem.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}