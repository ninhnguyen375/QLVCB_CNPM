using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IAirlineService
    {
        IEnumerable<AirlineDTO> GetAirlines(Pagination pagination, SearchAirline search);
        AirlineDTO GetAirline(string id);
        DataResult PutAirline(string id, SaveAirlineDTO saveAirlineDTO);
        DataResult PostAirline(SaveAirlineDTO saveAirlineDTO); 
        DataResult DeleteAirline(string id);
    }
}