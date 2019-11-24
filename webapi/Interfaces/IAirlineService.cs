using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IAirlineService
    {
        Task<IEnumerable<AirlineDTO>> GetAirlinesAsync(Pagination pagination, SearchAirline search);
        Task<AirlineDTO> GetAirlineAsync(string id);
        Task<DataResult> PutAirlineAsync(string id, SaveAirlineDTO saveAirlineDTO);
        Task<DataResult> PostAirlineAsync(SaveAirlineDTO saveAirlineDTO); 
        Task<DataResult> DeleteAirlineAsync(string id);
    }
}