using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers {
  [Route ("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController (IUnitOfWork unitOfWork) {
      _unitOfWork = unitOfWork;
    }

    // GET: api/orders
    [HttpGet]
    public ActionResult GetOrders () {
      var orders = _unitOfWork.Orders.GetAll ();
      var totalCount = orders.Count<Order> ();

      return Ok (new { success = true, data = orders, totalCount = totalCount });
    }

    // GET: api/orders/id
    [HttpGet ("{id}")]
    public ActionResult GetOrder (string id) {
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { success = false, message = "Invalid Order" });
      }

      return Ok (new { success = true, data = order });
    }

    // PUT: api/orders/id
    [HttpPut ("{id}")]
    public ActionResult PutOrder (string id, EditOrder values) {
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { success = false, message = "Invalid Order" });
      }

      order.Status = values.Status;
      _unitOfWork.Complete ();

      return Ok (new { success = true, data = order });
    }

    // POST: api/orders
    [HttpPost]
    public ActionResult PostOrder (AddOrder values) {
      // Check Customer existence
      var customer = _unitOfWork.Customers.GetBy (values.CustomerId);
      if (values.FlightIds.Count () <= 0) {
        return Ok ();
      }

      if (customer != null) {
        customer.BookingCount++; // Tăng số lần đặt vé nếu tồn tại ID
      } else {
        // Add Customer
        customer = new Customer {
          Id = values.CustomerId,
          FullName = values.FullName,
          Phone = values.Phone,
          BookingCount = 1
        };

        _unitOfWork.Customers.Add (customer);
      }

      // Add Order
      Order order = new Order {
        Id = this.autoOrderId (),
        TicketCount = values.TicketCount,
        TotalPrice = values.TotalPrice,
        CreateAt = DateTime.Now, // Lấy thời điểm đặt vé
        Status = 0,
        CustomerId = values.CustomerId
      };
      _unitOfWork.Orders.Add (order);

      for (int i = 0; i < values.FlightIds.Count (); i++) {

      // Add tickets
        for (int j = 0; j < values.TicketCount; i++) {
          _unitOfWork.Tickets.Add (new Ticket {
            Id = this.autoTicketId (),
              // Price = 
          });
        }
      }



      _unitOfWork.Complete ();

      return Ok (new { success = true, message = "Add Successfully" });
    }

    // Hàm phát sinh:
    // 0. Kiểm tra có bao nhiêu chữ số
    private int totalDigits (int number) {
      int sum = 0;

      while (number != 0) {
        sum++;
        number = number / 10;
      }

      return sum;
    }

    // 1. Tự động sinh OrderId
    private string autoOrderId () {
      string orderId = "";
      var orders = _unitOfWork.Orders.GetAll ();

      if (orders.Any ()) {
        int orderIdNum = Int32.Parse (orders.Last ().Id) + 1; // Lấy mã đơn hàng cũ chuyển qua kiểu int và + 1
        int totalDigits = this.totalDigits (orderIdNum); // Tính tổng chữ số của mã mới lấy được
        switch (totalDigits) {
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
    // 1. Tự động sinh OrderId
    private string autoTicketId () {
      string orderId = "";
      var orders = _unitOfWork.Tickets.GetAll ();

      if (orders.Any ()) {
        int orderIdNum = Int32.Parse (orders.Last ().Id) + 1; // Lấy mã đơn hàng cũ chuyển qua kiểu int và + 1
        int totalDigits = this.totalDigits (orderIdNum); // Tính tổng chữ số của mã mới lấy được
        switch (totalDigits) {
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