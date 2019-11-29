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

        return dates;
      }

      // GET: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet ("{id}")]
      public async Task<ActionResult> GetDateAsync(int id) {
        var date = await _service.GetDateAsync(id);

        return date;
      }

      // PUT: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public async Task<ActionResult> PutDateAsync(int id, SaveDateDTO saveDateDTO) {
        var date = await _service.UpdateDateAsync(id, saveDateDTO);

        return date;
      }

      // POST: api/dates
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public async Task<ActionResult> PostDateAsync(SaveDateDTO saveDateDTO) {
        var res = await _service.AddDateAsync(saveDateDTO);

        return res;
      }

      // DELETE: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}")]
      public async Task<ActionResult> DeleteDateAsync(int id) {
        var res = await _service.DeleteDateAsync(id);

        return res;
      }

      // POST: api/dates/id/addflights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost ("{id}/addflights")]
      public async Task<ActionResult> PostFlightAsync(int id, AddDateFlight values) {
        var res = await _service.AddDateFlightAsync(id, values);

        return res;
      }

      // DELETE: api/dates/id/removeflight
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}/removeflight")]
      public async Task<ActionResult> DeleteFlightAsync(int id, RemoveFlight values) {
        var res = await _service.DeleteFlightAsync(id, values);

        return res;
      }

      // GET: api/dates/searchflights
      [AllowAnonymous]
      [HttpGet ("/api/searchflights")]
      public async Task<ActionResult> SearchFlightsAsync([FromQuery] SearchFlightFE values) {
        var flights = await _service.SearchFlightsAsync(values);
        
        return flights;
      }

      // GET: api/dates/passengerslist
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet ("passengerslist")]
      public async Task<ActionResult> GetPassengersAsync([FromQuery] PassengersList values) {
        var passengers = await _service.GetPassengersAsync(values);

        return passengers;
      }
    }
}