using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class AirlinesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirlinesController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/airlines
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAirlines([FromQuery] Pagination pagination, [FromQuery] SearchAirline search) {
          var airlines = _unitOfWork.Airlines.GetAll();

          // Search by Id:
          if (search.Id != "") {
            airlines = airlines.Where(a =>
              a.Id.ToLower().Contains(search.Id.ToLower()));
          }

          // Search by Name:
          if (search.Name != "") {
            airlines = airlines.Where(a =>
            a.Name.ToLower().Contains(search.Name.ToLower()));
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            airlines = airlines.OrderBy(a => 
              a.GetType().GetProperty(search.sortAsc).GetValue(a));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            airlines = airlines.OrderByDescending(a =>
              a.GetType().GetProperty(search.sortDesc).GetValue(a));
          }

          return Ok (PaginatedList<Airline>.Create(airlines, pagination.current, pagination.pageSize));
        }

        // GET: api/airlines/id
        [AllowAnonymous]
        [HttpGet ("{id}")]
        public ActionResult GetAirline(string id) {
          var airline = _unitOfWork.Airlines.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }

          return Ok (new { success = true, data = airline });
        }

        // PUT: api/airlines/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutAirline(string id, EditAirline values) {
          var airline = _unitOfWork.Airlines.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }
          
          if (_unitOfWork.Airlines.Find(a =>
                a.Name.ToLower().Equals(values.Name.ToLower()) &&
                !a.Id.ToLower().Equals(id.ToLower()))
                .Count() != 0) {
            return BadRequest (new  { Name = "Tên hãng hàng không này đã tồn tại." });
          }

          airline.Name = values.Name;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = airline, message = "Sửa thành công." });
        }

        // POST: api/airlines
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostAirline(Airline airline) {
          // Check id đã tồn tại trong Database chưa
          if(_unitOfWork.Airlines.Find(a => 
              a.Id.ToLower().Equals(airline.Id.ToLower()))
              .Count() != 0) {
            return BadRequest(new {
                Id = "Mã hãng hàng không này đã tồn tại."
            });
          }

          // Check name đã tồn tại trong Database chưa
          if(_unitOfWork.Airlines.Find(a => 
              a.Name.ToLower().Equals(airline.Name.ToLower()))
              .Count() != 0) {
            return BadRequest(new {
                Name = "Tên hãng hàng không này đã tồn tại."
            });
          }

          _unitOfWork.Airlines.Add(airline);
          _unitOfWork.Complete();

          return Ok (new { sucess = true, message = "Thêm thành công." });
        }

        // DELETE: api/airlines/id
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public ActionResult DeleteAirline(string id) {
          var airline = _unitOfWork.Airlines.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }

          _unitOfWork.Airlines.Remove(airline);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}