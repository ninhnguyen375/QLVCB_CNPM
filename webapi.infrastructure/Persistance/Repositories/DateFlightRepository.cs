using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class DateFlightRepository : Repository<DateFlight>, IDateFlightRepository
    {
      public DateFlightRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}