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
    public class AirportService : ControllerBase, IAirportService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirportService (IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }
        
        public async Task<ActionResult> GetAirportsAsync(Pagination pagination, SearchAirport search) {
          // Mapping: Airport
          var airportsSource = await _unitOfWork.Airports.GetAllAsync();
          var airports = _mapper.Map<IEnumerable<Airport>, IEnumerable<AirportDTO>>(airportsSource);
          
          // Search by Id:
          if (search.Id != "") {
            airports = airports.Where(a =>
              a.Id.ToLower().Contains(search.Id.ToLower()));
          }

          // Search by Name:
          if (search.Name != "") {
            airports = airports.Where(a =>
              a.Name.ToLower().Contains(search.Name.ToLower()));
          }

          // Search by Location:
          if (search.Location != "") {
            airports = airports.Where(a =>
              a.Location.ToLower().Contains(search.Location.ToLower()));
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            airports = airports.OrderBy(a =>
              a.GetType().GetProperty(search.sortAsc).GetValue(a));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            airports = airports.OrderByDescending(a =>
              a.GetType().GetProperty(search.sortDesc).GetValue(a));
          }

          return Ok (PaginatedList<AirportDTO>.Create (airports, pagination.current, pagination.pageSize));
        }

        public async Task<ActionResult> GetAirportAsync(string id) {
          // Mapping: Airport
          var airportSource = await _unitOfWork.Airports.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));
          var airport = _mapper.Map<Airport, AirportDTO>(airportSource.SingleOrDefault());

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }

          return Ok (new { success = true, data = airport });
        }

        public async Task<ActionResult> UpdateAirportAsync(string id, SaveAirportDTO saveAirportDTO) {
          var airportAsync = await _unitOfWork.Airports.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airport exists
          var airport = airportAsync.SingleOrDefault();

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }
          
          // Check name of airport exists except self
          var airportExist = await _unitOfWork.Airports.FindAsync(a =>
            a.Name.ToLower().Equals(saveAirportDTO.Name.ToLower()) &&
            !a.Id.ToLower().Equals(id.ToLower()));

          if (airportExist.Count() != 0) {
            return BadRequest (new  { Name = "Tên sân bay này đã tồn tại." });
          }

          // Mapping: SaveAirport
          _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO, airport); 

          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, data = airport, message = "Sửa thành công." });
        }

        public async Task<ActionResult> AddAirportAsync(SaveAirportDTO saveAirportDTO) {
          // Mapping: SaveAirport
          var airport = _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO);

          // Check id đã tồn tại trong Database chưa
          var airlineExist = await _unitOfWork.Airports.FindAsync(a => 
              a.Id.ToLower().Equals(airport.Id.ToLower()));

          if(airlineExist.Count() != 0) {
            return BadRequest(new { Id = "Mã sân bay này đã tồn tại." });
          }

          // Check name đã tồn tại trong Database chưa
          airlineExist = await _unitOfWork.Airports.FindAsync(a => 
              a.Name.ToLower().Equals(airport.Name.ToLower()));

          if(airlineExist.Count() != 0) {
            return BadRequest(new { Name = "Tên sân bay này đã tồn tại." });
          }

          await _unitOfWork.Airports.AddAsync(airport);
          await _unitOfWork.CompleteAsync();

          return Ok (new { sucess = true, message = "Thêm thành công." });
        }

        public async Task<ActionResult> DeleteAirportAsync(string id) {
          var airportAsync = await _unitOfWork.Airports.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airport exists
          var airport = airportAsync.SingleOrDefault();

          if (airport == null) {
            return NotFound (new { Id = "Mã sân bay này không tồn tại." });
          }

          await _unitOfWork.Airports.RemoveAsync(airport);
          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Xóa thành công" });
        }
    }
}