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
        Task<ActionResult> GetAirlinesAsync(Pagination pagination, SearchAirline search);
        Task<ActionResult> GetAirlineAsync(string id);
        Task<ActionResult> PutAirlineAsync(string id, SaveAirlineDTO saveAirlineDTO);
        Task<ActionResult> PostAirlineAsync(SaveAirlineDTO saveAirlineDTO); 
        Task<ActionResult> DeleteAirlineAsync(string id);
    }
}