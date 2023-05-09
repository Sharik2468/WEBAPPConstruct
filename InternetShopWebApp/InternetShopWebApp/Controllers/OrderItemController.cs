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
            //return _unitOfWork.OrderItemRepository.Get().ToList();
            return _unitOfWork.OrderItemRepository.Get(includeProperties: "ProductCodeNavigation,OrderCodeNavigation").ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllOrderItemStatuses")]
        public async Task<ActionResult<IEnumerable<StatusOrderItemTable>>> GetAllOrderItemStatuses()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.StatusOrderItemRepository.Get().ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllInStockOrderItem")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllInStockOrderItem()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderItemRepository.Get(includeProperties: "ProductCodeNavigation,OrderCodeNavigation").Where(a=>a.StatusOrderItemTableId==1).ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllCanceledOrderItem")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetAllCanceledOrderItem()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderItemRepository.Get(includeProperties: "ProductCodeNavigation,OrderCodeNavigation").Where(a => a.StatusOrderItemTableId == 2).ToList();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderItemTable>>> GetOrderItem(int id)
        {
            //var orderitem = _unitOfWork.OrderItemRepository.GetByID(id);
            var orderitem = _orderService.GetAllActiveOrderItemsByClientId(id);
            var products = _unitOfWork.ProductRepository.Get();

            foreach (var item in orderitem)
                item.ProductCodeNavigation = products.Where(a => a.ProductCode == item.ProductCode).FirstOrDefault();

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

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //_unitOfWork.OrderItemRepository.Insert(OrderItem);
            //_unitOfWork.Save();

            var result = _orderService.AddNewOrderItem(first, second);
            return CreatedAtAction("GetOrderItem", new { id = result.OrderItemCode }, result);
        }

        // GET: api/Orders/5
        [HttpGet("GetOrderItemStatus/{id}")]
        public async Task<ActionResult<StatusOrderItemTable>> GetOrderItesStatusByID(int id)
        {
            var status = _unitOfWork.StatusOrderItemRepository.GetByID(id);
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

        // PUT: api/OrderItem/5
        [HttpPut("PutNewStatusID/{id}/{status}")]
        public async Task<IActionResult> PutStatusIDOrderItem(int id, int status)
        {
            var OrderItem = _unitOfWork.OrderItemRepository.GetByID(id);
            OrderItem.StatusOrderItemTableId = status;
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

            //_unitOfWork.OrderItemRepository.Update();
            //try
            //{
            //    _unitOfWork.Save();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!OrderItemExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _unitOfWork.OrderItemRepository.Get().Any(e => e.OrderItemCode == id);
        }

        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var order = _orderService.GetOrderByOrderItemID(id);
            var orderitem = _unitOfWork.OrderItemRepository.GetByID(id);
            if (orderitem == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderItemRepository.Delete(orderitem);
            _unitOfWork.Save();

            if (order.OrderItemTables.Count() - 1 == 0)
            {
                order.OrderItemTables.Clear();
                _unitOfWork.OrderRepository.Delete(order);
                _unitOfWork.Save();
            }

            return NoContent();
        }
    }
}