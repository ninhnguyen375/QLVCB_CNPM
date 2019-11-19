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
    public class LuggagesController : ControllerBase
    {
        private readonly ILuggageService _service;

        public LuggagesController(ILuggageService luggageService) {
          _service = luggageService;
        }

        // GET: api/luggages
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetLuggages([FromQuery] Pagination pagination, [FromQuery] SearchLuggage search) {
          var luggages = _service.GetLuggages(pagination, search);

          return Ok (PaginatedList<LuggageDTO>.Create(luggages, pagination.current, pagination.pageSize));
        }

        // GET: api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet ("{id}")]
        public ActionResult GetLuggage(int id) {
          var luggage = _service.GetLuggage(id);

          if (luggage == null) {
            return NotFound (new { Id = "Mã hành lý này không tồn tại." });
          }

          return Ok (new { success = true, data = luggage });
        }

        // PUT: api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutLuggage(int id, SaveLuggageDTO saveLuggageDTO) {
          var luggage = _service.PutLuggage(id, saveLuggageDTO);

          if (luggage.Error == 1) {
            return NotFound (new { Id = "Mã hành lý này không tồn tại." });
          } else if (luggage.Error == 2) {
            return BadRequest (new { LuggageWeight = "Khối lượng hành lý đã được thiết lập" });
          }

          return Ok (new { success = true, data = luggage.Data, message = "Sửa thành công." });
        }

        // POST: api/luggages
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostLuggage(SaveLuggageDTO saveLuggageDTO) {
          // Mapping: SaveLuggage
          var luggage = _service.PostLuggage(saveLuggageDTO);

          if (luggage.Error == 1) {
            return BadRequest(new { LuggageWeight = "Khối lượng hành lý đã được thiết lập" });
          }

          return Ok (new { success = true, message = "Thêm thành công" });
        }

        // DELETE : api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public ActionResult DeleteLuggage(int id) {
          var luggage = _service.DeleteLuggage(id);

          if (luggage.Error == 1) {
            return NotFound (new { message = "Mã hành lý này không tồn tại." });
          }
          
          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}