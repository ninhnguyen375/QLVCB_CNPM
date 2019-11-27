using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;
using AutoMapper;
using webapi.core.DTOs;
using Microsoft.EntityFrameworkCore;
using webapi.Interfaces;
using System.Threading.Tasks;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
      private readonly IFlightService _service;

      public FlightsController(IFlightService flightService) {
        _service = flightService;
      }

      // GET: api/flights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet]
      public async Task<ActionResult> GetFlightsAsync([FromQuery] Pagination pagination, [FromQuery] SearchFlight search) {
        var flights = await _service.GetFlightsAsync(pagination, search);

        return flights;
      }

      // GET: api/flights/id
      [AllowAnonymous]
      [HttpGet ("{id}")]
      public async Task<ActionResult> GetFlightAsync(string id) {
        var flight = await _service.GetFlightAsync(id);

        return flight;
      }

      // PUT: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public async Task<ActionResult> PutFlightAsync(string id, SaveFlightDTO values) {
        var flight = await _service.PutFlightAsync(id, values);

        return flight;
      }

      // POST: api/flights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public async Task<ActionResult> PostFlightAsync(SaveFlightDTO saveFlightDTO) {
        var res = await _service.PostFlightAsync(saveFlightDTO);

        return res;
      }

      // DELETE: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}")]
      public async Task<ActionResult> DeleteFlightAsync(string id) {
        var res = await _service.DeleteFlightAsync(id);

        return res;
      }

      // POST: api/flights/id/addflightticketcategory
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost ("{id}/addflightticketcategory")]
      public async Task<ActionResult> PostFlightTicketCategoriesAsync(string id, SaveFlightTicketCategoryDTO values) {
        var res = await _service.PostFlightTicketCategoriesAsync(id, values);
        
        return res;
      }

      // DELETE: api/flights/id/removeflightticketcategory
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}/removeflightticketcategory")]
      public async Task<ActionResult> DeleteFlightTicketCategoriesAsync(string id, RemoveFlightTicketCategory values) {
        var res = await _service.DeleteFlightTicketCategoriesAsync(id, values);
        
        return res;
      }
    }
}