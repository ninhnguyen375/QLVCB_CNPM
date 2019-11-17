using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Domain.Entities;
using webapi.core.DTOs;
using webapi.core.Interfaces;
using webapi.core.UseCases;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class LuggagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LuggagesController(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        // GET: api/luggages
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetLuggages([FromQuery] Pagination pagination, [FromQuery] SearchLuggage search) {
          // Mapping: Luggage
          var luggagesSource = _unitOfWork.Luggages.GetAll();
          var luggages = _mapper.Map<IEnumerable<Luggage>, IEnumerable<LuggageDTO>>(luggagesSource);
          
          // Search by LuggageWeight:
          if (search.LuggageWeight != null) {
            luggages = luggages.Where(l =>
              l.LuggageWeight == search.LuggageWeight);
          }

          // Search by Price:
          if (search.PriceFrom != null && search.PriceTo != null) {
            luggages = luggages.Where(l =>
              l.Price >= search.PriceFrom &&
              l.Price <= search.PriceTo);
          } else if (search.PriceFrom != null && search.PriceTo == null) {
            luggages = luggages.Where(l =>
              l.Price >= search.PriceFrom);
          } else if (search.PriceFrom == null && search.PriceTo != null) {
            luggages = luggages.Where(l =>
              l.Price <= search.PriceTo);
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            luggages = luggages.OrderBy(l =>
              l.GetType().GetProperty(search.sortAsc).GetValue(l));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            luggages = luggages.OrderByDescending(l =>
              l.GetType().GetProperty(search.sortDesc).GetValue(l));
          }

          return Ok (PaginatedList<LuggageDTO>.Create(luggages, pagination.current, pagination.pageSize));
        }

        // GET: api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpGet ("{id}")]
        public ActionResult GetLuggage(int id) {
          // Mapping: Luggage
          var luggageSource = _unitOfWork.Luggages.GetBy(id);
          var luggage = _mapper.Map<Luggage, LuggageDTO>(luggageSource);

          if (luggage == null) {
            return NotFound (new { Id = "Mã hành lý này không tồn tại." });
          }

          return Ok (new { success = true, data = luggage });
        }

        // PUT: api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPut ("{id}")]
        public ActionResult PutLuggage(int id, SaveLuggageDTO saveLuggageDTO) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return NotFound (new { Id = "Mã hành lý này không tồn tại." });
          }

          if (_unitOfWork.Luggages.Find(l =>
                l.LuggageWeight == saveLuggageDTO.LuggageWeight &&
                l.Id != id)
                .Count() != 0 ) {
            return BadRequest (new { LuggageWeight = "Khối lượng hành lý đã được thiết lập" });
          }

          // Mapping: SaveLuggage
          _mapper.Map<SaveLuggageDTO, Luggage>(saveLuggageDTO, luggage);
          
          _unitOfWork.Complete();

          return Ok (new { success = true, data = luggage, message = "Sửa thành công." });
        }

        // POST: api/luggages
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpPost]
        public ActionResult PostLuggage(SaveLuggageDTO saveLuggageDTO) {
          // Mapping: SaveLuggage
          var luggage = _mapper.Map<SaveLuggageDTO, Luggage>(saveLuggageDTO);

          if(_unitOfWork.Luggages.Find(l => 
                l.LuggageWeight.Equals(luggage.LuggageWeight))
                .Count() != 0) {
            return BadRequest(new {
                LuggageWeight = "Khối lượng hành lý đã được thiết lập"
            });
          }

          _unitOfWork.Luggages.Add(luggage);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Thêm thành công" });
        }

        // DELETE : api/luggages/1
        [Authorize (Roles = "STAFF, ADMIN")]
        [HttpDelete ("{id}")]
        public ActionResult DeleteLuggage(int id) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return NotFound (new { message = "Mã hành lý này không tồn tại." });
          }

          _unitOfWork.Luggages.Remove(luggage);
          _unitOfWork.Complete();

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}