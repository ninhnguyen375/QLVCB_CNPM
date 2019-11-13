using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IDateRepository : IRepository<Date>
    {    
        public IEnumerable<DateFlight> getDateFlights();
        public DateFlight getDateFlight(int dateId, string flightId);
    }
}