using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class TicketCategoryRepository : Repository<TicketCategory>, ITicketCategoryRepository
    {
      public TicketCategoryRepository(AppDbContext context) : base (context) 
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}