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
      
      return orders;
    }

    // GET: api/orders/id
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpGet ("{id}")]
    public async Task<ActionResult> GetOrderAsync (string id) {
      var order = await _service.GetOrderAsync(id);  

      return order;
    }

    // PUT: api/orders/id/accept
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/accept")]
    public async Task<ActionResult> AcceptOrderAsync (string id) {
      var currentUserId = int.Parse (User.Identity.Name);
      var order = await _service.AcceptOrderAsync(id, currentUserId);

      return order;
    }
  
    // PUT: api/orders/id/refuse
    [Authorize (Roles = "STAFF, ADMIN")]
    [HttpPut ("{id}/refuse")]
    public async Task<ActionResult> RefuseOrderAsync (string id) {
      var currentUserId = int.Parse (User.Identity.Name);
      var order = await _service.RefuseOrderAsync(id, currentUserId);

      return order;
    }

    // POST: api/orders
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> PostOrderAsync (AddOrder values) {
      var res = await _service.AddOrderAsync(values);
        
      return res;
    }
  }
}