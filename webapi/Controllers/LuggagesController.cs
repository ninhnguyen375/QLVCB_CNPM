using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class LuggagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LuggagesController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/luggages
        [HttpGet]
        public ActionResult GetLuggages() {
          var luggages = _unitOfWork.Luggages.GetAll();
          var totalCount = luggages.Count<Luggage> ();
          
          return Ok (new { success = true, data = luggages });
        }

        // GET: api/luggages/1
        [HttpGet ("{id}")]
        public ActionResult GetLuggage(int id) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return NotFound (new { success = false, message = "Invalid luggage" });
          }

          return Ok (new { success = true, data = luggage });
        }

        // PUT: api/luggages/1
        [HttpPut ("{id}")]
        public ActionResult PutLuggage(int id, EditLuggage values) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return NotFound (new { success = false, message = "Invalid luggage" });
          }

          luggage.LuggageWeight = values.LuggageWeight;
          luggage.Price = values.Price;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = luggage });
        }

        // POST: api/luggages
        [HttpPost]
        public ActionResult PostLuggage(Luggage luggage) {
          if(_unitOfWork.Luggages.Find(u => u.LuggageWeight.Equals(luggage.LuggageWeight)).Count() != 0) {
            return BadRequest(new {
                LuggageWeight = "Khối lượng hành lý đã được thiết lập"
            });
          }
          _unitOfWork.Luggages.Add(luggage);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Add Successfully" });
        }

        // DELETE : api/luggages/1
        [HttpDelete ("{id}")]
        public ActionResult DeleteLuggage(int id) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return NotFound (new { success = false, message = "Invalid luggage" });
          }

          _unitOfWork.Luggages.Remove(luggage);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Delete Succesfully" });
        }
    }
}