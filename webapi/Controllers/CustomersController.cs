using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomersController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/customers/
        [HttpGet]
        public ActionResult GetCustomers() {
          var customers = _unitOfWork.Customers.GetAll();
          var totalCount = customers.Count<Customer> ();

          return Ok (new { success = true, data = customers });
        }

        // GET: api/customers/id
        [HttpGet ("{id}")]
        public ActionResult GetCustomer(string id) {
          var customer = _unitOfWork.Customers.GetBy(id);
          
          if (customer == null) {
            return NotFound (new { success = false, message = "Invalid Customer" });
          }

          return Ok (new { success = true, data = customer });
        }

        // PUT: api/customers/id
        [HttpPut ("{id}")]
        public ActionResult PutCustomer(string id, EditCustomer values) {
          var customer = _unitOfWork.Customers.GetBy(id);

          if (customer == null) {
            return NotFound (new { success = false, message = "Invalid Customer" });
          }

          if (values.FullName != "") {
            customer.FullName = values.FullName;
          }

          if (values.Phone != "") {
            customer.Phone = values.Phone;
          }

          _unitOfWork.Complete();

          return Ok (new { success = true, data = customer });
        }

        // POST: api/customers
        [HttpPost]
        public ActionResult PostCustomer(Customer customer) {
          var customerTemp = _unitOfWork.Customers.GetBy(customer.Id);

          if (customerTemp != null) {
            customerTemp.BookingCount++; // Tăng số lần đặt hàng nếu trùng Id
          } else {
            _unitOfWork.Customers.Add(customer);
          }
          
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Add Successfully" });
        }

        // DELETE: api/customers/id
        [HttpDelete ("{id}")]
        public ActionResult DeleteCustomer(string id) {
          var customer = _unitOfWork.Customers.GetBy(id);

          if (customer == null) {
            return NotFound (new { success = false, message = "Invalid Customer" });
          }

          _unitOfWork.Customers.Remove(customer);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Delete Successfully" });
        }
    }
}