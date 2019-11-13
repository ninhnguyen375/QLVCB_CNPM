using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;
using AutoMapper;
using webapi.core.DTOs;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class DatesController : ControllerBase
    {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public DatesController(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
      }

      // GET: api/dates
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpGet]
      public ActionResult GetDates([FromQuery] Pagination pagination, [FromQuery] SearchDate search) {
        _unitOfWork.Dates.getDateFlights();
        var dates = _mapper.Map<IEnumerable<Date>, IEnumerable<DateDTO>>(_unitOfWork.Dates.GetAll());

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

        // Default order newest departureDate
        dates = dates.OrderByDescending(d =>
            d.DepartureDate);

        return Ok (PaginatedList<DateDTO>.Create(dates, pagination.current, pagination.pageSize));
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
      public ActionResult PostDate(Date date) {
        DateTime departureDate = Convert.ToDateTime(date.DepartureDate);

        var dateTemp = _unitOfWork.Dates.Find(d =>
          d.DepartureDate == departureDate);

        if (dateTemp.Count() > 0) {
          return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." });
        }

        if (date.DateFlights != null) {
          var flights = _unitOfWork.Flights.GetAll();

          // Thêm ghế còn lại và trạng thái cho chuyến bay
          foreach (var dateFlight in date.DateFlights) {
            dateFlight.SeatsLeft = flights.Where(f =>
              f.Id == dateFlight.FlightId)
              .Select(f => f.SeatsCount)
              .SingleOrDefault();
            dateFlight.Status = 1; // Còn chỗ
          }
        }

        _unitOfWork.Dates.Add(date);
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
      
        // Xóa các chuyến bay trong ngày bị xóa
        var dateFlights = _unitOfWork.DateFlights.GetAll();
        foreach (var dateFlight in dateFlights) {
          if (dateFlight.DateId == id) {
            _unitOfWork.DateFlights.Remove(dateFlight);
          }
        }

        _unitOfWork.Dates.Remove(date);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Xóa thành công" });
      }

      // POST: api/dates/id/addflights
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpPost ("{id}/addflights")]
      public ActionResult PostFlight(int id, AddDateFlight values) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        }


        var flights = _unitOfWork.Flights.GetAll();

        // Thêm thông tin cho chuyến bay: gồm ngày, ghế còn lại, trạng thái
        foreach (var dateFlight in values.DateFlights) {
          if(_unitOfWork.Dates.getDateFlight(id, dateFlight.FlightId) == null) {
            dateFlight.DateId = id;
            dateFlight.SeatsLeft = flights.Where(f =>
              f.Id == dateFlight.FlightId)
              .Select(f => f.SeatsCount)
              .SingleOrDefault();
            dateFlight.Status = 1; // Còn chỗ
            _unitOfWork.DateFlights.Add(dateFlight);
          } else {
            return BadRequest(new { success = false, message = "Chuyến bay đã tồn tại trong ngày." });
          }
        }

        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      // DELETE: api/dates/id/removeflight
      [Authorize (Roles = "STAFF, ADMIN")]
      [HttpDelete ("{id}/removeflight")]
      public ActionResult DeleteFlight(int id, RemoveFlight values) {
        var date = _unitOfWork.Dates.GetBy(id);

        if (date == null) {
          return NotFound (new  { Id = "Mã ngày này không tồn tại." });
        }
      
        var flight = _unitOfWork.Flights.Find(a =>
            a.Id.ToLower().Equals(values.FlightId.ToLower()))
            .SingleOrDefault();

        // Kiểm tra chuyến bay có tồn tại hay không
        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        // Xóa chuyến bay được chọn
        var dateFlight = _unitOfWork.DateFlights.Find(df =>
          df.DateId == id &&
          df.FlightId.ToLower().Equals(values.FlightId.ToLower())).SingleOrDefault();

        _unitOfWork.DateFlights.Remove(dateFlight);
        _unitOfWork.Complete();

        return Ok (new { success = true, message = "Xóa thành công" });
      }
    }
}