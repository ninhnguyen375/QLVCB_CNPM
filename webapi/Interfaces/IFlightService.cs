using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IFlightService
    {
      Task<ActionResult> GetFlightsAsync(Pagination pagination, SearchFlight search);
      Task<ActionResult> GetFlightAsync(string id);
      Task<ActionResult> UpdateFlightAsync(string id, SaveFlightDTO values);
      Task<ActionResult> AddFlightAsync(SaveFlightDTO saveFlightDTO);
      Task<ActionResult> DeleteFlightAsync(string id);
      Task<ActionResult> AddFlightTicketCategoriesAsync(string id, SaveFlightTicketCategoryDTO values);
      Task<ActionResult> DeleteFlightTicketCategoriesAsync(string id, RemoveFlightTicketCategory values);
    }
}