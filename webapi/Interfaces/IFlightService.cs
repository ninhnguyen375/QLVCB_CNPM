using System.Collections.Generic;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IFlightService
    {
      IEnumerable<FlightDTO> GetFlights(Pagination pagination, SearchFlight search);
      FlightDTO GetFlight(string id);
      DataResult PutFlight(string id, SaveFlightDTO values);
      DataResult PostFlight(SaveFlightDTO saveFlightDTO);
      DataResult DeleteFlight(string id);
      DataResult PostFlightTicketCategories(string id, SaveFlightTicketCategoryDTO values);
      DataResult DeleteFlightTicketCategories(string id, RemoveFlightTicketCategory values);
    }
}