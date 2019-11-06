using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
      private readonly IUnitOfWork _unitOfWork;

      public FlightsController(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
      }

      // GET: api/flights
      [HttpGet]
      public ActionResult GetFlights() {
        var flights = _unitOfWork.Flights.GetAll();
        int totalCount = flights.Count<Flight> ();

        return Ok (new { success = true, data = flights, totalCount = totalCount });
      }

      // GET: api/flights/id
      [HttpGet ("{id}")]
      public ActionResult GetFlight(string id) {
        var flight = _unitOfWork.Flights.GetBy(id);

        if (flight == null) {
          return NotFound (new { success = false, message = "Invalid Flight" });
        }

        return Ok (new { success = true, data = flight });
      }

      // PUT: api/flights/id
      [HttpPut ("{id}")]
      public ActionResult PutFlight(string id, EditFlight values) {
        var flight = _unitOfWork.Flights.GetBy(id);

        if (flight == null) {
          return NotFound (new { success = false, message = "Invalid Flight" });
        }

        flight.StartTime = values.StartTime;
        flight.FlightTime = values.FlightTime;
        flight.AirportFrom = values.AirportFrom;
        flight.AirportTo = values.AirportTo;
        flight.Status = values.Status;
        flight.AirlineId = values.AirlineId;

        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Add Successfully" });
      }

      // POST: api/flights
      [HttpPost]
      public ActionResult PostFlight(Flight flight) {
        var flightTemp = _unitOfWork.Flights.GetBy(flight.Id);

        if (flightTemp != null) {
          return BadRequest (new { success = false, message = "Id already exists" });
        }

        _unitOfWork.Flights.Add(flight);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Add Successfully" });
      }

      // DELETE: api/flights/id
      [HttpDelete ("{id}")]
      public ActionResult DeleteFlight(string id) {
        var flight = _unitOfWork.Flights.GetBy(id);

        if (flight == null) {
          return NotFound (new { success = false, message = "Invalid Flight" });
        }

        _unitOfWork.Flights.Remove(flight);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Delete Successfully" });
      }
    }
}