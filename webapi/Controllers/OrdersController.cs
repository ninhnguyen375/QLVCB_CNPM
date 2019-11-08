using System;
using System.Collections.Generic;
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
        CustomerId = values.CustomerId,
        UserId = null
      };
      _unitOfWork.Orders.Add (order);

      IList<Ticket> tickets = new List<Ticket>(); // Dòng này để kiểm tra dữ liệu tạm thời (xóa sau)

      for (int i = 0; i < values.FlightIds.Count; i++) {
        for (int j = 0; j < values.Passengers.Count; j++) {
          // Luggage Price
          decimal luggagePrice = Convert.ToDecimal(
            _unitOfWork.Luggages.Find(l =>
              l.Id == values.Passengers.ElementAt(j).LuggageIds.ElementAt(i)
            ).ElementAt(0).Price
          );

          // TicketPrice
          decimal ticketPrice = Convert.ToDecimal(
            _unitOfWork.FlightTicketCategories.Find(ft => 
              ft.TicketCategoryId == values.Passengers.ElementAt(j).TicketCategoryId && 
              ft.FlightId == values.FlightIds.ElementAt(i)
            ).ElementAt(0).Price
          );

          var ticket = new Ticket {
            Id = this.autoTicketId(),
            PassengerName = values.Passengers.ElementAt(j).PassengerName,
            PassengerGender = values.Passengers.ElementAt(j).PassengerGender,
            LuggageId = values.Passengers.ElementAt(j).LuggageIds.ElementAt(i),
            FlightId = values.FlightIds.ElementAt(i),
            OrderId = order.Id,
            TicketCategoryId = values.Passengers.ElementAt(j).TicketCategoryId,
            Price = ticketPrice + luggagePrice
          };

          tickets.Add(ticket); // Xóa sau
          _unitOfWork.Tickets.Add (ticket);
          _unitOfWork.Complete ();
        }
      }

      return Ok (new { success = true, message = "Add Successfully", data = tickets });
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
      string ticketId = "";
      var tickets = _unitOfWork.Tickets.GetAll ();

      if (tickets.Any ()) {
        int ticketIdNum = Int32.Parse (tickets.Last ().Id) + 1; // Lấy mã đơn hàng cũ chuyển qua kiểu int và + 1
        int totalDigits = this.totalDigits (ticketIdNum); // Tính tổng chữ số của mã mới lấy được
        switch (totalDigits) {
          case 1:
            ticketId = "000" + ticketIdNum;
            break;
          case 2:
            ticketId = "00" + ticketIdNum;
            break;
          case 3:
            ticketId = "0" + ticketIdNum;
            break;
          default:
            ticketId += ticketIdNum;
            break;
        }
      } else {
        ticketId = "0001";
      }

      return ticketId;
    }
  }
}