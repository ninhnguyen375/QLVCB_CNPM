using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.infrastructure.Persistance;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class DatesController : ControllerBase
    {
      private readonly IUnitOfWork _unitOfWork;

      public DatesController(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
      }

      // GET: api/dates
      [HttpGet]
      public ActionResult GetDates([FromQuery] Pagination pagination) {
        var dates = _unitOfWork.Dates.GetAll();
        
        return Ok (PaginatedList<Date>.Create(dates, pagination.page, pagination.offset));
      }

      // GET: api/dates/id
      [HttpGet ("{id}")]
      public ActionResult GetDate(int id) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { success = false, message = "Invalid Date" });
        }

        return Ok (new { success = true, data = date });
      }

      // POST: api/dates
      [HttpPost]
      public ActionResult PostDate(Date date) {
        var dateTemp = _unitOfWork.Dates.GetBy(date.Id);

        if (dateTemp != null) {
          return BadRequest (new { success = false, message = "Date Flight already exists" });
        }

        return Ok (new { success = true, message = "Add Successfully" });
      }
    }
}