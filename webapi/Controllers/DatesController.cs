using System;
using System.Collections.Generic;
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
    public class DatesController : ControllerBase
    {
      private readonly IUnitOfWork _unitOfWork;

      public DatesController(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
      }

      // GET: api/dates
      [Authorize (Roles = "STAFF")]
      [HttpGet]
      public ActionResult GetDates([FromQuery] Pagination pagination) {
        var dates = _unitOfWork.Dates.GetAll();
        
        return Ok (PaginatedList<Date>.Create(dates, pagination.current, pagination.pageSize));
      }

      // GET: api/dates/id
      [Authorize (Roles = "STAFF")]
      [HttpGet ("{id}")]
      public ActionResult GetDate(int id) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { success = false, message = "Invalid Date" });
        }

        return Ok (new { success = true, data = date });
      }

      // POST: api/dates
      [Authorize (Roles = "STAFF")]
      [HttpPost]
      public ActionResult PostDate(AddDate values) {
        DateTime departureDate = Convert.ToDateTime(values.DepartureDate);

        var date = _unitOfWork.Dates.Find(d =>
          d.DepartureDate == departureDate
        );

        if (date.Count() > 0) {
          return BadRequest (new { success = false, message = "DateFlight already exists" });
        }

        _unitOfWork.Dates.Add(
          new Date {
            DepartureDate = departureDate
          }
        );
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Add Successfully" });
      }
    }
}