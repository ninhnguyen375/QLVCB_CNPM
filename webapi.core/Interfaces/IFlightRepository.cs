using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IFlightRepository : IRepository<Flight>
    {  
        Task<IEnumerable<FlightTicketCategory>> GetFlightTicketCategoriesAsync ();

        Task<IEnumerable<FlightTicketCategory>> GetFlightTicketCategoriesByIdAsync (string id);
    }
}