using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopWebApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using InternetShopWebApp.Repository;
using InternetShopWebApp.Services;

namespace InternetShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServices _orderService;

        public OrderController(OrderServices newOrderService)
        {
            _orderService = newOrderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllOrder()
        {
            return _orderService.GetAllOrderService();
        }

        // GET: api/Status
        [HttpGet("GetAllStatuses")]
        public async Task<ActionResult<IEnumerable<StatusTable>>> GetAllStatus()
        {
            return _orderService.GetAllStatusServices();
        }


        // GET: api/Orders/5
        [HttpGet("GetStatus/{id}")]
        public async Task<ActionResult<StatusTable>> GetStatusByID(int id)
        {
            StatusTable status = _orderService.GetStatusByIDService(id);
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
            return _orderService.GetAllTreatmentOrderService();
        }


        // GET: api/Status
        [HttpGet("GetAllPreparedOrder")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllPreparedOrder()
        {
            return _orderService.GetAllPreparedOrderService();
        }


        // GET: api/Status
        [HttpGet("GetAllPreparedUserOrder/{clientid}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllPreparedUserOrder(int clientid)
        {
            return _orderService.GetAllPreparedUserOrderService(clientid);
        }


        // GET: api/Status
        [HttpGet("GetAllUserOrder/{clientid}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllUserOrder(int clientid)
        {
            return _orderService.GetAllUserOrderService(clientid);
        }

        // GET: api/Status
        [HttpGet("GetAllPaidOrder")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllPaidOrder()
        {
            return _orderService.GetAllPaidOrderService();
        }


        // GET: api/Status
        [HttpGet("GetAllCanceledOrder")]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllCanceledOrder()
        {
            return _orderService.GetAllCanceledOrderService();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderTable>> GetOrder(int id)
        {
            OrderTable blog = _orderService.GetOrderByIDService(id);
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
            _orderService.NewOrderService(Order);
            return CreatedAtAction("GetOrder", new { id = Order.OrderCode }, Order);
        }



        // PUT: api/Order/5
        [HttpPut("PrepareOrder/{id}")]
        public async Task<IActionResult> PrepareOrder(int id)
        {
            try
            {
                if (!_orderService.PrepareOrderService(id)) return NoContent();

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
            try
            {
                _orderService.PaidOrderService(id, clientid);
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
            try
            {
                _orderService.DeleteOrderService(id);
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
            try
            {
                _orderService.PutOrderService(Order);
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
            return _orderService.OrderExistService(id);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteOrder1(int id)
        {
            if (!_orderService.FullDeleteOrderService(id)) return NotFound();

            return NoContent();
        }

    }
}