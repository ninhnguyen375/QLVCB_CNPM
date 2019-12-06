using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IDateService
    {
      Task<ActionResult> GetDatesAsync(Pagination pagination, SearchDate search);
      Task<ActionResult> GetDateAsync(int id);
      Task<ActionResult> UpdateDateAsync(int id, SaveDateDTO saveDateDTO);
      Task<ActionResult> AddDateAsync(SaveDateDTO saveDateDTO);
      Task<ActionResult> DeleteDateAsync(int id);
      Task<ActionResult> AddDateFlightAsync(int id, AddDateFlight values);
      Task<ActionResult> DeleteFlightAsync(int id, RemoveFlight values);
      Task<ActionResult> SearchFlightsAsync(SearchFlightFE values);
      Task<ActionResult> GetPassengersAsync(PassengersList values);
    }
}