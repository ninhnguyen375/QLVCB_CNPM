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
    public class TicketCategoriesController : ControllerBase
    {
        private readonly ITicketCategoryService _service;
        
        public TicketCategoriesController(ITicketCategoryService ticketCategoryService) {
          _service = ticketCategoryService;
        }

        // GET: api/ticketcategories
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetTicketCategoriesAsync([FromQuery] Pagination pagination, [FromQuery] SearchTicketCategory search) {
          var ticketCategories = await _service.GetTicketCategoriesAsync(pagination, search);

          return ticketCategories;
        }

        // GET: api/ticketcategories/id
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetTicketCategoryAsync(int id) {
          var ticketCategory = await _service.GetTicketCategoryAsync(id);

          return ticketCategory;
        }

        // PUT: api/ticketcategories/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutTicketCategoryAsync(int id, SaveTicketCategoryDTO saveTicketCategoryDTO) {
          var ticketCategory = await _service.UpdateTicketCategoryAsync(id, saveTicketCategoryDTO);

          return ticketCategory;
        }

        // POST: api/ticketcategories
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public async Task<ActionResult> PostTicketCategoryAsync(SaveTicketCategoryDTO saveTicketCategoryDTO) {
          var res = await _service.AddTicketCategoryAsync(saveTicketCategoryDTO);
          
          return res;
        }

        // DELETE: api/ticketcategories/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteTicketCategoryAsync(int id) {
          var res = await _service.DeleteTicketCategoryAsync(id);

          return res;
        }
    }
}