using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> GetCustomersAsync([FromQuery] Pagination pagination, [FromQuery] SearchCustomer search) {
          var customers = await _service.GetCustomersAsync(pagination, search);

          return customers;
        }

        // GET: api/customers/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetCustomerAsync(string id) {
          var customer = await _service.GetCustomerAsync(id);
         
          return customer;
        }

        // PUT: api/customers/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutCustomerAsync(string id, SaveCustomerDTO saveCustomerDTO) {
          var customer = await _service.PutCustomerAsync(id, saveCustomerDTO);
          
          return customer;
        }
    }
}