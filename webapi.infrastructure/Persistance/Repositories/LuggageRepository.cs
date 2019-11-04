using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class LuggageRepository : Repository<Luggage>, ILuggageRepository
    {
      public LuggageRepository(AppDbContext context) : base (context)
      {
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}