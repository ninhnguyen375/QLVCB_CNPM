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
    public class AirportsController : ControllerBase 
    {
        private readonly IAirportService _service;

        public AirportsController (IAirportService airportService) {
          _service = airportService;
        }

        // GET: api/airports (GET all airports)
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAirportsAsync ([FromQuery] Pagination pagination, [FromQuery] SearchAirport search) {
          var airports = await _service.GetAirportsAsync(pagination, search);

          return airports;
        }

        // GET: api/airports/id (GET airport by Id)
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public async Task<ActionResult> GetAirportAsync (string id) {
          var airport = await _service.GetAirportAsync(id);

          return airport;
        }

        // PUT: api/airports/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public async Task<ActionResult> PutAirportAsync (string id, SaveAirportDTO saveAirportDTO) {
          var airport = await _service.PutAirportAsync(id, saveAirportDTO);

          return airport;
        }

        // POST: api/airports
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public async Task<ActionResult> PostAirportAsync (SaveAirportDTO saveAirportDTO) {
          var res = await _service.PostAirportAsync(saveAirportDTO);

          return res;          
        }

        // DELETE: api/airports/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")] 
        public async Task<ActionResult> DeleteAirportAsync (string id) {
          var res = await _service.DeleteAirportAsync(id);

          return res;
        }
    }
}