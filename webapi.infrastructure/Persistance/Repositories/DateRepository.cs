using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class DateRepository : Repository<Date>, IDateRepository
    {
      public DateRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}