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
        private readonly OrderServices _orderService = new OrderServices();
        private readonly UnitOfWork _unitOfWork;

        public OrderItemController(UnitOfWork newUnitOfWork)
        {
            _unitOfWork = newUnitOfWork;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllOrderItem()
        {
            return _unitOfWork.OrderItemRepository.Get().ToList();
        }
        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetOrderItem(int id)
        {
            //var orderitem = _unitOfWork.OrderItemRepository.GetByID(id);
            var orderitem = _orderService.GetAllActiveOrderItemsByClientId(id);
            if (orderitem == null)
            {
                return NotFound();
            }
            return orderitem.ToList();
        }

        // POST: api/OrderItem
        [HttpPost]
        public async Task<ActionResult<OrderItemTable>> NewOrderItem(OrderItemTable OrderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.OrderItemRepository.Insert(OrderItem);
            _unitOfWork.Save();
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
            _unitOfWork.OrderItemRepository.Update(OrderItem);
            try
            {
                _unitOfWork.Save();
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
            return _unitOfWork.OrderItemRepository.Get().Any(e => e.OrderItemCode == id);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderitem = _unitOfWork.OrderItemRepository.GetByID(id);
            if (orderitem == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderItemRepository.Delete(orderitem);
            _unitOfWork.Save();
            return NoContent();
        }
    }
}