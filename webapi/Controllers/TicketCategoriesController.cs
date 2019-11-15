using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class TicketCategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public TicketCategoriesController(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        // GET: api/ticketcategories
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetTicketCategories([FromQuery] Pagination pagination, [FromQuery] SearchTicketCategory search) {
          // Mapping: TicketCategory
          var ticketCategoriesSource = _unitOfWork.TicketCategories.GetAll();
          var ticketCategories = _mapper.Map<IEnumerable<TicketCategory>, IEnumerable<TicketCategoryDTO>>(ticketCategoriesSource);

          // Search by Name:
          if (search.Name != "") {
            ticketCategories = ticketCategories.Where(tc => 
              tc.Name.ToLower().Contains(search.Name.ToLower()));
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            ticketCategories = ticketCategories.OrderBy(tc => 
              tc.GetType().GetProperty(search.sortAsc).GetValue(tc));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            ticketCategories = ticketCategories.OrderByDescending(tc => 
              tc.GetType().GetProperty(search.sortDesc).GetValue(tc));
          }

          return Ok (PaginatedList<TicketCategoryDTO>.Create(ticketCategories, pagination.current, pagination.pageSize));
        }

        // GET: api/ticketcategories/id
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public ActionResult GetTicketCategory(int id) {
          // Mapping: TicketCategory
          var ticketCategorySource = _unitOfWork.TicketCategories.GetBy(id);
          var ticketCategory = _mapper.Map<TicketCategory, TicketCategoryDTO>(ticketCategorySource);

          if (ticketCategory == null) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          }

          return Ok (new { success = true, data = ticketCategory });
        }

        // PUT: api/ticketcategories/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutTicketCategory(int id, EditTicketCategory values) {
          var ticketCategory = _unitOfWork.TicketCategories.GetBy(id);

          if (ticketCategory == null) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          }

          if (_unitOfWork.TicketCategories.Find(tc =>
                tc.Name.ToLower().Equals(values.Name.ToLower()) &&
                tc.Id != id)
                .Count() != 0) {
            return BadRequest (new { Name = "Loại vé này đã tồn tại." });
          }

          ticketCategory.Name = values.Name;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = ticketCategory, message = "Sửa thành công." });
        }

        // POST: api/ticketcategories
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostTicketCategory(TicketCategory ticketCategory) {
          if (_unitOfWork.TicketCategories.Find(tc => 
                tc.Name.ToLower().Equals(ticketCategory.Name.ToLower()))
                .Count() != 0) {
            return BadRequest (new { Name = "Loại vé này đã tồn tại." });
          }

          _unitOfWork.TicketCategories.Add(ticketCategory);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Thêm thành công." });
        }

        // DELETE: api/ticketcategories/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public ActionResult DeleteTicketCategory(int id) {
          var ticketCategory = _unitOfWork.TicketCategories.GetBy(id);

          if (ticketCategory == null) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          }

          _unitOfWork.TicketCategories.Remove(ticketCategory);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}