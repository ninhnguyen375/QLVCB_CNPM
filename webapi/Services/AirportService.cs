using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        public async Task<IEnumerable<AirportDTO>> GetAirportsAsync(Pagination pagination, SearchAirport search) {
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

          return airports;
        }

        public async Task<AirportDTO> GetAirportAsync(string id) {
          // Mapping: Airport
          var airportSource = await _unitOfWork.Airports.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));
          var airport = _mapper.Map<Airport, AirportDTO>(airportSource.SingleOrDefault());

          return airport;
        }

        public async Task<DataResult> PutAirportAsync(string id, SaveAirportDTO saveAirportDTO) {
          var airportAsync = await _unitOfWork.Airports.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airport exists
          var airport = airportAsync.SingleOrDefault();

          if (airport == null) {
            return new DataResult { Error = 1 };
          }
          
          // Check name of airport exists except self
          var airportExist = await _unitOfWork.Airports.FindAsync(a =>
            a.Name.ToLower().Equals(saveAirportDTO.Name.ToLower()) &&
            !a.Id.ToLower().Equals(id.ToLower()));

          if (airportExist.Count() != 0) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveAirport
          _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO, airport); 

          await _unitOfWork.CompleteAsync();

          return new DataResult { Data = airport };
        }

        public async Task<DataResult> PostAirportAsync(SaveAirportDTO saveAirportDTO) {
          // Mapping: SaveAirport
          var airport = _mapper.Map<SaveAirportDTO, Airport>(saveAirportDTO);

          // Check id đã tồn tại trong Database chưa
          var airlineExist = await _unitOfWork.Airports.FindAsync(a => 
              a.Id.ToLower().Equals(airport.Id.ToLower()));

          if(airlineExist.Count() != 0) {
            return new DataResult { Error = 1 };
          }

          // Check name đã tồn tại trong Database chưa
          airlineExist = await _unitOfWork.Airports.FindAsync(a => 
              a.Name.ToLower().Equals(airport.Name.ToLower()));

          if(airlineExist.Count() != 0) {
            return new DataResult { Error = 2 };
          }

          await _unitOfWork.Airports.AddAsync(airport);
          await _unitOfWork.CompleteAsync();

          return new DataResult { };
        }

        public async Task<DataResult> DeleteAirportAsync(string id) {
          var airportAsync = await _unitOfWork.Airports.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airport exists
          var airport = airportAsync.SingleOrDefault();

          if (airport == null) {
            return new DataResult { Error = 1 };
          }

          await _unitOfWork.Airports.RemoveAsync(airport);
          await _unitOfWork.CompleteAsync();

          return new DataResult { };
        }
    }
}