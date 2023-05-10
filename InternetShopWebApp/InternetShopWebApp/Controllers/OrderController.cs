using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using InternetShopWebApp.Repository;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

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

        // GET: api/Status
        [HttpGet("GetAllStatuses")]
        public async Task<ActionResult<IEnumerable<StatusTable>>> GetAllStatus()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.StatusOrderRepository.Get().ToList();
        }

        // GET: api/Orders/5
        [HttpGet("GetStatus/{id}")]
        public async Task<ActionResult<StatusTable>> GetStatusByID(int id)
        {
            var status = _unitOfWork.StatusOrderRepository.GetByID(id);
            if (status == null)
            {
                return NotFound();
            }
            return status;
        }

        // GET: api/Status
        [HttpGet("GetAllTreatmentOrder")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllTreatmentOrder()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 1).ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllPreparedOrder")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllPreparedOrder()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 2).ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllPreparedUserOrder/{clientid}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllPreparedUserOrder(int clientid)
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables")
                .Where(a => a.StatusCode == 2 && a.ClientCode == clientid).ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllUserOrder/{clientid}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllUserOrder(int clientid)
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables")
                .Where(a => a.ClientCode == clientid
                && (a.StatusCode == 3 || a.StatusCode == 4)).ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllPaidOrder")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllPaidOrder()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 3).ToList();
        }

        // GET: api/Status
        [HttpGet("GetAllCanceledOrder")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllCanceledOrder()
        {
            //return _unitOfWork.OrderRepository.Get().ToList();
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 4).ToList();
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
        [HttpPut("PrepareOrder/{id}")]
        public async Task<IActionResult> PrepareOrder(int id)
        {
            int currentStatus = 2;
            //_context.Entry(Order).State = EntityState.Modified;
            var currentOrder = _unitOfWork.OrderRepository.GetByID(id);

            bool isInsert = false;
            OrderTable newOrder = new OrderTable();
            newOrder.OrderFullfillment = currentOrder.OrderFullfillment;
            newOrder.OrderDate = currentOrder.OrderDate;
            newOrder.ClientCode = currentOrder.ClientCode;
            newOrder.SalesmanCode = currentOrder.SalesmanCode;
            newOrder.StatusCode = currentOrder.StatusCode;
            var table = _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").LastOrDefault();
            newOrder.OrderCode = table == null ? 0 :
                                      table.OrderCode + 1;

            var currentOrderRec = _unitOfWork.OrderRepository
                .Get(includeProperties: "OrderItemTables")
                .Where(a => a.OrderCode == id)
                .LastOrDefault();

            var currentOrderItemsRec = currentOrderRec.OrderItemTables.Where(a => a.StatusOrderItemTableId == 2);

            if (currentOrderRec.OrderItemTables.Count() == currentOrderItemsRec.ToList().Count())
                return NoContent();

            List<OrderItemTable> OrderTableForRepalce = new List<OrderItemTable>();
            foreach (var item in currentOrderItemsRec)
            {
                var current = _unitOfWork.OrderItemRepository.GetByID(item.OrderItemCode);
                current.OrderCode = newOrder.OrderCode;
                OrderTableForRepalce.Add(current);
                isInsert = true;
            }

            if (isInsert)
            {
                _unitOfWork.OrderRepository.Insert(newOrder);
                foreach (var item in OrderTableForRepalce)
                {
                    item.OrderCodeNavigation = null;
                    _unitOfWork.OrderItemRepository.Update(item);
                }
                _unitOfWork.Save();
            }

            currentOrder.StatusCode = currentStatus;
            _unitOfWork.OrderRepository.Update(currentOrder);
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
            catch (Exception e)
            {
                return NotFound();
            }
            return NoContent();
        }

        // PUT: api/Order/5
        [HttpPut("PaidOrder/{id}/{clientid}")]
        public async Task<IActionResult> PaidOrder(int id, int clientid)
        {
            int currentStatus = 3;
            //_context.Entry(Order).State = EntityState.Modified;
            var currentOrder = _unitOfWork.OrderRepository.GetByID(id);

            var currentOrderRec = _unitOfWork.OrderRepository
                .Get(includeProperties: "OrderItemTables")
                .Where(a => a.OrderCode == id)
                .LastOrDefault();

            ProductTable product;
            foreach (var item in currentOrderRec.OrderItemTables)
            {
                product = _unitOfWork.ProductRepository.GetByID(item.ProductCode);
                product.NumberInStock -= item.AmountOrderItem;
                _unitOfWork.ProductRepository.Update(product);
            }

            currentOrder.SalesmanCode = clientid;
            currentOrder.OrderFullfillment = DateTime.Now;
            currentOrder.StatusCode = currentStatus;
            _unitOfWork.OrderRepository.Update(currentOrder);
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
            catch (Exception e)
            {
                return NotFound();
            }
            return NoContent();
        }

        // PUT: api/Order/5
        [HttpPut("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            int currentStatus = 4;
            //_context.Entry(Order).State = EntityState.Modified;
            var currentOrder = _unitOfWork.OrderRepository.GetByID(id);

            currentOrder.StatusCode = currentStatus;
            _unitOfWork.OrderRepository.Update(currentOrder);
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
            catch (Exception e)
            {
                return NotFound();
            }
            return NoContent();
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
        public async Task<IActionResult> DeleteOrder1(int id)
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