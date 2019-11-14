using System;
using webapi.core.DTOs;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;
using AutoMapper;

namespace webapi.Controllers {
  [Authorize]
  [Route ("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrdersController (IUnitOfWork unitOfWork, IMapper mapper) {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }
    
    // GET: api/orders
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet]
    public ActionResult GetOrders ([FromQuery] Pagination pagination, [FromQuery] SearchOrder search) {
      _unitOfWork.Customers.GetAll();
      var orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(_unitOfWork.Orders.GetAll ());
      
      // Search by Id:
      if (search.Id != "") {
        orders = orders.Where(o =>
          o.Id.Contains(search.Id));
      }

      // Search by TicketCount:
      if (search.TicketCount != null) {
        orders = orders.Where(o =>
          o.TicketCount == search.TicketCount);
      }

      // Search by TotalPrice:
      if (search.TotalPrice != null) {
        orders = orders.Where(o =>
          o.TotalPrice == search.TotalPrice);
      }

      // Search by Status:
      if (search.Status != null) {
        orders = orders.Where(o =>
          o.Status == search.Status);
      }

      // Search by CreateAt:
      if (search.DateFrom != "" && search.DateTo != "") {
          orders = orders.Where(o =>
            Convert.ToDateTime(o.CreateAt.ToShortDateString()) >= Convert.ToDateTime(search.DateFrom) && 
            Convert.ToDateTime(o.CreateAt.ToShortDateString()) <= Convert.ToDateTime(search.DateTo));
      } else if (search.DateFrom != "" && search.DateTo == "") {
          orders = orders.Where(o =>
            Convert.ToDateTime(o.CreateAt.ToShortDateString()) >= Convert.ToDateTime(search.DateFrom));
      } else if (search.DateFrom == "" && search.DateTo != "") {
          orders = orders.Where(o =>
            Convert.ToDateTime(o.CreateAt.ToShortDateString()) <= Convert.ToDateTime(search.DateTo));
      }

      // Search by DateId:
      if (search.DateCreateFrom != "" && search.DateCreateTo != "") {
        var dates = _unitOfWork.Dates.GetAll();
        orders = (from o in orders
                  from d in dates
                  where o.DateId == d.Id && 
                    d.DepartureDate >= Convert.ToDateTime(search.DateCreateFrom) &&
                    d.DepartureDate <= Convert.ToDateTime(search.DateCreateTo)
                  select o);
      } else if (search.DateCreateFrom != "" && search.DateCreateTo == "") {
        var dates = _unitOfWork.Dates.GetAll();
        orders = (from o in orders
                  from d in dates
                  where o.DateId == d.Id && 
                    d.DepartureDate >= Convert.ToDateTime(search.DateCreateFrom)
                  select o);
      } else if (search.DateCreateFrom == "" && search.DateCreateTo != "") {
        var dates = _unitOfWork.Dates.GetAll();
        orders = (from o in orders
                  from d in dates
                  where o.DateId == d.Id && 
                    d.DepartureDate <= Convert.ToDateTime(search.DateCreateTo)
                  select o);
      }

      // Search by CustomerId:
      if (search.CustomerId != "") {
        orders = orders.Where(o =>
          o.CustomerId.Equals(search.CustomerId));
      }

      // Search by UserId:
      if (search.UserId != null) {
        orders = orders.Where(o =>
          o.UserId == search.UserId);
      }

      // Sort Asc:
      if (search.sortAsc != "") {
        orders = orders.OrderBy(o =>
          o.GetType().GetProperty(search.sortAsc).GetValue(o));
      }

      // Sort Desc:
      if (search.sortDesc != "") {
        orders = orders.OrderByDescending(o =>
          o.GetType().GetProperty(search.sortDesc).GetValue(o));
      }

      return Ok (PaginatedList<OrderDTO>.Create(orders, pagination.current, pagination.pageSize));
    }

    // GET: api/orders/id
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet ("{id}")]
    public ActionResult GetOrder (string id) {
      _unitOfWork.Customers.GetAll();
      _unitOfWork.Luggages.GetAll();
      _unitOfWork.TicketCategories.GetAll();
      _unitOfWork.Airports.GetAll();
      _unitOfWork.Airlines.GetAll();
      _unitOfWork.Flights.GetAll();
      _unitOfWork.Dates.GetAll();

      var order = _mapper.Map<Order, OrderDTO>(_unitOfWork.Orders.GetBy (id));
      var tickets = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDTO>>(_unitOfWork.Tickets.Find(t => t.OrderId.Equals(order.Id)));

      if (order == null) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      order.Tickets = (ICollection<TicketDTO>)tickets;

      return Ok (new { success = true, data = order });
    }

    // PUT: api/orders/id/accept
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/accept")]
    public ActionResult AcceptOrder (string id) {
      var currentUserId = int.Parse (User.Identity.Name);
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }
      
      var customer = _unitOfWork.Customers.GetBy(order.CustomerId);
      var dateFlights = _unitOfWork.DateFlights.GetAll();
      var tickets = _unitOfWork.Tickets.GetAll();
      var dateFlight = (
        from df in dateFlights
        from t in tickets
        where t.OrderId == id && order.DateId == df.DateId && t.FlightId == df.FlightId
        select df
      );

      foreach (var df in dateFlight) {
        if (df.SeatsLeft > 0) {
          df.SeatsLeft--;
          if (df.SeatsLeft == 0) {
            df.Status = 0; // Sold out
          }
        }
      }

      order.Status = 1; // Confirm
      order.UserId = currentUserId; // Get User do this
      customer.BookingCount++;

      _unitOfWork.Complete();

      return Ok (new { success = true, data = order, message = "Xác nhận hóa đơn thành công." });
    }
  
    // PUT: api/orders/id/refuse
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/refuse")]
    public ActionResult RefuseOrder (string id) {
      var currentUserId = int.Parse (User.Identity.Name);
      var order = _unitOfWork.Orders.GetBy (id);

      if (order == null) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      order.Status = 2; // Unconfirm
      order.UserId = currentUserId; // Get User do this
      _unitOfWork.Complete();

      return Ok (new { success = true, data = order, message = "Từ chối hóa đơn thành công." });
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
        DateId = values.DateId,
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

          // Ticket Price
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

      return Ok (new { success = true, message = "Thêm thành công.", data = tickets });
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