using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IFlightService
    {
      Task<IEnumerable<FlightDTO>> GetFlightsAsync(Pagination pagination, SearchFlight search);
      Task<FlightDTO> GetFlightAsync(string id);
      Task<DataResult> PutFlightAsync(string id, SaveFlightDTO values);
      Task<DataResult> PostFlightAsync(SaveFlightDTO saveFlightDTO);
      Task<DataResult> DeleteFlightAsync(string id);
      Task<DataResult> PostFlightTicketCategoriesAsync(string id, SaveFlightTicketCategoryDTO values);
      Task<DataResult> DeleteFlightTicketCategoriesAsync(string id, RemoveFlightTicketCategory values);
    }
}