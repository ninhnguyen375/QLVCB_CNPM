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
using System.Threading.Tasks;

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
    public async Task<ActionResult> GetOrdersAsync ([FromQuery] Pagination pagination, [FromQuery] SearchOrder search) {
      var orders = await _service.GetOrdersAsync(pagination, search);
      
      return Ok (PaginatedList<OrderDTO>.Create(orders, pagination.current, pagination.pageSize));
    }

    // GET: api/orders/id
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet ("{id}")]
    public async Task<ActionResult> GetOrderAsync (string id) {
      var order= await _service.GetOrderAsync(id);  

      if (order == null) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      return Ok (new { success = true, data = order });
    }

    // PUT: api/orders/id/accept
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/accept")]
    public async Task<ActionResult> AcceptOrderAsync (string id) {
      var currentUserId = int.Parse (User.Identity.Name);
      var order = await _service.AcceptOrderAsync(id, currentUserId);

      if (order.Error == 1) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      return Ok (new { success = true, data = order.Data, message = "Xác nhận hóa đơn thành công." });
    }
  
    // PUT: api/orders/id/refuse
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/refuse")]
    public async Task<ActionResult> RefuseOrderAsync (string id) {
      var currentUserId = int.Parse (User.Identity.Name);
      var order = await _service.RefuseOrderAsync(id, currentUserId);

      if (order.Error == 1) {
        return NotFound (new { Id = "Mã hóa đơn không tồn tại." });
      }

      return Ok (new { success = true, data = order.Data, message = "Từ chối hóa đơn thành công." });
    }

    // POST: api/orders
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> PostOrderAsync (AddOrder values) {
      var tickets = await _service.PostOrderAsync(values);
        
      return Ok (new { success = true, message = "Thêm thành công.", data = tickets.Data });
    }
  }
}