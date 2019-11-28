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
    public class AirlineService : ControllerBase, IAirlineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirlineService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }
        
        public async Task<ActionResult> GetAirlinesAsync([FromQuery] Pagination pagination, [FromQuery] SearchAirline search) {
          // Mapping: Airline
          var airlinesSource = await _unitOfWork.Airlines.GetAllAsync();
          var airlines = _mapper.Map<IEnumerable<Airline>, IEnumerable<AirlineDTO>>(airlinesSource);

          // Search by Id:
          if (search.Id != "") {
            airlines = airlines.Where(a =>
              a.Id.ToLower().Contains(search.Id.ToLower()));
          }

          // Search by Name:
          if (search.Name != "") {
            airlines = airlines.Where(a =>
            a.Name.ToLower().Contains(search.Name.ToLower()));
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            airlines = airlines.OrderBy(a => 
              a.GetType().GetProperty(search.sortAsc).GetValue(a));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            airlines = airlines.OrderByDescending(a =>
              a.GetType().GetProperty(search.sortDesc).GetValue(a));
          }

          return Ok (PaginatedList<AirlineDTO>.Create(airlines, pagination.current, pagination.pageSize));
        }

        public async Task<ActionResult> GetAirlineAsync(string id) {
          // Mapping: Airline
          var airlineSource = await _unitOfWork.Airlines.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));
          var airline = _mapper.Map<Airline, AirlineDTO>(airlineSource.SingleOrDefault());

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }

          return Ok (new { success = true, data = airline });
        }

        public async Task<ActionResult> UpdateAirlineAsync(string id, SaveAirlineDTO saveAirlineDTO) {
          var airlineAsync = await _unitOfWork.Airlines.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airline exists
          var airline = airlineAsync.SingleOrDefault();

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }
          
          // Check name of airline exists except self
          var airlineExist = await _unitOfWork.Airlines.FindAsync(a =>
            a.Name.ToLower().Equals(saveAirlineDTO.Name.ToLower()) &&
            !a.Id.ToLower().Equals(id.ToLower()));

          if (airlineExist.Count() != 0) {
            return BadRequest (new { Name = "Tên hãng hàng không này đã tồn tại." });
          }

          // Mapping: SaveAirline
          _mapper.Map<SaveAirlineDTO, Airline>(saveAirlineDTO, airline);

          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, data = airline, message = "Sửa thành công." });
        }

        public async Task<ActionResult> AddAirlineAsync(SaveAirlineDTO saveAirlineDTO) {
          // Mapping: SaveAirline
          var airline = _mapper.Map<SaveAirlineDTO, Airline>(saveAirlineDTO);

          // Check id đã tồn tại trong Database chưa
          var airlineExist = await _unitOfWork.Airlines.FindAsync(a => 
            a.Id.ToLower().Equals(airline.Id.ToLower()));

          if(airlineExist.Count() != 0) {
            return BadRequest (new { Id = "Mã hãng hàng không này đã tồn tại." });
          }

          // Check name đã tồn tại trong Database chưa
          airlineExist = await _unitOfWork.Airlines.FindAsync(a => 
            a.Name.ToLower().Equals(airline.Name.ToLower()));

          if(airlineExist.Count() != 0) {
            return BadRequest(new { Name = "Tên hãng hàng không này đã tồn tại." });
          }

          await _unitOfWork.Airlines.AddAsync(airline);
          await _unitOfWork.CompleteAsync();

          return Ok (new { sucess = true, message = "Thêm thành công." });
        }

        public async Task<ActionResult> DeleteAirlineAsync(string id) {
          var airlineAsync = await _unitOfWork.Airlines.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airline exists
          var airline = airlineAsync.SingleOrDefault();

          if (airline == null) {
            return NotFound (new { Id = "Mã hãng hàng không này không tồn tại." });
          }

          await _unitOfWork.Airlines.RemoveAsync(airline);
          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}