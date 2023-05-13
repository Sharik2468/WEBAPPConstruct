using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using InternetShopWebApp.Repository;
using InternetShopWebApp.Services;
using Microsoft.AspNetCore.Identity;
using ASPNetCoreApp.Controllers;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderServices _orderService;

        public OrderItemController(OrderServices newOrderService)
        {
            _orderService = newOrderService;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllOrderItem()
        {
            return _orderService.GetAllOrderItemService();
        }


        // GET: api/Status
        [HttpGet("GetAllOrderItemStatuses")]
        public async Task<ActionResult<IEnumerable<StatusOrderItemTable>>> GetAllOrderItemStatuses()
        {
            return _orderService.GetAllOrderItemStatusesService();
        }


        // GET: api/Status
        [HttpGet("GetAllInStockOrderItem")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllInStockOrderItem()
        {
            return _orderService.GetAllInStockOrderItemService();
        }

        // GET: api/Status
        [HttpGet("GetAllCanceledOrderItem")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllCanceledOrderItem()
        {
            return _orderService.GetAllCanceledOrderItemService();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetOrderItem(int id)
        {
            var orderitem = _orderService.GetAllActiveOrderItemsByClientId(id);

            if (orderitem == null)
            {
                return NotFound();
            }
            return orderitem.ToList();
        }

        // POST: api/OrderItem/1.0(Код клиента.Код товара)
        [HttpPost("{Request}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<OrderItemTable>> NewOrderItem(string Request)
        {
            string[] words = Request.Split(new char[] { '.' });
            // new char[] - массив символов-разделителей. Как меня поправили в 
            // комментариях, в данном случае достаточно написать text.Split(':')

            int first = words[0] == null ? -1 : int.Parse(words[0]);
            int second = words[1] == null ? -1 : int.Parse(words[1]);

            var result = _orderService.AddNewOrderItem(first, second);
            return CreatedAtAction("GetOrderItem", new { id = result.OrderItemCode }, result);
        }

        // GET: api/Orders/5
        [HttpGet("GetOrderItemStatus/{id}")]
        public async Task<ActionResult<StatusOrderItemTable>> GetOrderItesStatusByID(int id)
        {
            StatusOrderItemTable status = _orderService.GetOrderItemStatusByIDService(id);
            if (status == null)
            {
                return NotFound();
            }
            return status;
        }

        // PUT: api/OrderItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItemTable OrderItem)
        {
            if (id != OrderItem.OrderItemCode)
            {
                return BadRequest();
            }
            try
            {
                _orderService.PutClearOrderItemService(OrderItem);
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


        // PUT: api/OrderItem/5
        [HttpPut("PutNewStatusID/{id}/{status}")]
        public async Task<IActionResult> PutStatusIDOrderItem(int id, int status)
        {
            try
            {
                _orderService.PutStatusIDForOrderItemService(id, status);
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

        // PUT: api/OrderItem/updateamount/5/3
        [HttpPut("{id}/{amount}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> PutOrderItemUpdateAmount(int id, int amount)
        {
            bool result = _orderService.SetNewOrderItemAmount(id, amount);

            if (result == false)
            {
                DeleteOrderItem(id);
            }

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _orderService.OrderItemExistService(id);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            if (!_orderService.DeleteOrderItemService(id)) return NotFound();

            return NoContent();
        }
    }
}