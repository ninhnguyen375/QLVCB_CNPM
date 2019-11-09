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
    public class DateFlightsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DateFlightsController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/dateflights
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDateFlights([FromQuery] Pagination pagination, [FromQuery] SearchDateFlight search) {
          var dateFlights = _unitOfWork.DateFlights.GetAll();
          
          IEnumerable<DateFlight> listDateFlights;
          // Search by FlightID
          if (search.FlightId != "") {
            listDateFlights = dateFlights.Where(df =>
              df.FlightId == search.FlightId
            );
          }

          // Search by DateDeparture
          if (search.DepartureDate != "") {
            listDateFlights = dateFlights.Where(df =>
              df.FlightId == search.FlightId
            );
          }
          listDateFlights = dateFlights.Where(df =>
              df.FlightId == search.FlightId
            );
          return Ok (PaginatedList<DateFlight>.Create(listDateFlights, pagination.current, pagination.pageSize));
        }
    }
}