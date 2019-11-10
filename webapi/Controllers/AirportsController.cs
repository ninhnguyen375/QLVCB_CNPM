using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.infrastructure.Persistance;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase 
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirportsController (IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/airports (GET all airports)
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAirports ([FromQuery] Pagination pagination, [FromQuery] SearchAirport search) {
          var airports = _unitOfWork.Airports.GetAll();
          
          // Search by Id:
          if (search.Id != "") {
            airports = airports.Where(a =>
              a.Id.ToLower().Contains(search.Id.ToLower()));
          }

          // Search by Name:
          if (search.Name != "") {
            airports = airports.Where(a =>
              a.Name.ToLower().Contains(search.Name.ToLower()));
          }

          // Search by Location:
          if (search.Location != "") {
            airports = airports.Where(a =>
              a.Location.ToLower().Contains(search.Location.ToLower()));
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            airports = airports.OrderBy(a =>
              a.GetType().GetProperty(search.sortAsc).GetValue(a));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            airports = airports.OrderByDescending(a =>
              a.GetType().GetProperty(search.sortDesc).GetValue(a));
          }

          return Ok (PaginatedList<Airport>.Create (airports, pagination.current, pagination.pageSize));
        }

        // GET: api/airports/id (GET airport by Id)
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public ActionResult GetAirport (string id) {
          var airport = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }

          return Ok (new { success = true, data = airport });
        }

        // PUT: api/airports/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutAirport (string id, EditAirport values) {
          var airport = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }
          
          if (_unitOfWork.Airports.Find(a =>
                a.Name.ToLower().Equals(values.Name.ToLower()) &&
                !a.Id.ToLower().Equals(id.ToLower()))
                .Count() != 0) {
            return BadRequest (new  { Name = "Tên sân bay này đã tồn tại." });
          }

          airport.Name = values.Name;
          airport.Location = values.Location;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = airport, message = "Sửa thành công." });
        }

        // POST: api/airports
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostAirport (Airport airport) {
          // Check id đã tồn tại trong Database chưa
          if(_unitOfWork.Airports.Find(a => 
              a.Id.ToLower().Equals(airport.Id.ToLower()))
              .Count() != 0) {
            return BadRequest(new {
                Id = "Mã sân bay này đã tồn tại."
            });
          }

          // Check name đã tồn tại trong Database chưa
          if(_unitOfWork.Airports.Find(a => 
              a.Name.ToLower().Equals(airport.Name.ToLower()))
              .Count() != 0) {
            return BadRequest(new {
                Name = "Tên sân bay này đã tồn tại."
            });
          }

          _unitOfWork.Airports.Add(airport);
          _unitOfWork.Complete();

          return Ok (new { sucess = true, message = "Thêm thành công." });
        }

        // DELETE: api/airports/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")] 
        public ActionResult DeleteAirport (string id) {
          var airport = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }

          _unitOfWork.Airports.Remove(airport);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}