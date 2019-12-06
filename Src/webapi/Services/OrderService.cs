using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;

namespace webapi.Services
{
  public class OrderService : ControllerBase, IOrderService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<ActionResult> GetOrdersAsync(Pagination pagination, SearchOrder search)
    {
      // Mapping: Order
      var ordersSource = await _unitOfWork.Orders.GetAllAsync();
      await _unitOfWork.Customers.GetAllAsync();
      await _unitOfWork.Users.GetAllAsync();
      await _unitOfWork.Dates.GetAllAsync();
      var orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(ordersSource);

      // Search by Id:
      if (search.Id != "")
      {
        orders = orders.Where(o =>
         o.Id.Contains(search.Id));
      }

      // Search by TicketCount:
      if (search.TicketCount != null)
      {
        orders = orders.Where(o =>
         o.TicketCount == search.TicketCount);
      }

      // Search by TotalPrice:
      if (search.TotalPrice != null)
      {
        orders = orders.Where(o =>
         o.TotalPrice == search.TotalPrice);
      }

      // Search by Status:
      if (search.Status != null)
      {
        orders = orders.Where(o =>
         o.Status == search.Status);
      }

      // Search by CreateAt:
      if (search.DateFrom != "" && search.DateTo != "")
      {
        orders = orders.Where(o =>
         Convert.ToDateTime(o.CreateAt.ToShortDateString()) >= Convert.ToDateTime(search.DateFrom) &&
         Convert.ToDateTime(o.CreateAt.ToShortDateString()) <= Convert.ToDateTime(search.DateTo));
      }
      else if (search.DateFrom != "" && search.DateTo == "")
      {
        orders = orders.Where(o =>
         Convert.ToDateTime(o.CreateAt.ToShortDateString()) >= Convert.ToDateTime(search.DateFrom));
      }
      else if (search.DateFrom == "" && search.DateTo != "")
      {
        orders = orders.Where(o =>
         Convert.ToDateTime(o.CreateAt.ToShortDateString()) <= Convert.ToDateTime(search.DateTo));
      }

      // // Search by DateId:
      // if (search.DateCreateFrom != "" && search.DateCreateTo != "") {
      //   var dates = _unitOfWork.Dates.GetAll();
      //   orders = (from o in orders
      //             from d in dates
      //             where o.DateId == d.Id && 
      //               d.DepartureDate >= Convert.ToDateTime(search.DateCreateFrom) &&
      //               d.DepartureDate <= Convert.ToDateTime(search.DateCreateTo)
      //             select o);
      // } else if (search.DateCreateFrom != "" && search.DateCreateTo == "") {
      //   var dates = _unitOfWork.Dates.GetAll();
      //   orders = (from o in orders
      //             from d in dates
      //             where o.DateId == d.Id && 
      //               d.DepartureDate >= Convert.ToDateTime(search.DateCreateFrom)
      //             select o);
      // } else if (search.DateCreateFrom == "" && search.DateCreateTo != "") {
      //   var dates = _unitOfWork.Dates.GetAll();
      //   orders = (from o in orders
      //             from d in dates
      //             where o.DateId == d.Id && 
      //               d.DepartureDate <= Convert.ToDateTime(search.DateCreateTo)
      //             select o);
      // }

      // Search by CustomerId:
      if (search.CustomerId != "")
      {
        orders = orders.Where(o =>
         o.CustomerId.Equals(search.CustomerId));
      }

      // Search by UserId:
      if (search.UserId != null)
      {
        orders = orders.Where(o =>
         o.UserId == search.UserId);
      }

      // Sort Asc:
      if (search.sortAsc != "")
      {
        orders = orders.OrderBy(o =>
         o.GetType().GetProperty(search.sortAsc).GetValue(o));
      }

      // Sort Desc:
      if (search.sortDesc != "")
      {
        orders = orders.OrderByDescending(o =>
         o.GetType().GetProperty(search.sortDesc).GetValue(o));
      }

      return Ok(PaginatedList<OrderDTO>.Create(orders, pagination.current, pagination.pageSize));
    }

