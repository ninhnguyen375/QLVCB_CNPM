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
    public class AirportService : IAirportService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirportService (IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }
        
        public IEnumerable<AirportDTO> GetAirports(Pagination pagination, SearchAirport search) {
          // Mapping: Airport
          var airportsSource = _unitOfWork.Airports.GetAll();
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

          return airports;
        }

        public AirportDTO GetAirport(string id) {
          // Mapping: Airport
          var airportSource = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
          var airport = _mapper.Map<Airport, AirportDTO>(airportSource);

          return airport;
        }

        public DataResult PutAirport(string id, SaveAirportDTO saveAirportDTO) {
          var airport = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airport == null) {
            return new DataResult { Error = 1 };
          }
          
          if (_unitOfWork.Airports.Find(a =>
                a.Name.ToLower().Equals(saveAirportDTO.Name.ToLower()) &&
                !a.Id.ToLower().Equals(id.ToLower()))
                .Count() != 0) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveAirport
          _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO, airport); 

          _unitOfWork.Complete();

          return new DataResult { Data = airport };
        }

        public DataResult PostAirport(SaveAirportDTO saveAirportDTO) {
          // Mapping: SaveAirport
          var airport = _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO);

          // Check id đã tồn tại trong Database chưa
          if(_unitOfWork.Airports.Find(a => 
              a.Id.ToLower().Equals(airport.Id.ToLower()))
              .Count() != 0) {
            return new DataResult { Error = 1 };
          }

          // Check name đã tồn tại trong Database chưa
          if(_unitOfWork.Airports.Find(a => 
              a.Name.ToLower().Equals(airport.Name.ToLower()))
              .Count() != 0) {
            return new DataResult { Error = 2 };
          }

          _unitOfWork.Airports.Add(airport);
          _unitOfWork.Complete();

          return new DataResult { };
        }

        public DataResult DeleteAirport(string id) {
          var airport = _unitOfWork.Airports.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airport == null) {
            return new DataResult { Error = 1 };
          }

          _unitOfWork.Airports.Remove(airport);
          _unitOfWork.Complete();

          return new DataResult { };
        }
    }
}