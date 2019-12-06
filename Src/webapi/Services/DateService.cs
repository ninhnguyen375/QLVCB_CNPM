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
    public class DateService : ControllerBase, IDateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DateService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public async Task<ActionResult> GetDatesAsync(Pagination pagination, SearchDate search) {
          // Mapping: Date
          var datesSource = await _unitOfWork.Dates.GetAllAsync();
          await _unitOfWork.Dates.GetDateFlightsAsync();
          var dates = _mapper.Map<IEnumerable<Date>, IEnumerable<DateDTO>>(datesSource);

          // Search by DepartureDate
          if (search.DepartureDate != "") {
            DateTime departureDate = Convert.ToDateTime(search.DepartureDate);

            dates = dates.Where(d =>
              d.DepartureDate == departureDate);
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            dates = dates.OrderBy(d =>
              d.GetType().GetProperty(search.sortAsc).GetValue(d));
          }
          
          // Sort Desc:
          if (search.sortDesc != "") {
            dates = dates.OrderByDescending(d =>
              d.GetType().GetProperty(search.sortDesc).GetValue(d));
          }

          // Default order newest departureDate
          dates = dates.OrderByDescending(d =>
              d.DepartureDate);
          
          return Ok (PaginatedList<DateDTO>.Create(dates, pagination.current, pagination.pageSize));
        }
        
        public async Task<ActionResult> GetDateAsync(int id) {
          // Mapping: Date
          var dateSource = await _unitOfWork.Dates.GetByAsync(id);
          await _unitOfWork.Dates.GetDateFlightsAsync();
          var date = _mapper.Map<Date, DateDTO>(dateSource);

          if (date == null) {
            return NotFound (new  { Id = "Mã ngày này không tồn tại." });
          }

          return Ok (new { success = true, data = date });
        }
        public async Task<ActionResult> UpdateDateAsync(int id, SaveDateDTO saveDateDTO) {
          var date = await _unitOfWork.Dates.GetByAsync(id);

          // Check date exists
          if (date == null) {
            return NotFound (new  { Id = "Mã ngày này không tồn tại." });
          }

          // Check departureDate of date exists except self
          var dateExist = await _unitOfWork.Dates.FindAsync(d =>
            d.DepartureDate == Convert.ToDateTime(saveDateDTO.DepartureDate) &&
            d.Id != id);

          if (dateExist.Count() != 0 ) {
            return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." }); 
          }

          // Mapping: SaveDate
          _mapper.Map<SaveDateDTO, Date>(saveDateDTO, date);

          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, data = date, message = "Sửa thành công" });
        }

        public async Task<ActionResult> AddDateAsync(SaveDateDTO saveDateDTO) {
          // Mapping: SaveDate
          var date = _mapper.Map<SaveDateDTO, Date>(saveDateDTO);
          
          DateTime departureDate = Convert.ToDateTime(date.DepartureDate);

          // Check date exists
          var dateTemp = await _unitOfWork.Dates.FindAsync(d =>
            d.DepartureDate == departureDate);

          if (dateTemp.Count() > 0) {
            return BadRequest (new { DepartureDate = "Ngày khởi hành này đã tồn tại." });
          }

          // Nếu có thêm các chuyến bay cho ngày
          if (date.DateFlights != null) {
            var flights = await _unitOfWork.Flights.GetAllAsync();

            // Thêm ghế còn lại và trạng thái cho chuyến bay
            foreach (var dateFlight in date.DateFlights) {
              dateFlight.SeatsLeft = flights.Where(f =>
                f.Id == dateFlight.FlightId)
                .Select(f => f.SeatsCount)
                .SingleOrDefault();
              dateFlight.Status = 1; // Còn chỗ
            }
          }

          await _unitOfWork.Dates.AddAsync(date);
          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Thêm thành công." });
        }

        public async Task<ActionResult> DeleteDateAsync(int id) {
          var date = await _unitOfWork.Dates.GetByAsync(id);

          // Check date exists
          if (date == null) {
            return NotFound (new  { Id = "Mã ngày này không tồn tại." });
          }
        
          // Xóa các chuyến bay trong ngày bị xóa
          var dateFlights = await _unitOfWork.DateFlights.GetAllAsync();
          foreach (var dateFlight in dateFlights) {
            if (dateFlight.DateId == id) {
              await _unitOfWork.DateFlights.RemoveAsync(dateFlight);
            }
          }

          await _unitOfWork.Dates.RemoveAsync(date);
          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Xóa thành công" });
        }

        public async Task<ActionResult> AddDateFlightAsync(int id, AddDateFlight values) {
          var date = await _unitOfWork.Dates.GetByAsync(id);

          // Check date exists
          if (date == null) {
            return NotFound (new  { Id = "Mã ngày này không tồn tại." });
          }

          var flights = await _unitOfWork.Flights.GetAllAsync();

          // Thêm thông tin cho chuyến bay: gồm ngày, ghế còn lại, trạng thái
          foreach (var dateFlight in values.DateFlights) {
            if(await _unitOfWork.Dates.GetDateFlightAsync(id, dateFlight.FlightId) == null) {
              // Mapping: SaveDateFlight
              SaveDateFlightDTO saveDateFlightDTO = new SaveDateFlightDTO {
                FlightId = dateFlight.FlightId,
                DateId = id,
                SeatsLeft =  flights.Where(f =>
                  f.Id == dateFlight.FlightId)
                  .Select(f => f.SeatsCount)
                  .SingleOrDefault(),
                Status = 1, // Còn chỗ
              };
              var dateFlight1 = _mapper.Map<SaveDateFlightDTO, DateFlight>(saveDateFlightDTO);
              await _unitOfWork.DateFlights.AddAsync(dateFlight1);
            } else {
              // Nếu chuyến bay đã tồn tại trong ngày thì báo tồn tại
              return BadRequest(new { success = false, message = "Chuyến bay đã tồn tại trong ngày." });
            }
          }

          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Thêm thành công." });
        }

        public async Task<ActionResult> DeleteFlightAsync(int id, RemoveFlight values) {
          var date = await _unitOfWork.Dates.GetByAsync(id);

          // Check date exists
          if (date == null) {
            return NotFound (new  { Id = "Mã ngày này không tồn tại." });
          }
        
          var flightAsync = await _unitOfWork.Flights.FindAsync(a =>
              a.Id.ToLower().Equals(values.FlightId.ToLower()));
          
          // Kiểm tra chuyến bay có tồn tại hay không
          var flight = flightAsync.SingleOrDefault();

          if (flight == null) {
            return NotFound (new { Id = "Mã chuyến bay này không tồn tại." });
          }

          // Kiểm tra chuyến bay này trong ngày đã được bán chưa
          var seatsCountAsync = await _unitOfWork.Flights.FindAsync(f =>
            f.Id.ToLower().Equals(values.FlightId.ToLower()));

          var seatsCount = seatsCountAsync.Select(f => f.SeatsCount).SingleOrDefault();

          var flightTempAsync = await _unitOfWork.DateFlights.FindAsync(df =>
            df.DateId == id &&
            df.FlightId.ToLower().Equals(values.FlightId.ToLower()) &&
            df.SeatsLeft == seatsCount); // Chưa bán vé nào
          
          var flightTemp = flightTempAsync.SingleOrDefault();

          if (flightTemp == null) {
            return BadRequest (new { SeatsLeft = "Không thể xóa vì loại vé của chuyến bay này đã được bán." });
          }

          // Xóa chuyến bay được chọn
          var dateFlightAsync = await _unitOfWork.DateFlights.FindAsync(df =>
            df.DateId == id &&
            df.FlightId.ToLower().Equals(values.FlightId.ToLower()));
          
          var dateFlight = dateFlightAsync.SingleOrDefault();

          await _unitOfWork.DateFlights.RemoveAsync(dateFlight);
          await _unitOfWork.CompleteAsync();

          return Ok (new { success = true, message = "Xóa thành công" });
        }

        public async Task<ActionResult> SearchFlightsAsync(SearchFlightFE values) {
          var departureDate = Convert.ToDateTime(values.DepartureDate);

          // Mapping để lấy thông tin
          // Flights:
          await _unitOfWork.Airlines.GetAllAsync();
          await _unitOfWork.Airports.GetAllAsync();
          await _unitOfWork.TicketCategories.GetAllAsync();
          await _unitOfWork.Flights.GetFlightTicketCategoriesAsync();
          var flights = _mapper.Map<IEnumerable<Flight>, IEnumerable<FlightDTO>>(await _unitOfWork.Flights.GetAllAsync());

          // DateFlights:
          var dateFlights = _mapper.Map<IEnumerable<DateFlight>, IEnumerable<DateFlightDTO>>(await _unitOfWork.DateFlights.GetAllAsync());

          // Dates:
          var dates = _mapper.Map<IEnumerable<Date>, IEnumerable<DateDTO>>(await _unitOfWork.Dates.GetAllAsync());

          // Total Passengers:
          int totalSeats= 0;
          foreach (var passenger in values.TicketCategories) {
            if (passenger.Id != 3) { // *Gán cứng*: 3 là em bé nên ko tính số ghế
              totalSeats += passenger.Quantity;
            }   
          }

          // Search Departure Flights:
          var departureFlights = (
            from f in flights 
            from df in dateFlights
            from d in dates
            where f.Id.Equals(df.FlightId) &&
                  d.Id.Equals(df.DateId) &&
                  d.DepartureDate == departureDate &&
                  f.AirportFrom.Equals(values.AirportFrom) &&
                  f.AirportTo.Equals(values.AirportTo) &&
                  f.Status == 1 && // 1 => Chuyến bay đang hoạt động
                  df.SeatsLeft >= totalSeats // Số ghế trống phải >= Số hành khách đăng ký
            select f
          );

          // Search Return Flights:
          IEnumerable<FlightDTO> returnFlights = null;
          if (values.ReturnDate != "") {
            var returnDate = Convert.ToDateTime(values.ReturnDate);
            returnFlights = (
              from f in flights 
              from df in dateFlights
              from d in dates
              where f.Id.Equals(df.FlightId) &&
                    d.Id.Equals(df.DateId) &&
                    d.DepartureDate == returnDate &&
                    f.AirportFrom.Equals(values.AirportTo) && // Đổi vị trí From và To cho chiều về
                    f.AirportTo.Equals(values.AirportFrom) &&
                    f.Status == 1 && // 1 => Chuyến bay đang hoạt động
                    df.SeatsLeft >= totalSeats
              select f
            );
          } else {
            return Ok (new { success = true, departureFlights = departureFlights });
          }

          return Ok (new { success = true, DepartureFlights = departureFlights, ReturnFlights = returnFlights });
        }

        public async Task<ActionResult> GetPassengersAsync(PassengersList values) {
          var passengers = _mapper.Map<IEnumerable<PassengerDF>, IEnumerable<PassengerDTO>>(
            await _unitOfWork.Orders.GetTicketsByDateFlightAsync(values.DateId, values.FlightId));

          return Ok (new { success = true, Passengers = passengers });
        }
    }
}