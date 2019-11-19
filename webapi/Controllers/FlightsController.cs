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
      public ActionResult GetFlights([FromQuery] Pagination pagination, [FromQuery] SearchFlight search) {
        var flights = _service.GetFlights(pagination, search);

        return Ok (PaginatedList<FlightDTO>.Create(flights, pagination.current, pagination.pageSize));
      }

      // GET: api/flights/id
      [AllowAnonymous]
      [HttpGet ("{id}")]
      public ActionResult GetFlight(string id) {
        var flight = _service.GetFlight(id);

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        return Ok (new { success = true, data = flight });
      }

      // PUT: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public ActionResult PutFlight(string id, SaveFlightDTO values) {
        var flight = _service.PutFlight(id, values);

        if (flight.Error == 1) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        } else if (flight.Error == 2) {
          return BadRequest("Mã chuyến bay không hợp lệ.");
        }

        return Ok (new { success = true, message = "Sửa thành công" });
      }

      // POST: api/flights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public ActionResult PostFlight(SaveFlightDTO saveFlightDTO) {
        var flight = _service.PostFlight(saveFlightDTO);

        if (flight.Error == 1) {
          return BadRequest (new { Id = "Mã chuyến bay này đã tồn tại." });
        }

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      // DELETE: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}")]
      public ActionResult DeleteFlight(string id) {
        var flight = _service.DeleteFlight(id);

        if (flight.Error == 1) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        return Ok (new { success = true, message = "Xóa thành công." });
      }

      // POST: api/flights/id/addflightticketcategory
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost ("{id}/addflightticketcategory")]
      public ActionResult PostFlightTicketCategories(string id, SaveFlightTicketCategoryDTO values) {
        var flight = _service.PostFlightTicketCategories(id, values);
        
        if (flight.Error == 1) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        } else if (flight.Error == 2) {
          return BadRequest (new { Id = "Loại vé của chuyến bay này đã tồn tại." });
        }

        return Ok (new { success = true, message = "Thêm loại vé thành công." });
      }

      // DELETE: api/flights/id/removeflightticketcategory
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}/removeflightticketcategory")]
      public ActionResult DeleteFlightTicketCategories(string id, RemoveFlightTicketCategory values) {
        var flight = _service.DeleteFlightTicketCategories(id, values);
        
        if (flight.Error == 1) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        } else if (flight.Error == 2) {
          return BadRequest (new { Id = "Loại vé của chuyến bay này không tồn tại." });
        }

        return Ok (new { success = true, message = "Xóa thành công." });
      }
    }
}