    public async Task<ActionResult> GetOrderAsync(string id)
    {
      // Mapping: Order
      var orderSource = await _unitOfWork.Orders.GetByAsync(id);
      await _unitOfWork.Customers.GetAllAsync();
      await _unitOfWork.Luggages.GetAllAsync();
      await _unitOfWork.TicketCategories.GetAllAsync();
      await _unitOfWork.Airports.GetAllAsync();
      await _unitOfWork.Airlines.GetAllAsync();
      await _unitOfWork.Flights.GetAllAsync();
      await _unitOfWork.Dates.GetAllAsync();
      await _unitOfWork.Users.GetAllAsync();

      var order = _mapper.Map<Order, OrderDTO>(orderSource);

      // Check order exists
      if (order == null)
      {
        return NotFound(new { Id = "Mã hóa đơn không tồn tại." });
      }

      // Mapping để lấy thông tin
      var ticketsSource = await _unitOfWork.Orders.GetTicketsByIdAsync(order.Id);
      var tickets = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDTO>>(ticketsSource);

      order.Tickets = (ICollection<TicketDTO>)tickets;

      return Ok(new { success = true, data = order });
    }

    public async Task<ActionResult> AcceptOrderAsync(string id, int UserId)
    {
      // Check order exists
      var order = await _unitOfWork.Orders.GetByAsync(id);

      if (order == null)
      {
        return NotFound(new { Id = "Mã hóa đơn không tồn tại." });
      }

      var customer = await _unitOfWork.Customers.GetByAsync(order.CustomerId);
      var dateFlights = await _unitOfWork.DateFlights.GetAllAsync();
      var tickets = await _unitOfWork.Tickets.GetAllAsync();

      // Departure Date Flight:
      var dateFlight = (
        from df in dateFlights from t in tickets where t.OrderId == id && t.DateId == df.DateId && t.FlightId == df.FlightId select df
      );

      foreach (var df in dateFlight)
      {
        if (df.SeatsLeft > 0)
        {
          df.SeatsLeft--;
          if (df.SeatsLeft == 0)
          {
            df.Status = 0; // Sold out
          }
        }
      }

      order.Status = 1; // Confirm
      order.UserId = UserId;
      customer.BookingCount++;

      await _unitOfWork.CompleteAsync();

      return Ok(new { success = true, data = order, message = "Xác nhận hóa đơn thành công." });
    }

    public async Task<ActionResult> RefuseOrderAsync(string id, int UserId)
    {
      var order = await _unitOfWork.Orders.GetByAsync(id);

      if (order == null)
      {
        return NotFound(new { Id = "Mã hóa đơn không tồn tại." });
      }

      order.Status = 2; // Unconfirm
      order.UserId = UserId;
      await _unitOfWork.CompleteAsync();

      return Ok(new { success = true, data = order, message = "Từ chối hóa đơn thành công." });
    }

