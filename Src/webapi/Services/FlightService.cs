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
    public class FlightService : ControllerBase, IFlightService
    {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public FlightService(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
      }

      public async Task<ActionResult> GetFlightsAsync(Pagination pagination, SearchFlight search) {
        // Mapping: Flight
        var flightsSource = await _unitOfWork.Flights.GetAllAsync();
        await _unitOfWork.Airlines.GetAllAsync();
        await _unitOfWork.Airports.GetAllAsync();
        await _unitOfWork.TicketCategories.GetAllAsync();
        await _unitOfWork.Flights.GetFlightTicketCategoriesAsync();
        var flights = _mapper.Map<IEnumerable<Flight>, IEnumerable<FlightDTO>>(flightsSource);

        // Search by Id:
        if (search.Id != "") {
          flights = flights.Where(f =>
            f.Id.ToLower().Contains(search.Id.ToLower()));
        }

        // Search by StartTime:
        if (search.StartTime != null) {
          flights = flights.Where(f =>
            f.StartTime == search.StartTime);
        }

        /// Search by FlightTime:
        if (search.FlightTime != null) {
          flights = flights.Where(f =>
            f.FlightTime == search.FlightTime);
        }

        // Search by AirportFrom:
        if (search.AirportFrom != "") {
          flights = flights.Where(f =>
            f.AirportFrom.ToLower().Contains(search.AirportFrom.ToLower()));
        }

        // Search by AirportTo:
        if (search.AirportTo != "") {
          flights = flights.Where(f =>
            f.AirportTo.ToLower().Contains(search.AirportTo.ToLower()));
        }

        // Search by AirlineName:
        if (search.AirlineName != "") {
          var airlines = await _unitOfWork.Airlines.GetAllAsync();

          flights = (from f in flights
                     from a in airlines
                     where f.AirlineId.Equals(a.Id) && 
                     a.Name.ToLower().Contains(search.AirlineName.ToLower())
                     select f);
        }

        // Search by Status:
        if (search.Status != null) {
          flights = flights.Where(f =>
            f.Status == search.Status);
        }

        // Sort Asc:
        if (search.sortAsc != "") {
          flights = flights.OrderBy(f =>
            f.GetType().GetProperty(search.sortAsc).GetValue(f));
        }

        // Sort Desc:
        if (search.sortDesc != "") {
          flights = flights.OrderByDescending(f =>
            f.GetType().GetProperty(search.sortDesc).GetValue(f));
        }

        return Ok (PaginatedList<FlightDTO>.Create(flights, pagination.current, pagination.pageSize));
      }

      public async Task<ActionResult> GetFlightAsync(string id) {
        // Mapping: Flight
        var flightSource = await _unitOfWork.Flights.GetByAsync(id);
        await _unitOfWork.Airlines.GetAllAsync();
        await _unitOfWork.Airports.GetAllAsync();
        await _unitOfWork.TicketCategories.GetAllAsync();
        await _unitOfWork.Flights.GetFlightTicketCategoriesAsync();
        var flight = _mapper.Map<Flight, FlightDTO>(flightSource);

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        return Ok (new { success = true, data = flight });
      }

      public async Task<ActionResult> UpdateFlightAsync(string id, SaveFlightDTO values) {
        var flight = await _unitOfWork.Flights.GetByAsync(id);

        // Check flight exists
        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        // Check id of flight
        if (id != values.Id) {
          return BadRequest( new { id = "Mã chuyến bay không hợp lệ." });
        }

        // Create SaveFlightDTO
        SaveFlightDTO saveFlightDTO = new SaveFlightDTO {
          Id = values.Id,
          StartTime = values.StartTime,
          FlightTime = values.FlightTime,
          AirportFrom = values.AirportFrom,
          AirportTo = values.AirportTo,
          SeatsCount = values.SeatsCount,
          AirlineId = values.AirlineId,
          Status = values.Status
        };

        // Mapping: SaveAirport
        _mapper.Map<SaveFlightDTO, Flight>(saveFlightDTO, flight);

        // Mapping: SaveFlightTicketCategory
        foreach (var val in values.FlightTicketCategories) {
          var flightTicketCategoryAsync = await _unitOfWork.Flights
            .GetFlightTicketCategoriesByIdAsync(values.Id);

          var flightTicketCategory = flightTicketCategoryAsync
            .Where(ftc => ftc.TicketCategoryId == val.TicketCategoryId).SingleOrDefault();

          SaveFlightTicketCategoryDTO save = new SaveFlightTicketCategoryDTO {
            FlightId = values.Id,
            TicketCategoryId = val.TicketCategoryId,
            Price = val.Price,
          };
          _mapper.Map<SaveFlightTicketCategoryDTO, FlightTicketCategory>(save, flightTicketCategory);
        }

        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, message = "Sửa thành công" });
      }

      public async Task<ActionResult> AddFlightAsync(SaveFlightDTO saveFlightDTO) {
        // Mapping: SaveFlightDTO
        var flight = _mapper.Map<SaveFlightDTO, Flight>(saveFlightDTO);

        var flightTempAsync = await _unitOfWork.Flights.FindAsync(f =>
          f.Id.ToLower().Equals(flight.Id.ToLower()));

        // Kiểm tra mã chuyến bay đã tồn tại hay chưa
        var flightTemp = flightTempAsync.SingleOrDefault();

        if (flightTemp != null) {
          return BadRequest (new { Id = "Mã chuyến bay này đã tồn tại." });
        }

        await _unitOfWork.Flights.AddAsync(flight);
        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, message = "Thêm thành công." });
      }

      public async Task<ActionResult> DeleteFlightAsync(string id) {
        var flightAsync = await _unitOfWork.Flights.FindAsync(f =>
          f.Id.ToLower().Equals(id.ToLower()));

        // Kiểm tra mã chuyến bay
        var flight = flightAsync.SingleOrDefault();

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        await _unitOfWork.Flights.RemoveAsync(flight);
        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, message = "Xóa thành công." });
      }

      public async Task<ActionResult> AddFlightTicketCategoriesAsync(string id, SaveFlightTicketCategoryDTO values) {
        var flightAsync = await _unitOfWork.Flights.FindAsync(f =>
          f.Id.ToLower().Equals(id.ToLower()));
        
        // Kiểm tra mã chuyến bay
        var flight = flightAsync.SingleOrDefault();

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        var flightTicketCategoryAsync = await _unitOfWork.Flights
          .GetFlightTicketCategoriesByIdAsync(id);
        
        // Kiểm tra mã loại vé đã tồn tại hay chưa
        var flightTicketCategory = flightTicketCategoryAsync
          .Where(ftc => ftc.TicketCategoryId == values.TicketCategoryId).SingleOrDefault();

        if (flightTicketCategory != null) {
          return BadRequest (new { Id = "Loại vé của chuyến bay này đã tồn tại." });
        }

        // Mapping: SaveFlightTicketCategory
        flightTicketCategory = _mapper.Map<SaveFlightTicketCategoryDTO, FlightTicketCategory>(values);

        await _unitOfWork.FlightTicketCategories.AddAsync(flightTicketCategory);
        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, message = "Thêm loại vé thành công." });
      }

      public async Task<ActionResult> DeleteFlightTicketCategoriesAsync(string id, RemoveFlightTicketCategory values) {
        var flightAsync = await _unitOfWork.Flights.FindAsync(f =>
          f.Id.ToLower().Equals(id.ToLower()));
        
        // Kiểm tra mã chuyến bay
        var flight = flightAsync.SingleOrDefault();

        if (flight == null) {
          return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
        }

        var flightTicketCategoryAsync = await _unitOfWork.Flights
          .GetFlightTicketCategoriesByIdAsync(id);

        // Kiểm tra mã loại vé
        var flightTicketCategory = flightTicketCategoryAsync
          .Where(ftc => ftc.TicketCategoryId == values.TicketCategoryId).SingleOrDefault();

        if (flightTicketCategory == null) {
          return BadRequest (new { Id = "Loại vé của chuyến bay này không tồn tại." });
        }

        await _unitOfWork.FlightTicketCategories.RemoveAsync(flightTicketCategory);
        await _unitOfWork.CompleteAsync();

        return Ok (new { success = true, message = "Xóa thành công." });
      }
    }
}