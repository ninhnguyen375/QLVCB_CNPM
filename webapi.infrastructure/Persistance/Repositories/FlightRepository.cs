using webapi.core.Interfaces;
using webapi.infrastructure.Persistance.Repositories;
using webapi.core.Domain.Entities;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
      public FlightRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}