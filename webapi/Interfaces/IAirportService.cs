using System.Collections.Generic;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IAirportService
    {
        IEnumerable<AirportDTO> GetAirports(Pagination pagination, SearchAirport search);
        AirportDTO GetAirport(string id);
        DataResult PutAirport(string id, SaveAirportDTO saveAirportDTO);
        DataResult PostAirport(SaveAirportDTO saveAirportDTO); 
        DataResult DeleteAirport(string id);
    }
}