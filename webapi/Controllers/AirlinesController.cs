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
    public class AirlinesController : ControllerBase
    {
        private readonly IAirlineService _service;

        public AirlinesController(IAirlineService airlineService) {
          _service = airlineService;
        }

        // GET: api/airlines
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAirlinesAsync([FromQuery] Pagination pagination, [FromQuery] SearchAirline search) {
          var airlines = await _service.GetAirlinesAsync(pagination, search);

          return Ok (PaginatedList<AirlineDTO>.Create(airlines, pagination.current, pagination.pageSize));
        }

        // GET: api/airlines/id
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetAirlineAsync(string id) {
          var airline = await _service.GetAirlineAsync(id);

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }

          return Ok (new { success = true, data = airline });
        }

        // PUT: api/airlines/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutAirlineAsync(string id, SaveAirlineDTO saveAirlineDTO) {
          var airline = await _service.PutAirlineAsync(id, saveAirlineDTO);

          if (airline.Error == 1) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          } else if (airline.Error == 2) {
            return BadRequest (new { Name = "Tên hãng hàng không này đã tồn tại." });
          }
          
          return Ok (new { success = true, data = airline.Data, message = "Sửa thành công." });
        }

        // POST: api/airlines
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public async Task<ActionResult> PostAirlineAsync(SaveAirlineDTO saveAirlineDTO) {
          var airline = await _service.PostAirlineAsync(saveAirlineDTO);

          if(airline.Error == 1) {
            return BadRequest (new { Id = "Mã hãng hàng không này đã tồn tại." });
          } else if (airline.Error == 2) {
            return BadRequest(new { Name = "Tên hãng hàng không này đã tồn tại." });
          }

          return Ok (new { sucess = true, message = "Thêm thành công." });
        }

        // DELETE: api/airlines/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteAirlineAsync(string id) {
          var airline = await _service.DeleteAirlineAsync(id);

          if (airline.Error == 1) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}