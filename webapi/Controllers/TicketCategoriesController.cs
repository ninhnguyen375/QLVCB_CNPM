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
    public class TicketCategoriesController : ControllerBase
    {
        private readonly ITicketCategoryService _service;
        
        public TicketCategoriesController(ITicketCategoryService ticketCategoryService) {
          _service = ticketCategoryService;
        }

        // GET: api/ticketcategories
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetTicketCategories([FromQuery] Pagination pagination, [FromQuery] SearchTicketCategory search) {
          var ticketCategories = _service.GetTicketCategories(pagination, search);

          return Ok (PaginatedList<TicketCategoryDTO>.Create(ticketCategories, pagination.current, pagination.pageSize));
        }

        // GET: api/ticketcategories/id
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public ActionResult GetTicketCategory(int id) {
          var ticketCategory = _service.GetTicketCategory(id);

          if (ticketCategory == null) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          }

          return Ok (new { success = true, data = ticketCategory });
        }

        // PUT: api/ticketcategories/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutTicketCategory(int id, SaveTicketCategoryDTO saveTicketCategoryDTO) {
          var ticketCategory = _service.PutTicketCategory(id, saveTicketCategoryDTO);

          if (ticketCategory.Error == 1) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          } else if (ticketCategory.Error == 2) {
            return BadRequest (new { Name = "Loại vé này đã tồn tại." });
          }

          return Ok (new { success = true, data = ticketCategory.Data, message = "Sửa thành công." });
        }

        // POST: api/ticketcategories
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostTicketCategory(SaveTicketCategoryDTO saveTicketCategoryDTO) {
          var ticketCategory = _service.PostTicketCategory(saveTicketCategoryDTO);
          if (ticketCategory.Error == 1) {
            return BadRequest (new { Name = "Loại vé này đã tồn tại." });
          }

          return Ok (new { success = true, message = "Thêm thành công." });
        }

        // DELETE: api/ticketcategories/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public ActionResult DeleteTicketCategory(int id) {
          var ticketCategory = _service.DeleteTicketCategory(id);

          if (ticketCategory.Error == 1) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          }

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}