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

          return airlines;
        }

        // GET: api/airlines/id
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetAirlineAsync(string id) {
          var airline = await _service.GetAirlineAsync(id);

          return airline;
        }

        // PUT: api/airlines/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutAirlineAsync(string id, SaveAirlineDTO saveAirlineDTO) {
          var airline = await _service.UpdateAirlineAsync(id, saveAirlineDTO);

          return airline;
        }

        // POST: api/airlines
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public async Task<ActionResult> PostAirlineAsync(SaveAirlineDTO saveAirlineDTO) {
          var res = await _service.AddAirlineAsync(saveAirlineDTO);

          return res;
        }

        // DELETE: api/airlines/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteAirlineAsync(string id) {
          var res = await _service.DeleteAirlineAsync(id);

          return res;
        }
    }
}