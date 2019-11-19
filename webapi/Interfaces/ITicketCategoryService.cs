using System.Collections.Generic;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ITicketCategoryService
    {
      IEnumerable<TicketCategoryDTO> GetTicketCategories(Pagination pagination, SearchTicketCategory search);
      TicketCategoryDTO  GetTicketCategory(int id);
      DataResult PutTicketCategory(int id, SaveTicketCategoryDTO saveTicketCategoryDTO);
      DataResult PostTicketCategory(SaveTicketCategoryDTO saveTicketCategoryDTO);
      DataResult DeleteTicketCategory(int id);
    }
}