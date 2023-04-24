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
        private readonly Context.InternetShopContext _context;
        public OrderItemController(Context.InternetShopContext context)
        {
            _context = context;
            //if (!_context.OrderItem.Any())
            //{
            //    _context.OrderItem.Add(new OrderItemModel
            //    {
            //        Order_Item_Code = 1,
            //        Order_Sum = 100,
            //        Amount_Order_Item = 1,
            //        Product_Code = 1,
            //        Order_Code = 1,
            //        Status_Order_Item_Table_ID = 1
            //    });
            //    _context.SaveChanges();
            //}
        }

        //// GET: api/OrderItems
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderItemModel>>> GetAllOrderItem()
        //{
        //    return await _context.OrderItem.Include(p=>p.Products).ToListAsync();
        //}

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllOrderItem()
        {
            return await _context.OrderItemTables.ToListAsync();
        }
        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemTable>> GetOrderItem(int id)
        {
            var blog = await _context.OrderItemTables.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return blog;
        }

        // POST: api/OrderItem
        [HttpPost]
        public async Task<ActionResult<OrderItemTable>> NewOrderItem(OrderItemTable OrderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.OrderItemTables.Add(OrderItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrderItem", new { id = OrderItem.OrderItemCode }, OrderItem);
        }

        // PUT: api/OrderItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItemTable OrderItem)
        {
            if (id != OrderItem.OrderItemCode)
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
            return _context.OrderItemTables.Any(e => e.OrderItemCode == id);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var blog = await _context.OrderItemTables.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.OrderItemTables.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}