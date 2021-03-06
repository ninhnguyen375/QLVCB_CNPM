using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ITicketCategoryService
    {
      Task<ActionResult> GetTicketCategoriesAsync(Pagination pagination, SearchTicketCategory search);
      Task<ActionResult> GetTicketCategoryAsync(int id);
      Task<ActionResult> UpdateTicketCategoryAsync(int id, SaveTicketCategoryDTO saveTicketCategoryDTO);
      Task<ActionResult> AddTicketCategoryAsync(SaveTicketCategoryDTO saveTicketCategoryDTO);
      Task<ActionResult> DeleteTicketCategoryAsync(int id);
    }
}