using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class TicketCategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketCategoriesController(IUnitOfWork unitOfWork) {
          _unitOfWork = unitOfWork;
        }

        // GET: api/ticketcategories
        [HttpGet]
        public ActionResult GetTicketCategories() {
          var ticketCategories = _unitOfWork.TicketCategories.GetAll();
          int totalCount = ticketCategories.Count<TicketCategory> ();

          return Ok (new { success = true, data = totalCount });
        }

        // GET: api/ticketcategories/id
        [HttpGet ("{id}")]
        public ActionResult GetTicketCategory(int id) {
          var ticketCategory = _unitOfWork.TicketCategories.GetBy(id);

          if (ticketCategory == null) {
            return NotFound (new { success = false, message = "Invailid Ticket category" });
          }

          return Ok (new { success = true, data = ticketCategory });
        }

        // PUT: api/ticketcategories/id
        [HttpPut ("{id}")]
        public ActionResult PutTicketCategory(int id, EditTicketCategory values) {
          var ticketCategory = _unitOfWork.TicketCategories.GetBy(id);

          if (ticketCategory == null) {
            return NotFound (new { success = false, message = "Invailid Ticket category" });
          }

          ticketCategory.Name = values.Name;
          _unitOfWork.Complete();

          return Ok (new { success = true, data = ticketCategory });
        }

        // POST: api/ticketcategories
        [HttpPost]
        public ActionResult PostTicketCategory(TicketCategory values) {
          var ticketCategory = _unitOfWork.TicketCategories.GetBy(values.Id);

          if (ticketCategory != null) {
            return BadRequest (new { success = false, message = "Id already exists" });
          }

          _unitOfWork.TicketCategories.Add(ticketCategory);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Add Successfully" });
        }
    }
}