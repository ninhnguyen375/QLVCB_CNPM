using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirportsController (IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        // GET: api/airports (GET all airports)
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAirports ([FromQuery] Pagination pagination, [FromQuery] SearchAirport search) {
          // Mapping: Airport
          var airportsSource = _unitOfWork.Airports.GetAll();
          var airports = _mapper.Map<IEnumerable<Airport>, IEnumerable<AirportDTO>>(airportsSource);
          
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

          return Ok (PaginatedList<AirportDTO>.Create (airports, pagination.current, pagination.pageSize));
        }

        // GET: api/airports/id (GET airport by Id)
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public ActionResult GetAirport (string id) {
          // Mapping: Airport
          var airportSource = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
          var airport = _mapper.Map<Airport, AirportDTO>(airportSource);

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }

          return Ok (new { success = true, data = airport });
        }

        // PUT: api/airports/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutAirport (string id, SaveAirportDTO saveAirportDTO) {
          var airport = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }
          
          if (_unitOfWork.Airports.Find(a =>
                a.Name.ToLower().Equals(saveAirportDTO.Name.ToLower()) &&
                !a.Id.ToLower().Equals(id.ToLower()))
                .Count() != 0) {
            return BadRequest (new  { Name = "Tên sân bay này đã tồn tại." });
          }

          // Mapping: SaveAirport
          _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO, airport); 

          _unitOfWork.Complete();

          return Ok (new { success = true, data = airport, message = "Sửa thành công." });
        }

        // POST: api/airports
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostAirport (SaveAirportDTO saveAirportDTO) {
          // Mapping: SaveAirport
          var airport = _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO);

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