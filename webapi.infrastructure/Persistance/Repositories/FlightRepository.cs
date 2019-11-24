using System.Collections.Generic;
using webapi.core.Interfaces;
using webapi.infrastructure.Persistance.Repositories;
using webapi.core.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
      public FlightRepository(AppDbContext context) : base (context) 
      {
      }

      public async Task<IEnumerable<FlightTicketCategory>> GetFlightTicketCategoriesAsync () {
        return await AppDbContext.FlightTicketCategories.ToListAsync();
      }

      public async Task<IEnumerable<FlightTicketCategory>> GetFlightTicketCategoriesByIdAsync(string id)
      {
        return (
          await AppDbContext.FlightTicketCategories.Where(ftc =>
            ftc.FlightId.ToLower().Equals(id.ToLower())).ToListAsync()
        );
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}