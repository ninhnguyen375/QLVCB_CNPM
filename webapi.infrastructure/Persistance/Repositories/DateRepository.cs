using System.Collections.Generic;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class DateRepository : Repository<Date>, IDateRepository
    {
      public DateRepository(AppDbContext context) : base (context) 
      {
      }
    
      public async Task<IEnumerable<DateFlight>> GetDateFlightsAsync() {
        return await AppDbContext.DateFlights.ToListAsync();
      }

      public async Task<DateFlight> GetDateFlightAsync(int dateId, string flightId){
        return await AppDbContext.DateFlights.SingleOrDefaultAsync(
          df => df.DateId.Equals(dateId) && 
          df.FlightId.Equals(flightId)
        );
      }

      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}