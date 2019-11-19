using System.Collections.Generic;
using System.Linq;
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

      public IEnumerable<TicketCategoryDTO> GetTicketCategories(Pagination pagination, SearchTicketCategory search) {
        // Mapping: TicketCategory
        var ticketCategoriesSource = _unitOfWork.TicketCategories.GetAll();
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

      public TicketCategoryDTO  GetTicketCategory(int id) {
        // Mapping: TicketCategory
        var ticketCategorySource = _unitOfWork.TicketCategories.GetBy(id);
        var ticketCategory = _mapper.Map<TicketCategory, TicketCategoryDTO>(ticketCategorySource);

        return ticketCategory;
      }

      public DataResult PutTicketCategory(int id, SaveTicketCategoryDTO saveTicketCategoryDTO) {
        var ticketCategory = _unitOfWork.TicketCategories.GetBy(id);

        if (ticketCategory == null) {
          return new DataResult { Error = 1 };
        }

        if (_unitOfWork.TicketCategories.Find(tc =>
              tc.Name.ToLower().Equals(saveTicketCategoryDTO.Name.ToLower()) &&
              tc.Id != id)
              .Count() != 0) {
          return new DataResult { Error = 2 };
        }

        // Mapping: SaveTicketCategory
        _mapper.Map<SaveTicketCategoryDTO, TicketCategory>(saveTicketCategoryDTO, ticketCategory);

        _unitOfWork.Complete();

        return new DataResult { Data = ticketCategory };
      }

      public DataResult PostTicketCategory(SaveTicketCategoryDTO saveTicketCategoryDTO) {
        if (_unitOfWork.TicketCategories.Find(tc => 
              tc.Name.ToLower().Equals(saveTicketCategoryDTO.Name.ToLower()))
              .Count() != 0) {
          return new DataResult { Error = 1 };
        }

        // Mapping: SaveTicketCategory
        var ticketCategory = _mapper.Map<SaveTicketCategoryDTO, TicketCategory>(saveTicketCategoryDTO);

        _unitOfWork.TicketCategories.Add(ticketCategory);
        _unitOfWork.Complete();

        return new DataResult { };
      }

      public DataResult DeleteTicketCategory(int id) {
        var ticketCategory = _unitOfWork.TicketCategories.GetBy(id);

        if (ticketCategory == null) {
          return new DataResult { Error = 1 };
        }

        _unitOfWork.TicketCategories.Remove(ticketCategory);
        _unitOfWork.Complete();

        return new DataResult { };
      }
    }
}