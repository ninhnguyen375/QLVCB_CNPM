using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportDTO>> GetAirportsAsync(Pagination pagination, SearchAirport search);
        Task<AirportDTO> GetAirportAsync(string id);
        Task<DataResult> PutAirportAsync(string id, SaveAirportDTO saveAirportDTO);
        Task<DataResult> PostAirportAsync(SaveAirportDTO saveAirportDTO); 
        Task<DataResult> DeleteAirportAsync(string id);
    }
}