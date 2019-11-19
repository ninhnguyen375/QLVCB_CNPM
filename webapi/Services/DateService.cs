using System;
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
    public class DateService : IDateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DateService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public IEnumerable<DateDTO> GetDates(Pagination pagination, SearchDate search) {
          // Mapping: Date
          var datesSource = _unitOfWork.Dates.GetAll();
          _unitOfWork.Dates.GetDateFlights();
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
          
          return dates;
        }
        
        public DateDTO GetDate(int id) {
          // Mapping: Date
          var dateSource = _unitOfWork.Dates.GetBy(id);
          _unitOfWork.Dates.GetDateFlights();
          var date = _mapper.Map<Date, DateDTO>(dateSource);

          return date;
        }
        public DataResult PutDate(int id, SaveDateDTO saveDateDTO) {
          var date = _unitOfWork.Dates.GetBy(id);

          if (date == null) {
            return new DataResult { Error = 1 };
          }

          if (_unitOfWork.Dates.Find(d =>
                d.DepartureDate == Convert.ToDateTime(saveDateDTO.DepartureDate) &&
                d.Id != id)
                .Count() != 0 ) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveDate
          _mapper.Map<SaveDateDTO, Date>(saveDateDTO, date);

          _unitOfWork.Complete();

          return new DataResult { Data = date };
        }

        public DataResult PostDate(SaveDateDTO saveDateDTO) {
          // Mapping: SaveDate
          var date = _mapper.Map<SaveDateDTO, Date>(saveDateDTO);
          
          DateTime departureDate = Convert.ToDateTime(date.DepartureDate);

          var dateTemp = _unitOfWork.Dates.Find(d =>
            d.DepartureDate == departureDate);

          if (dateTemp.Count() > 0) {
            return new DataResult { Error = 1 };
          }

          if (date.DateFlights != null) {
            var flights = _unitOfWork.Flights.GetAll();

            // Thêm ghế còn lại và trạng thái cho chuyến bay
            foreach (var dateFlight in date.DateFlights) {
              dateFlight.SeatsLeft = flights.Where(f =>
                f.Id == dateFlight.FlightId)
                .Select(f => f.SeatsCount)
                .SingleOrDefault();
              dateFlight.Status = 1; // Còn chỗ
            }
          }

          _unitOfWork.Dates.Add(date);
          _unitOfWork.Complete();

          return new DataResult { };
        }

        public DataResult DeleteDate(int id) {
          var date = _unitOfWork.Dates.GetBy(id);

          if (date == null) {
            return new DataResult { Error = 1 };
          }
        
          // Xóa các chuyến bay trong ngày bị xóa
          var dateFlights = _unitOfWork.DateFlights.GetAll();
          foreach (var dateFlight in dateFlights) {
            if (dateFlight.DateId == id) {
              _unitOfWork.DateFlights.Remove(dateFlight);
            }
          }

          _unitOfWork.Dates.Remove(date);
          _unitOfWork.Complete();

          return new DataResult { };
        }

        public DataResult PostFlight(int id, AddDateFlight values) {
          var date = _unitOfWork.Dates.GetBy(id);

          if (date == null) {
            return new DataResult { Error = 1 };
          }

          var flights = _unitOfWork.Flights.GetAll();

          // Thêm thông tin cho chuyến bay: gồm ngày, ghế còn lại, trạng thái
          foreach (var dateFlight in values.DateFlights) {
            if(_unitOfWork.Dates.GetDateFlight(id, dateFlight.FlightId) == null) {
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
              _unitOfWork.DateFlights.Add(dateFlight1);
            } else {
              return new DataResult { Error = 2 };
            }
          }

          _unitOfWork.Complete();

          return new DataResult { };
        }

        public DataResult DeleteFlight(int id, RemoveFlight values) {
          var date = _unitOfWork.Dates.GetBy(id);

          if (date == null) {
            return new DataResult { Error = 1 };
          }
        
          var flight = _unitOfWork.Flights.Find(a =>
              a.Id.ToLower().Equals(values.FlightId.ToLower()))
              .SingleOrDefault();

          // Kiểm tra chuyến bay có tồn tại hay không
          if (flight == null) {
            return new DataResult { Error = 2 };
          }

          // Kiểm tra chuyến bay này trong ngày đã được bán chưa
          var seatsCount = _unitOfWork.Flights.Find(f =>
            f.Id.ToLower().Equals(values.FlightId.ToLower()))
            .Select(f => f.SeatsCount).SingleOrDefault();
          var flightTemp = _unitOfWork.DateFlights.Find(df =>
            df.DateId == id &&
            df.FlightId.ToLower().Equals(values.FlightId.ToLower()) &&
            df.SeatsLeft == seatsCount).SingleOrDefault(); // Chưa bán vé nào
          
          if (flightTemp == null) {
            return new DataResult { Error = 3 };
          }

          // Xóa chuyến bay được chọn
          var dateFlight = _unitOfWork.DateFlights.Find(df =>
            df.DateId == id &&
            df.FlightId.ToLower().Equals(values.FlightId.ToLower())).SingleOrDefault();

          _unitOfWork.DateFlights.Remove(dateFlight);
          _unitOfWork.Complete();

          return new DataResult { };
        }

        public DataResult SearchFlights(SearchFlightFE values) {
          var departureDate = Convert.ToDateTime(values.DepartureDate);

          // Flights:
          _unitOfWork.Airlines.GetAll();
          _unitOfWork.Airports.GetAll();
          _unitOfWork.TicketCategories.GetAll();
          _unitOfWork.Flights.GetFlightTicketCategories();
          var flights = _mapper.Map<IEnumerable<Flight>, IEnumerable<FlightDTO>>(_unitOfWork.Flights.GetAll());

          // DateFlights:
          var dateFlights = _mapper.Map<IEnumerable<DateFlight>, IEnumerable<DateFlightDTO>>(_unitOfWork.DateFlights.GetAll());

          // Dates:
          var dates = _mapper.Map<IEnumerable<Date>, IEnumerable<DateDTO>>(_unitOfWork.Dates.GetAll());

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
            return new DataResult { DepartureFlights = departureFlights };
          }

          return new DataResult { DepartureFlights = departureFlights, ReturnFlights = returnFlights };
        }
    }
}