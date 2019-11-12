using System.Collections.Generic;
using webapi.core.Interfaces;
using webapi.infrastructure.Persistance.Repositories;
using webapi.core.Domain.Entities;
using System.Linq;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
      public FlightRepository(AppDbContext context) : base (context) 
      {
      }

      public IEnumerable<FlightTicketCategory> GetFlightTicketCategories () {
        return AppDbContext.FlightTicketCategories.ToList();
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}