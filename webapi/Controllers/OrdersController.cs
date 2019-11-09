using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers {
  [Authorize]
  [Route ("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController (IUnitOfWork unitOfWork) {
      _unitOfWork = unitOfWork;
    }
    
    // GET: api/orders
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet]
    public ActionResult GetOrders ([FromQuery] Pagination pagination) {
      var orders = _unitOfWork.Orders.GetAll ();
      
      return Ok (PaginatedList<Order>.Create(orders, pagination.current, pagination.pageSize));
    }

    // GET: api/orders/id
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet ("{id}")]
    public ActionResult GetOrder (string id) {
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { success = false, message = "Invalid Order" });
      }

      return Ok (new { success = true, data = order });
    }

    // PUT: api/orders/id/accept
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/accept")]
    public ActionResult AcceptOrder (string id) {
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { success = false, message = "Invalid Order" });
      }
      
      var customer = _unitOfWork.Customers.GetBy(order.CustomerId);
      var flights = _unitOfWork.Flights.GetAll();
      var tickets = _unitOfWork.Tickets.GetAll();
      var flight = (
        from f in flights
        from t in tickets
        where f.Id == t.FlightId && t.OrderId == id
        select f
      );

      foreach (var f in flight) {
        if (f.SeatsLeft > 0) {
          f.SeatsLeft--;
          if (f.SeatsLeft == 0) {
            var dateFlight = _unitOfWork.DateFlights.GetBy(f.Id);
            dateFlight.Status = 0;
          }
        }
      }

      order.Status = 1;
      customer.BookingCount++;

      _unitOfWork.Complete();

      return Ok (new { success = true, data = order });
    }
  
    // PUT: api/orders/id/refuse
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/refuse")]
    public ActionResult RefuseOrder (string id) {
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { success = false, message = "Invalid Order" });
      }

      order.Status = 2;
      _unitOfWork.Complete();

      return Ok (new { success = true, data = order });
    }

    // POST: api/orders
    [AllowAnonymous]
    [HttpPost]
    public ActionResult PostOrder (AddOrder values) {
      // Check Customer existence
      var customer = _unitOfWork.Customers.GetBy (values.CustomerId);

      if (customer == null) // Nếu không tồn tại khách hàng này trong db
      {
        // Add Customer
        customer = new Customer {
          Id = values.CustomerId,
          FullName = values.FullName,
          Phone = values.Phone,
          BookingCount = 0
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