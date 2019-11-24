using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;
using AutoMapper;
using webapi.core.DTOs;
using webapi.Interfaces;
using System.Threading.Tasks;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class DatesController : ControllerBase
    {
      private readonly IDateService _service;

      public DatesController(IDateService dateService) {
        _service = dateService;
      }

      // GET: api/dates
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet]
      public async Task<ActionResult> GetDatesAsync([FromQuery] Pagination pagination, [FromQuery] SearchDate search) {
        var dates = await _service.GetDatesAsync(pagination, search);

        return Ok (PaginatedList<DateDTO>.Create(dates, pagination.current, pagination.pageSize));
      }

      // GET: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet ("{id}")]
      public async Task<ActionResult> GetDateAsync(int id) {
        var date = await _service.GetDateAsync(id);

        if (date == null) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        }

        return Ok (new { success = true, data = date });
      }

      // PUT: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public async Task<ActionResult> PutDateAsync(int id, SaveDateDTO saveDateDTO) {
        var date = await _service.PutDateAsync(id, saveDateDTO);

        if (date.Error == 1) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        } else if (date.Error == 2) {
          return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." });        
        }

        return Ok (new { success = true, data = date.Data, message = "Sửa thành công" });
      }

      // POST: api/dates
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public async Task<ActionResult> PostDateAsync(SaveDateDTO saveDateDTO) {
        var date = await _service.PostDateAsync(saveDateDTO);

        if (date.Error == 1) {
          return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." });
        }

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      // DELETE: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}")]
      public async Task<ActionResult> DeleteDateAsync(int id) {
        var date = await _service.DeleteDateAsync(id);

        if (date.Error == 1) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        } 

        return Ok (new { success = true, message = "Xóa thành công" });
      }

      // POST: api/dates/id/addflights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost ("{id}/addflights")]
      public async Task<ActionResult> PostFlightAsync(int id, AddDateFlight values) {
        var date = await _service.PostFlightAsync(id, values);

        if (date.Error == 1) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        } else if (date.Error == 2) {
          return BadRequest(new { success = false, message = "Chuyến bay đã tồn tại trong ngày." });
        }

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      // DELETE: api/dates/id/removeflight
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}/removeflight")]
      public async Task<ActionResult> DeleteFlightAsync(int id, RemoveFlight values) {
        var date = await _service.DeleteFlightAsync(id, values);

        if (date.Error == 1) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        } else if (date.Error == 2) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        } else if (date.Error == 3) {
          return BadRequest (new { SeatsLeft = "Không thể xóa vì loại vé của chuyến bay này đã được bán." });
        }

        return Ok (new { success = true, message = "Xóa thành công" });
      }

      // GET: api/dates/searchflights
      [AllowAnonymous]
      [HttpGet ("/api/searchflights")]
      public async Task<ActionResult> SearchFlightsAsync([FromQuery] SearchFlightFE values) {
        var flights = await _service.SearchFlightsAsync(values);
        var departureFlights = flights.DepartureFlights;
        var returnFlights = flights.ReturnFlights;

        return Ok (new { success = true, DepartureFlights = departureFlights, ReturnFlights = returnFlights });
      }
    }
}