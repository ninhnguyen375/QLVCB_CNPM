using webapi.core.Interfaces;
using webapi.infrastructure.Persistance.Repositories;
using webapi.core.Domain.Entities;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class AirportRepository : Repository<Airport>, IAirportRepository
    {
      public AirportRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}