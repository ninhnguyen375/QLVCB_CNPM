using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.Interfaces
{
    public interface IFlightRepository : IRepository<Flight>
    {  
        IEnumerable<FlightTicketCategory> GetFlightTicketCategories ();

        IEnumerable<FlightTicketCategory> GetFlightTicketCategoriesById (string id);
    }
}