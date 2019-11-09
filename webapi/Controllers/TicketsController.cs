using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketsController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/tickets
        [HttpGet]
        public ActionResult GetTickets([FromQuery] Pagination pagination, [FromQuery] SearchTicket search) {
          var tickets = _unitOfWork.Tickets.GetAll();
          
          if (search.PassengerName != "") {
            tickets = tickets.Where(t => 
              t.PassengerName == search.PassengerName
            );
          }

          return Ok (PaginatedList<Ticket>.Create(tickets, pagination.current, pagination.pageSize));
        }

        // GET: api/tickets/id
        [HttpGet ("{id}")]
        public ActionResult GetTicket(string id) {
          var ticket = _unitOfWork.Tickets.GetBy(id);

          if (ticket == null) {
            return NotFound (new { success = false, message = "Inivalid Ticket" });
          }

          return Ok (new { success = true, data = ticket });
        }
    }
}