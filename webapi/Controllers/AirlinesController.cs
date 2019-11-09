using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class AirlinesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AirlinesController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/airlines
        [HttpGet]
        public ActionResult GetAirlines() {
          var airlines = _unitOfWork.Airlines.GetAll();
          var totalCount = airlines.Count<Airline> ();

          return Ok (new { success = true, data = airlines, totalCount = totalCount });
        }

        // GET: api/airlines/id
        [HttpGet ("{id}")]
        public ActionResult GetAirline(string id) {
          var airline = _unitOfWork.Airlines.GetBy(id);

          if (airline == null) {
            return NotFound (new { success = false, message = "Invalid airline" });
          }

          return Ok (new { success = true, data = airline });
        }

        // PUT: api/airlines/id
        [HttpPut ("{id}")]
        public ActionResult PutAirline(string id, EditAirline values) {
          var airline = _unitOfWork.Airlines.GetBy(id);

          if (airline == null) {
            return NotFound (new { success = false, message = "Invalid airline" });
          }
          
          airline.Name = values.Name;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = airline });
        }

        // POST: api/airlines
        [HttpPost]
        public ActionResult PostAirline(Airline airline) {
          // Check id đã tồn tại trong Database chưa
          if(_unitOfWork.Airlines.Find(u => u.Id.Equals(airline.Id)).Count() != 0) {
            return BadRequest(new {
                Id = "Mã hãng hàng không đã được sử dụng"
            });
          }

          _unitOfWork.Airlines.Add(airline);
          _unitOfWork.Complete();

          return Ok (new { sucess = true, message = "Add successfully" });
        }

        // DELETE: api/airlines/id
        [HttpDelete ("{id}")]
        public ActionResult DeleteAirline(string id) {
          var airline = _unitOfWork.Airlines.GetBy(id);

          if (airline == null) {
            return NotFound (new { success = false, message = "Invalid airline" });
          }

          _unitOfWork.Airlines.Remove(airline);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Delete successfully" });
        }
    }
}