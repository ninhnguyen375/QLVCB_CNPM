using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;

namespace webapi.Services
{
    public class TicketCategoryService : ITicketCategoryService
    {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;
      
      public TicketCategoryService(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
      }

      public async Task<IEnumerable<TicketCategoryDTO>> GetTicketCategoriesAsync(Pagination pagination, SearchTicketCategory search) {
        // Mapping: TicketCategory
        var ticketCategoriesSource = await _unitOfWork.TicketCategories.GetAllAsync();
        var ticketCategories = _mapper.Map<IEnumerable<TicketCategory>, IEnumerable<TicketCategoryDTO>>(ticketCategoriesSource);

        // Search by Name:
        if (search.Name != "") {
          ticketCategories = ticketCategories.Where(tc => 
            tc.Name.ToLower().Contains(search.Name.ToLower()));
        }

        // Sort Asc:
        if (search.sortAsc != "") {
          ticketCategories = ticketCategories.OrderBy(tc => 
            tc.GetType().GetProperty(search.sortAsc).GetValue(tc));
        }

        // Sort Desc:
        if (search.sortDesc != "") {
          ticketCategories = ticketCategories.OrderByDescending(tc => 
            tc.GetType().GetProperty(search.sortDesc).GetValue(tc));
        }

        return ticketCategories;
      }

      public async Task<TicketCategoryDTO>  GetTicketCategoryAsync(int id) {
        // Mapping: TicketCategory
        var ticketCategorySource = await _unitOfWork.TicketCategories.GetByAsync(id);
        var ticketCategory = _mapper.Map<TicketCategory, TicketCategoryDTO>(ticketCategorySource);

        return ticketCategory;
      }

      public async Task<DataResult> PutTicketCategoryAsync(int id, SaveTicketCategoryDTO saveTicketCategoryDTO) {
        // Check ticketCategory exists
        var ticketCategory = await _unitOfWork.TicketCategories.GetByAsync(id);

        if (ticketCategory == null) {
          return new DataResult { Error = 1 };
        }

        // Check name of ticketCategory exists except self
        var ticketCategoryExist = await _unitOfWork.TicketCategories.FindAsync(tc =>
          tc.Name.ToLower().Equals(saveTicketCategoryDTO.Name.ToLower()) &&
          tc.Id != id);

        if (ticketCategoryExist.Count() != 0) {
          return new DataResult { Error = 2 };
        }

        // Mapping: SaveTicketCategory
        _mapper.Map<SaveTicketCategoryDTO, TicketCategory>(saveTicketCategoryDTO, ticketCategory);

        await _unitOfWork.CompleteAsync();

        return new DataResult { Data = ticketCategory };
      }

      public async Task<DataResult> PostTicketCategoryAsync(SaveTicketCategoryDTO saveTicketCategoryDTO) {
        // Check ticketCategory exists
        var ticketCategoryExist = await _unitOfWork.TicketCategories.FindAsync(tc => 
              tc.Name.ToLower().Equals(saveTicketCategoryDTO.Name.ToLower()));

        if (ticketCategoryExist.Count() != 0) {
          return new DataResult { Error = 1 };
        }

        // Mapping: SaveTicketCategory
        var ticketCategory = _mapper.Map<SaveTicketCategoryDTO, TicketCategory>(saveTicketCategoryDTO);

        await _unitOfWork.TicketCategories.AddAsync(ticketCategory);
        await _unitOfWork.CompleteAsync();

        return new DataResult { };
      }

      public async Task<DataResult> DeleteTicketCategoryAsync(int id) {
        var ticketCategory = await _unitOfWork.TicketCategories.GetByAsync(id);

        if (ticketCategory == null) {
          return new DataResult { Error = 1 };
        }

        await _unitOfWork.TicketCategories.RemoveAsync(ticketCategory);
        await _unitOfWork.CompleteAsync();

        return new DataResult { };
      }
  }
}