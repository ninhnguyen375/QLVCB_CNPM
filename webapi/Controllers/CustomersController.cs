using System;
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

          return Ok (new { success = true, data = customers, auto = this.autoOrderId() });
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
        public ActionResult PostCustomer(AddCustomer values) {
          var customer = _unitOfWork.Customers.GetBy(values.CustomerId);

          if (customer != null) {
            customer.BookingCount++;
          } else {
            // Add Customer
            customer = new Customer { 
              Id = values.CustomerId,
              FullName = values.FullName,
              Phone = values.Phone,
              BookingCount = values.BookingCount
            };

            _unitOfWork.Customers.Add(customer);
          }

          // Add Order
          Order order = new Order {
            Id = this.autoOrderId(),
            TicketCount = values.TicketCount,
            TotalPrice = values.TotalPrice,
            CreateAt = values.CreateAt,
            Status = values.Status,
            CustomerId = values.CustomerId,
          };
    
          _unitOfWork.Orders.Add(order);
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
        
        // Hàm phát sinh:
        // 0. Kiểm tra có bao nhiêu chữ số
        private int totalDigits(int number) {
          int sum = 0;

          while (number != 0) {
            sum++;
            number = number / 10;
          }

          return sum;
        }

        // 1. Tự động sinh OrderId
        private string autoOrderId() {
          string orderId = "";
          var orders = _unitOfWork.Orders.GetAll();

          if (orders.Any()) {
            int orderIdNum = Int32.Parse(orders.Last().Id) + 1; // Lấy mã đơn hàng cũ chuyển qua kiểu int và + 1
            int totalDigits = this.totalDigits(orderIdNum); // Tính tổng chữ số của mã mới lấy được
            switch(totalDigits) {
              case 1:
                orderId = "000" + orderIdNum;
                break;
              case 2:
                orderId = "00" + orderIdNum;
                break;
              case 3:
                orderId = "0" + orderIdNum;
                break;
              default:
                orderId += orderIdNum;
                break;
            }
          } else {
            orderId = "0001";
          }

          return orderId;
        }
    }
}