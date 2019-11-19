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
using webapi.Interfaces;

namespace webapi.Controllers {
  [Authorize]
  [Route ("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase {
    private readonly IOrderService _service;

    public OrdersController(IOrderService orderService) {
      _service = orderService;
    }
    
    // GET: api/orders
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet]
    public ActionResult GetOrders ([FromQuery] Pagination pagination, [FromQuery] SearchOrder search) {
      var orders = _service.GetOrders(pagination, search);
      
      return Ok (PaginatedList<OrderDTO>.Create(orders, pagination.current, pagination.pageSize));
    }

    // GET: api/orders/id
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet ("{id}")]
    public ActionResult GetOrder (string id) {
      var order= _service.GetOrder(id);  

      if (order == null) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      return Ok (new { success = true, data = order });
    }

    // PUT: api/orders/id/accept
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/accept")]
    public ActionResult AcceptOrder (string id, EditOrder values) {
      var order = _service.AcceptOrder(id, values);

      if (order.Error == 1) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      return Ok (new { success = true, data = order.Data, message = "Xác nhận hóa đơn thành công." });
    }
  
    // PUT: api/orders/id/refuse
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/refuse")]
    public ActionResult RefuseOrder (string id, EditOrder values) {
      var order = _service.RefuseOrder(id, values);

      if (order.Error == 1) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      return Ok (new { success = true, data = order.Data, message = "Từ chối hóa đơn thành công." });
    }

    // POST: api/orders
    [AllowAnonymous]
    [HttpPost]
    public ActionResult PostOrder (AddOrder values) {
      var tickets = _service.PostOrder(values);
        
      return Ok (new { success = true, message = "Thêm thành công.", data = tickets.Data });
    }
  }
}