using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IDateRepository : IRepository<Date>
    {    
        IEnumerable<DateFlight> GetDateFlights();
        DateFlight GetDateFlight(int dateId, string flightId);
    }
}