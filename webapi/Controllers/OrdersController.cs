using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public OrdersController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/orders
        [HttpGet]
        public ActionResult GetOrders() {
          var orders = _unitOfWork.Orders.GetAll();
          var totalCount = orders.Count<Order> ();

          return Ok (new { success = true, data = orders });
        }

        // GET: api/orders/id
        [HttpGet ("{id}")]
        public ActionResult GetOrder(string id) {
          var order = _unitOfWork.Orders.GetBy(id);

          if (order == null) {
            return NotFound (new { success = false, message = "Invalid Order" });
          }

          return Ok (new { success = true, data = order });
        }

        // PUT: api/orders/id
        [HttpPut ("{id}")]
        public ActionResult PutOrder(string id, EditOrder values) {
          var order = _unitOfWork.Orders.GetBy(id);

          if (order == null) {
            return NotFound (new { success = false, message = "Invalid Order" });
          }

          order.Status = values.Status;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = order });
        }

        // POST: api/orders
        [HttpPost]
        public ActionResult PostOrder(Order order) {
          var orderTemp = _unitOfWork.Orders.GetBy(order.Id);

          if (orderTemp != null) {
            return BadRequest (new { success = false, message = "Id already exists" });
          }

          return Ok (new { success = true, message = "Add Successfully" });
        }
    }
}