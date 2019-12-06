using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using webapi.core.UseCases;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
      public OrderRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }

      public async Task<IEnumerable<Ticket>> GetTicketsByIdAsync(string id) {
        return (
          await Context.Tickets.Where(t =>
            t.OrderId.Equals(id)).ToListAsync()
        );
      }

      public async Task<IEnumerable<PassengerDF>> GetTicketsByDateFlightAsync(int dateId , string flightId) {
        return (
          await Context.Tickets.Where(t =>
            t.DateId == dateId && t.FlightId == flightId)
            .Select(t => new PassengerDF {
              Name = t.PassengerName,
              Gender = t.PassengerGender,
              LuggageId = t.LuggageId,
              Luggage = t.Luggage,
              TicketCategoryId = t.TicketCategoryId,
              TicketCategory = t.TicketCategory}).ToListAsync()
        );
      }
    }
}