using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ILuggageService
    {
        Task<ActionResult> GetLuggagesAsync(Pagination pagination, SearchLuggage search);
        Task<ActionResult> GetLuggageAsync(int id);
        Task<ActionResult> UpdateLuggageAsync(int id, SaveLuggageDTO saveLuggageDTO);
        Task<ActionResult> AddLuggageAsync(SaveLuggageDTO saveLuggageDTO);
        Task<ActionResult> DeleteLuggageAsync(int id);
    }
}