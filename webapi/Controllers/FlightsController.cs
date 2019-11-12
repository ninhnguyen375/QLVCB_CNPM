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

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public FlightsController(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
      }

      // GET: api/flights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet]
      public ActionResult GetFlights([FromQuery] Pagination pagination, [FromQuery] SearchFlight search) {
        _unitOfWork.Airlines.GetAll();
        _unitOfWork.Airports.GetAll();
        _unitOfWork.TicketCategories.GetAll();
        _unitOfWork.Flights.GetFlightTicketCategories();
        IEnumerable<FlightDTO> flights = _mapper.Map<IEnumerable<Flight>, IEnumerable<FlightDTO>>(_unitOfWork.Flights.GetAll());

        // Search by Id:
        if (search.Id != "") {
          flights = flights.Where(f =>
            f.Id.ToLower().Contains(search.Id.ToLower()));
        }

        // Search by StartTime:
        if (search.StartTime != null) {
          flights = flights.Where(f =>
            f.StartTime == search.StartTime);
        }

        /// Search by FlightTime:
        if (search.FlightTime != null) {
          flights = flights.Where(f =>
            f.FlightTime == search.FlightTime);
        }

        // Search by AirportFrom:
        if (search.AirportFrom != "") {
          flights = flights.Where(f =>
            f.AirportFrom.ToLower().Contains(search.AirportFrom.ToLower()));
        }

        // Search by AirportTo:
        if (search.AirportTo != "") {
          flights = flights.Where(f =>
            f.AirportTo.ToLower().Contains(search.AirportTo.ToLower()));
        }

        // Search by AirlineName:
        if (search.AirlineName != "") {
          var airlines = _unitOfWork.Airlines.GetAll();

          flights = (from f in flights
                     from a in airlines
                     where f.AirlineId.Equals(a.Id) && 
                     a.Name.ToLower().Contains(search.AirlineName.ToLower())
                     select f);
        }

        // Search by Status:
        if (search.Status != null) {
          flights = flights.Where(f =>
            f.Status == search.Status);
        }

        // Sort Asc:
        if (search.sortAsc != "") {
          flights = flights.OrderBy(f =>
            f.GetType().GetProperty(search.sortAsc).GetValue(f));
        }

        // Sort Desc:
        if (search.sortDesc != "") {
          flights = flights.OrderByDescending(f =>
            f.GetType().GetProperty(search.sortDesc).GetValue(f));
        }

        return Ok (PaginatedList<FlightDTO>.Create(flights, pagination.current, pagination.pageSize));
      }

      // GET: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet ("{id}")]
      public ActionResult GetFlight(string id) {
        _unitOfWork.Airlines.GetAll();
        _unitOfWork.Airports.GetAll();
        _unitOfWork.TicketCategories.GetAll();
        _unitOfWork.Flights.GetFlightTicketCategories();
        var flight = _mapper.Map<Flight, FlightDTO>(_unitOfWork.Flights.GetBy(id));

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        return Ok (new { success = true, data = flight });
      }

      // PUT: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public ActionResult PutFlight(string id, EditFlight values) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        flight.StartTime = values.StartTime;
        flight.FlightTime = values.FlightTime;
        flight.AirportFrom = values.AirportFrom;
        flight.AirportTo = values.AirportTo;
        flight.SeatsCount = values.SeatsCount;
        flight.Status = values.Status;
        flight.AirlineId = values.AirlineId;

        _unitOfWork.Complete();

        return Ok (new { success = true, data = flight, message = "Sửa thành công" });
      }

      // POST: api/flights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public ActionResult PostFlight(Flight flight) {
        var flightTemp = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(flight.Id.ToLower())).SingleOrDefault();

        if (flightTemp != null) {
          return BadRequest (new { Id = "Mã chuyến bay này đã tồn tại." });
        }

        _unitOfWork.Flights.Add(flight);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      // DELETE: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}")]
      public ActionResult DeleteFlight(string id) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        _unitOfWork.Flights.Remove(flight);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Xóa thành công" });
      }
    }
}