using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService customerService) {
          _service = customerService;
        }

        // GET: api/customers/
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet]
        public ActionResult GetCustomers([FromQuery] Pagination pagination, [FromQuery] SearchCustomer search) {
          var customers = _service.GetCustomers(pagination, search);

          return Ok (PaginatedList<CustomerDTO>.Create(customers, pagination.current, pagination.pageSize));
        }

        // GET: api/customers/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet ("{id}")]
        public ActionResult GetCustomer(string id) {
          var customer = _service.GetCustomer(id);
         
          if (customer == null) {
            return NotFound (new { Id = "Mã khách hàng này không tồn tại." });
          }

          return Ok (new { success = true, data = customer });
        }

        // PUT: api/customers/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutCustomer(string id, SaveCustomerDTO saveCustomerDTO) {
          var customer = _service.PutCustomer(id, saveCustomerDTO);
          
          if (customer.Error == 1) {
            return NotFound (new { Id = "Mã khách hàng này không tồn tại." });
          } else if (customer.Error == 2) {
            return BadRequest (new { Phone = "Số điện thoại này đã tồn tại." });
          }

          return Ok (new { success = true, data = customer.Data, message = "Sửa thành công." });
        }
    }
}