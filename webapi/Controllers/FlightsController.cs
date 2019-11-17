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
        // Mapping: Flight
        var flightsSource = _unitOfWork.Flights.GetAll();
        _unitOfWork.Airlines.GetAll();
        _unitOfWork.Airports.GetAll();
        _unitOfWork.TicketCategories.GetAll();
        _unitOfWork.Flights.GetFlightTicketCategories();
        var flights = _mapper.Map<IEnumerable<Flight>, IEnumerable<FlightDTO>>(flightsSource);

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
      [AllowAnonymous]
      [HttpGet ("{id}")]
      public ActionResult GetFlight(string id) {
        // Mapping: Flight
        var flightSource = _unitOfWork.Flights.GetBy(id);
        _unitOfWork.Airlines.GetAll();
        _unitOfWork.Airports.GetAll();
        _unitOfWork.TicketCategories.GetAll();
        _unitOfWork.Flights.GetFlightTicketCategories();
        var flight = _mapper.Map<Flight, FlightDTO>(flightSource);

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        return Ok (new { success = true, data = flight });
      }

      // PUT: api/flights/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public ActionResult PutFlight(string id, Flight saveFlightDTO) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        flight.StartTime = saveFlightDTO.StartTime;
        flight.FlightTime = saveFlightDTO.FlightTime;
        flight.AirportFrom = saveFlightDTO.AirportFrom;
        flight.AirportTo = saveFlightDTO.AirportTo;
        flight.SeatsCount = saveFlightDTO.SeatsCount;
        flight.Status = saveFlightDTO.Status;
        flight.AirlineId = saveFlightDTO.AirlineId;
        
        var flightTicketCategories = _unitOfWork.Flights.GetFlightTicketCategoriesById(id);
        for (int i = 0; i < saveFlightDTO.FlightTicketCategories.Count; i++) {
          flightTicketCategories.ElementAt(i).Price = saveFlightDTO.FlightTicketCategories.ElementAt(i).Price;
        }

        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Sửa thành công" });
      }

      // POST: api/flights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public ActionResult PostFlight(SaveFlightDTO saveFlightDTO) {
        // Mapping: SaveFlightDTO
        var flight = _mapper.Map<SaveFlightDTO, Flight>(saveFlightDTO);

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

        return Ok (new { success = true, message = "Xóa thành công." });
      }

      // POST: api/flights/id/addflightticketcategory
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost ("{id}/addflightticketcategory")]
      public ActionResult PostFlightTicketCategories(string id, FlightTicketCategory values) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
        
        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        var flightTicketCategory = _unitOfWork.FlightTicketCategories.Find(ftc =>
          ftc.FlightId.ToLower().Equals(id.ToLower()) &&
          ftc.TicketCategoryId == values.TicketCategoryId).SingleOrDefault();

        if (flightTicketCategory != null) {
          return BadRequest (new { Id = "Loại vé của chuyến bay này đã tồn tại." });
        }

        _unitOfWork.FlightTicketCategories.Add(values);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Thêm loại vé thành công." });
      }

      // DELETE: api/flights/id/removeflightticketcategory
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}/removeflightticketcategory")]
      public ActionResult DeleteFlightTicketCategories(string id, RemoveFlightTicketCategory values) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
        
        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        var flightTicketCategory = _unitOfWork.FlightTicketCategories.Find(ftc =>
          ftc.FlightId.ToLower().Equals(id.ToLower()) &&
          ftc.TicketCategoryId == values.TicketCategoryId).SingleOrDefault();

        if (flightTicketCategory == null) {
          return BadRequest (new { Id = "Loại vé của chuyến bay này không tồn tại." });
        }

        _unitOfWork.FlightTicketCategories.Remove(flightTicketCategory);
        _unitOfWork.Complete();

        
        return Ok (new { success = true, message = "Xóa thành công." });
      }
    }
}