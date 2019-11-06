using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.infrastructure.Persistance;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase 
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirportsController (IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/airports (GET all airports)
        [HttpGet]
        public ActionResult GetAirports ([FromQuery] Pagination pagination) {
          var airports = _unitOfWork.Airports.GetAll();
          // int totalCount = airports.Count<Airport> (); // Đang test thui

          return Ok (PaginatedList<Airport>.Create (airports, pagination.page, pagination.offset));
        }

        // GET: api/airports/id (GET airport by Id)
        [HttpGet ("{id}")]
        public ActionResult GetAirport (string id) {
          var airport = _unitOfWork.Airports.GetBy(id);

          if (airport == null) {
            return NotFound(new { success = false, message = "Invalid airport" });
          }

          return Ok (new { success = true, data = airport });
        }

        // PUT: api/airports/id
        [HttpPut ("{id}")]
        public ActionResult PutAirport (string id, EditAirport values) {
          var airport = _unitOfWork.Airports.GetBy(id);

          if (airport == null) {
            return NotFound(new { success = false, message = "Invalid airport" });
          }

          airport.Name = values.Name;
          airport.Location = values.Location;

          _unitOfWork.Complete();

          return Ok (new { success = true, data = airport });
        }

        // POST: api/airports
        [HttpPost]
        public ActionResult PostAirport (Airport airport) {
          // Check Id đã tồn tại trong database chưa
          var isValidId = _unitOfWork.Airports.Find(a => a.Id == airport.Id).Count();

          if (isValidId > 0) {
            return BadRequest (new { success = false, message = "Id already exists" });
          }

          _unitOfWork.Airports.Add(airport);
          _unitOfWork.Complete();

          return Ok (new {  success = true, message = "Add Successfully" });
        }

        // DELETE: api/airports/id
        [HttpDelete ("{id}")] 
        public ActionResult DeleteAirport (string id) {
          var airport = _unitOfWork.Airports.GetBy(id);

          if (airport == null) {
            return NotFound(new { success = false, message = "Invalid airport" });
          }

          _unitOfWork.Airports.Remove(airport);
          _unitOfWork.Complete();

          return Ok (new {  success = true, message = "Delete Successfully" });
        }
    }
}