using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IDateRepository : IRepository<Date>
    {    
        Task<IEnumerable<DateFlight>> GetDateFlightsAsync();
        Task<DateFlight> GetDateFlightAsync(int dateId, string flightId);
    }
}