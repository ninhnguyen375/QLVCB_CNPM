using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class FlightTicketCategoryRepository : Repository<FlightTicketCategory>, IFlightTicketCategoryRepository
    {
      public FlightTicketCategoryRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}