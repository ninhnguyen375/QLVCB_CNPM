using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ITicketCategoryService
    {
      Task<IEnumerable<TicketCategoryDTO>> GetTicketCategoriesAsync(Pagination pagination, SearchTicketCategory search);
      Task<TicketCategoryDTO>  GetTicketCategoryAsync(int id);
      Task<DataResult> PutTicketCategoryAsync(int id, SaveTicketCategoryDTO saveTicketCategoryDTO);
      Task<DataResult> PostTicketCategoryAsync(SaveTicketCategoryDTO saveTicketCategoryDTO);
      Task<DataResult> DeleteTicketCategoryAsync(int id);
    }
}