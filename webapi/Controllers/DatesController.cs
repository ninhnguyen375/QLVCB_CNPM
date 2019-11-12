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
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet]
      public ActionResult GetDates([FromQuery] Pagination pagination, [FromQuery] SearchDate search) {
        var dates = _unitOfWork.Dates.GetAll();

        // Search by DepartureDate
        if (search.DepartureDate != "") {
          DateTime departureDate = Convert.ToDateTime(search.DepartureDate);

          dates = dates.Where(d =>
            d.DepartureDate == departureDate);
        }

        // Sort Asc:
        if (search.sortAsc != "") {
          dates = dates.OrderBy(d =>
            d.GetType().GetProperty(search.sortAsc).GetValue(d));
        }
        
        // Sort Desc:
        if (search.sortDesc != "") {
          dates = dates.OrderByDescending(d =>
            d.GetType().GetProperty(search.sortDesc).GetValue(d));
        }

        return Ok (PaginatedList<Date>.Create(dates, pagination.current, pagination.pageSize));
      }

      // GET: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet ("{id}")]
      public ActionResult GetDate(int id) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        }

        return Ok (new { success = true, data = date });
      }

      // PUT: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPut ("{id}")]
      public ActionResult PutDate(int id, EditDate values) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        }

        if (_unitOfWork.Dates.Find(d =>
              d.DepartureDate == Convert.ToDateTime(values.DepartureDate))
              .Count() != 0 ) {
          return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." });        
        }

        date.DepartureDate = Convert.ToDateTime(values.DepartureDate);
        _unitOfWork.Complete();

        return Ok (new { success = true, data = date, message = "Sửa thành công" });
      }

      // POST: api/dates
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost]
      public ActionResult PostDate(AddDate values) {
        DateTime departureDate = Convert.ToDateTime(values.DepartureDate);

        var date = _unitOfWork.Dates.Find(d =>
          d.DepartureDate == departureDate
        );

        if (date.Count() > 0) {
          return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." });
        }

        _unitOfWork.Dates.Add(
          new Date {
            DepartureDate = departureDate
          }
        );
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      // DELETE: api/dates/id
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}")]
      public ActionResult DeleteDate(int id) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        }
      
        _unitOfWork.Dates.Remove(date);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Xóa thành công" });
      }
    }
}