using System.Collections.Generic;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;
using System.Linq;

namespace webapi.infrastructure.Persistance.Repositories
{
    public class DateRepository : Repository<Date>, IDateRepository
    {
      public DateRepository(AppDbContext context) : base (context) 
      {
      }
    
      public IEnumerable<DateFlight> getDateFlights() {
        return AppDbContext.DateFlights.ToList();
      }

        public DateFlight getDateFlight(int dateId, string flightId){
        return AppDbContext.DateFlights.SingleOrDefault(
          df => df.DateId.Equals(dateId) && 
          df.FlightId.Equals(flightId)
        );
      }


      protected AppDbContext AppDbContext {
        get { return Context as AppDbContext; }
      }
    }
}