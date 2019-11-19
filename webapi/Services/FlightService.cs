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
    public class FlightService : IFlightService
    {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IMapper _mapper;

      public FlightService(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
      }

      public IEnumerable<FlightDTO> GetFlights(Pagination pagination, SearchFlight search) {
        // Mapping: Flight
        var flightsSource = _unitOfWork.Flights.GetAll();
        _unitOfWork.Airlines.GetAll();
        _unitOfWork.Airports.GetAll();
        _unitOfWork.TicketCategories.GetAll();
        _unitOfWork.Flights.GetFlightTicketCategories();
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
          var airlines = _unitOfWork.Airlines.GetAll();

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

        return flights;
      }

      public FlightDTO GetFlight(string id) {
        // Mapping: Flight
        var flightSource = _unitOfWork.Flights.GetBy(id);
        _unitOfWork.Airlines.GetAll();
        _unitOfWork.Airports.GetAll();
        _unitOfWork.TicketCategories.GetAll();
        _unitOfWork.Flights.GetFlightTicketCategories();
        var flight = _mapper.Map<Flight, FlightDTO>(flightSource);

        return flight;
      }

      public DataResult PutFlight(string id, SaveFlightDTO values) {
        var flight = _unitOfWork.Flights.GetBy(id);

        if (flight == null) {
          return new DataResult { Error = 1 };
        }

        if(id != values.Id) {
          return new DataResult { Error = 2 };
        }

        // Create SaveFlightDTO
        // SaveFlightDTO saveFlightDTO = new SaveFlightDTO {
        //   Id = values.Id,
        //   StartTime = values.StartTime,
        //   FlightTime = values.FlightTime,
        //   AirportFrom = values.AirportFrom,
        //   AirportTo = values.AirportTo,
        //   SeatsCount = values.SeatsCount,
        //   AirlineId = values.AirlineId
        // };

        // Mapping: SaveAirport
        _mapper.Map<SaveFlightDTO, Flight>(values, flight);

        // Mapping: SaveFlightTicketCategory
        // foreach (var val in values.FlightTicketCategories) {
        //   var flightTicketCategory = _unitOfWork.FlightTicketCategories.Find(ftc =>
        //     ftc.FlightId == values.Id &&
        //     ftc.TicketCategoryId == val.TicketCategoryId).SingleOrDefault();
        //   SaveFlightTicketCategoryDTO save = new SaveFlightTicketCategoryDTO {
        //     FlightId = values.Id,
        //     TicketCategoryId = val.TicketCategoryId,
        //     Price = val.Price,
        //   };
        //   _mapper.Map<SaveFlightTicketCategoryDTO, FlightTicketCategory>(save, flightTicketCategory);
        // }

        _unitOfWork.Complete();

        return new DataResult { };
      }

      public DataResult PostFlight(SaveFlightDTO saveFlightDTO) {
        // Mapping: SaveFlightDTO
        var flight = _mapper.Map<SaveFlightDTO, Flight>(saveFlightDTO);

        var flightTemp = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(flight.Id.ToLower())).SingleOrDefault();

        if (flightTemp != null) {
          return new DataResult { Error = 1 };
        }

        _unitOfWork.Flights.Add(flight);
        _unitOfWork.Complete();

        return new DataResult { };
      }

      public DataResult DeleteFlight(string id) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

        if (flight == null) {
          return new DataResult { Error = 1 };
        }

        _unitOfWork.Flights.Remove(flight);
        _unitOfWork.Complete();

        return new DataResult { };
      }

      public DataResult PostFlightTicketCategories(string id, SaveFlightTicketCategoryDTO values) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
        
        if (flight == null) {
          return new DataResult { Error = 1 };
        }

        var flightTicketCategory = _unitOfWork.FlightTicketCategories.Find(ftc =>
          ftc.FlightId.ToLower().Equals(id.ToLower()) &&
          ftc.TicketCategoryId == values.TicketCategoryId).SingleOrDefault();

        if (flightTicketCategory != null) {
          return new DataResult { Error = 2 };
        }

        // Mapping: SaveFlightTicketCategory
        flightTicketCategory = _mapper.Map<SaveFlightTicketCategoryDTO, FlightTicketCategory>(values);

        _unitOfWork.FlightTicketCategories.Add(flightTicketCategory);
        _unitOfWork.Complete();

        return new DataResult { };
      }

      public DataResult DeleteFlightTicketCategories(string id, RemoveFlightTicketCategory values) {
        var flight = _unitOfWork.Flights.Find(f =>
          f.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
        
        if (flight == null) {
          return new DataResult { Error = 1 };
        }

        var flightTicketCategory = _unitOfWork.FlightTicketCategories.Find(ftc =>
          ftc.FlightId.ToLower().Equals(id.ToLower()) &&
          ftc.TicketCategoryId == values.TicketCategoryId).SingleOrDefault();

        if (flightTicketCategory == null) {
          return new DataResult { Error = 2 };
        }

        _unitOfWork.FlightTicketCategories.Remove(flightTicketCategory);
        _unitOfWork.Complete();

        return new DataResult { };
      }
    }
}