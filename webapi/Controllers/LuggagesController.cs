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
    public class LuggagesController : ControllerBase
    {
        private readonly ILuggageService _service;

        public LuggagesController(ILuggageService luggageService) {
          _service = luggageService;
        }

        // GET: api/luggages
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetLuggagesAsync([FromQuery] Pagination pagination, [FromQuery] SearchLuggage search) {
          var luggages = await _service.GetLuggagesAsync(pagination, search);

          return Ok (PaginatedList<LuggageDTO>.Create(luggages, pagination.current, pagination.pageSize));
        }

        // GET: api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetLuggageAsync(int id) {
          var luggage = await _service.GetLuggageAsync(id);

          if (luggage == null) {
            return NotFound (new { Id = "Mã hành lý này không tồn tại." });
          }

          return Ok (new { success = true, data = luggage });
        }

        // PUT: api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutLuggageAsync(int id, SaveLuggageDTO saveLuggageDTO) {
          var luggage = await _service.PutLuggageAsync(id, saveLuggageDTO);

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
        public async Task<ActionResult> PostLuggageAsync(SaveLuggageDTO saveLuggageDTO) {
          // Mapping: SaveLuggage
          var luggage = await _service.PostLuggageAsync(saveLuggageDTO);

          if (luggage.Error == 1) {
            return BadRequest(new { LuggageWeight = "Khối lượng hành lý đã được thiết lập" });
          }

          return Ok (new { success = true, message = "Thêm thành công" });
        }

        // DELETE : api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteLuggageAsync(int id) {
          var luggage = await _service.DeleteLuggageAsync(id);

          if (luggage.Error == 1) {
            return NotFound (new { message = "Mã hành lý này không tồn tại." });
          }
          
          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}