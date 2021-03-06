using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IAirportService
    {
        Task<ActionResult> GetAirportsAsync(Pagination pagination, SearchAirport search);
        Task<ActionResult> GetAirportAsync(string id);
        Task<ActionResult> UpdateAirportAsync(string id, SaveAirportDTO saveAirportDTO);
        Task<ActionResult> AddAirportAsync(SaveAirportDTO saveAirportDTO); 
        Task<ActionResult> DeleteAirportAsync(string id);
    }
}