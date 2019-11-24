using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IDateService
    {
      Task<IEnumerable<DateDTO>> GetDatesAsync(Pagination pagination, SearchDate search);
      Task<DateDTO> GetDateAsync(int id);
      Task<DataResult> PutDateAsync(int id, SaveDateDTO saveDateDTO);
      Task<DataResult> PostDateAsync(SaveDateDTO saveDateDTO);
      Task<DataResult> DeleteDateAsync(int id);
      Task<DataResult> PostFlightAsync(int id, AddDateFlight values);
      Task<DataResult> DeleteFlightAsync(int id, RemoveFlight values);
      Task<DataResult> SearchFlightsAsync(SearchFlightFE values);
    }
}