    public async Task<ActionResult> AddOrderAsync(AddOrder values)
    {
      // Check Customer existence
      var customer = await _unitOfWork.Customers.GetByAsync(values.CustomerId);

      if (customer == null) // Nếu không tồn tại khách hàng này trong db
      {
        // Add Customer
        var saveCustomerDTO = new SaveCustomerDTO
        {
          Id = values.CustomerId,
          FullName = values.FullName,
          Phone = values.Phone,
        };

        // Mapping: SaveCustomer
        customer = _mapper.Map<SaveCustomerDTO, Customer>(saveCustomerDTO);
        await _unitOfWork.Customers.AddAsync(customer);
      } else {
        customer.FullName = values.FullName;
        customer.Phone = values.Phone;
      }

      // Lấy DepartureDate Id
      var departureDateIdAsync = await _unitOfWork.Dates.FindAsync(d =>
       d.DepartureDate == Convert.ToDateTime(values.DepartureDateName));

      var departureDateId = departureDateIdAsync
        .Select(d => d.Id).SingleOrDefault();

      // Add Order
      SaveOrderDTO saveOrderDTO = new SaveOrderDTO
      {
        Id = await this.autoOrderIdAsync(),
        TicketCount = values.TicketCount,
        TotalPrice = values.TotalPrice,
        CreateAt = DateTime.Now, // Lấy thời điểm đặt vé
        Status = 0,
        CustomerId = values.CustomerId,
        DepartureDateId = departureDateId,
        ReturnDateId = null,
        UserId = null
      };

      // Lấy ReturnDate Id
      if (values.ReturnDateName != "")
      {
        var returnDateIdAsync = await _unitOfWork.Dates.FindAsync(d =>
         d.DepartureDate == Convert.ToDateTime(values.ReturnDateName));

        saveOrderDTO.ReturnDateId = returnDateIdAsync
          .Select(d => d.Id).SingleOrDefault();
      }

      // Mapping: SaveOrder
      var order = _mapper.Map<SaveOrderDTO, Order>(saveOrderDTO);
      await _unitOfWork.Orders.AddAsync(order);

      IList<Ticket> tickets = new List<Ticket>(); // Dòng này để kiểm tra dữ liệu tạm thời (xóa sau)

      // Add Tickets
      for (int i = 0; i < values.FlightIds.Count; i++)
      {
        for (int j = 0; j < values.Passengers.Count; j++)
        {
          // Luggage Price
          var luggageAsync = await _unitOfWork.Luggages.FindAsync(l =>
           l.Id == values.Passengers.ElementAt(j).LuggageIds.ElementAt(i));

          decimal luggagePrice = Convert.ToDecimal(luggageAsync.SingleOrDefault().Price);

          // Ticket Price
          var ticketAsync = await _unitOfWork.FlightTicketCategories.FindAsync(ft =>
           ft.TicketCategoryId == values.Passengers.ElementAt(j).TicketCategoryId &&
           ft.FlightId == values.FlightIds.ElementAt(i));

          decimal ticketPrice = Convert.ToDecimal(ticketAsync.SingleOrDefault().Price);

          // Lấy ngày bay, i = 0 là của chiều đi, i = 1 là của chiều về
          int dateId = 0;
          if (i == 0)
          {
            dateId = order.DepartureDateId;
          }
          else
          {
            dateId = (int)order.ReturnDateId;
          }

          var saveTicketDTO = new SaveTicketDTO
          {
            Id = await this.autoTicketIdAsync(),
            PassengerName = values.Passengers.ElementAt(j).PassengerName,
            PassengerGender = values.Passengers.ElementAt(j).PassengerGender,
            LuggageId = values.Passengers.ElementAt(j).LuggageIds.ElementAt(i),
            FlightId = values.FlightIds.ElementAt(i),
            OrderId = order.Id,
            DateId = dateId,
            TicketCategoryId = values.Passengers.ElementAt(j).TicketCategoryId,
            Price = ticketPrice + luggagePrice
          };

          // Mapping: SaveTicket
          var ticket = _mapper.Map<SaveTicketDTO, Ticket>(saveTicketDTO);

          tickets.Add(ticket); // Xóa sau
          await _unitOfWork.Tickets.AddAsync(ticket);
          await _unitOfWork.CompleteAsync();
        }
      }

      return Ok(new { success = true, message = "Thêm thành công.", data = tickets });
    }

    // Hàm phát sinh:
    // 0. Kiểm tra có bao nhiêu chữ số
    private int totalDigits(int number)
    {
      int sum = 0;

      while (number != 0)
      {
        sum++;
        number = number / 10;
      }

      return sum;
    }

    // 1. Tự động sinh OrderId
    private async Task<string> autoOrderIdAsync()
    {
      string orderId = "";
      var orders = await _unitOfWork.Orders.GetAllAsync();

      if (orders.Any())
      {
        int orderIdNum = Int32.Parse(orders.Last().Id) + 1; // Lấy mã đơn hàng cũ chuyển qua kiểu int và + 1
        int totalDigits = this.totalDigits(orderIdNum); // Tính tổng chữ số của mã mới lấy được
        switch (totalDigits)
        {
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
      }
      else
      {
        orderId = "0001";
      }

      return orderId;
    }

    // 2. Tự động sinh TicketId
    private async Task<string> autoTicketIdAsync()
    {
      string ticketId = "";
      var tickets = await _unitOfWork.Tickets.GetAllAsync();

      if (tickets.Any())
      {
        int ticketIdNum = Int32.Parse(tickets.Last().Id) + 1; // Lấy mã đơn hàng cũ chuyển qua kiểu int và + 1
        int totalDigits = this.totalDigits(ticketIdNum); // Tính tổng chữ số của mã mới lấy được
        switch (totalDigits)
        {
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
      }
      else
      {
        ticketId = "0001";
      }

      return ticketId;
    }
  }
}