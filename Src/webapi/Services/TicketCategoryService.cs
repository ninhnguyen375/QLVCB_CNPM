using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Interfaces;

namespace webapi.Services
{
    public class TicketCategoryService : ControllerBase, ITicketCategoryService
    {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;
      
      public TicketCategoryService(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
      }

      public async Task<ActionResult> GetTicketCategoriesAsync(Pagination pagination, SearchTicketCategory search) {
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

        return Ok (PaginatedList<TicketCategoryDTO>.Create(ticketCategories, pagination.current, pagination.pageSize));
      }

      public async Task<ActionResult>  GetTicketCategoryAsync(int id) {
        // Mapping: TicketCategory
        var ticketCategorySource = await _unitOfWork.TicketCategories.GetByAsync(id);
        var ticketCategory = _mapper.Map<TicketCategory, TicketCategoryDTO>(ticketCategorySource);

        if (ticketCategory == null) {
            return NotFound (new { Id = "Mã loại vé này không tồn tại." });
          }

        return Ok (new { success = true, data = ticketCategory });
      }

      public async Task<ActionResult> UpdateTicketCategoryAsync(int id, SaveTicketCategoryDTO saveTicketCategoryDTO) {
        // Check ticketCategory exists
        var ticketCategory = await _unitOfWork.TicketCategories.GetByAsync(id);

        if (ticketCategory == null) {
          return NotFound (new { Id = "Mã loại vé này không tồn tại." });
        }

        // Check name of ticketCategory exists except self
        var ticketCategoryExist = await _unitOfWork.TicketCategories.FindAsync(tc =>
          tc.Name.ToLower().Equals(saveTicketCategoryDTO.Name.ToLower()) &&
          tc.Id != id);

        if (ticketCategoryExist.Count() != 0) {
          return BadRequest (new { Name = "Loại vé này đã tồn tại." });
        }

        // Mapping: SaveTicketCategory
        _mapper.Map<SaveTicketCategoryDTO, TicketCategory>(saveTicketCategoryDTO, ticketCategory);

        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, data = ticketCategory, message = "Sửa thành công." });
      }

      public async Task<ActionResult> AddTicketCategoryAsync(SaveTicketCategoryDTO saveTicketCategoryDTO) {
        // Check ticketCategory exists
        var ticketCategoryExist = await _unitOfWork.TicketCategories.FindAsync(tc => 
              tc.Name.ToLower().Equals(saveTicketCategoryDTO.Name.ToLower()));

        if (ticketCategoryExist.Count() != 0) {
          return BadRequest (new { Name = "Loại vé này đã tồn tại." });
        }

        // Mapping: SaveTicketCategory
        var ticketCategory = _mapper.Map<SaveTicketCategoryDTO, TicketCategory>(saveTicketCategoryDTO);

        await _unitOfWork.TicketCategories.AddAsync(ticketCategory);
        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      public async Task<ActionResult> DeleteTicketCategoryAsync(int id) {
        var ticketCategory = await _unitOfWork.TicketCategories.GetByAsync(id);

        if (ticketCategory == null) {
          return NotFound (new { Id = "Mã loại vé này không tồn tại." });
        }

        try {
          await _unitOfWork.TicketCategories.RemoveAsync(ticketCategory);
          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Xóa thành công." });
        } catch (Exception) {
          return BadRequest (new { message = "Xóa không thành công." });
        }
      }
  }
}