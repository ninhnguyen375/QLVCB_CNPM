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
    public class AirlineService : IAirlineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirlineService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }
        
        public async Task<IEnumerable<AirlineDTO>> GetAirlinesAsync([FromQuery] Pagination pagination, [FromQuery] SearchAirline search) {
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

          return airlines;
        }

        public async Task<AirlineDTO> GetAirlineAsync(string id) {
          // Mapping: Airline
          var airlineSource = await _unitOfWork.Airlines.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));
          var airline = _mapper.Map<Airline, AirlineDTO>(airlineSource.SingleOrDefault());

          return airline;
        }

        public async Task<DataResult> PutAirlineAsync(string id, SaveAirlineDTO saveAirlineDTO) {
          var airlineAsync = await _unitOfWork.Airlines.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airline exists
          var airline = airlineAsync.SingleOrDefault();

          if (airline == null) {
            return new DataResult { Error = 1 };
          }
          
          // Check name of airline exists except self
          var airlineExist = await _unitOfWork.Airlines.FindAsync(a =>
            a.Name.ToLower().Equals(saveAirlineDTO.Name.ToLower()) &&
            !a.Id.ToLower().Equals(id.ToLower()));

          if (airlineExist.Count() != 0) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveAirline
          _mapper.Map<SaveAirlineDTO, Airline>(saveAirlineDTO, airline);

          await _unitOfWork.CompleteAsync();

          return new DataResult { Data = airline };
        }

        public async Task<DataResult> PostAirlineAsync(SaveAirlineDTO saveAirlineDTO) {
          // Mapping: SaveAirline
          var airline = _mapper.Map<SaveAirlineDTO, Airline>(saveAirlineDTO);

          // Check id đã tồn tại trong Database chưa
          var airlineExist = await _unitOfWork.Airlines.FindAsync(a => 
            a.Id.ToLower().Equals(airline.Id.ToLower()));

          if(airlineExist.Count() != 0) {
            return new DataResult { Error = 1 };
          }

          // Check name đã tồn tại trong Database chưa
          airlineExist = await _unitOfWork.Airlines.FindAsync(a => 
            a.Name.ToLower().Equals(airline.Name.ToLower()));

          if(airlineExist.Count() != 0) {
            return new DataResult { Error = 2 };
          }

          await _unitOfWork.Airlines.AddAsync(airline);
          await _unitOfWork.CompleteAsync();

          return new DataResult { };
        }

        public async Task<DataResult> DeleteAirlineAsync(string id) {
          var airlineAsync = await _unitOfWork.Airlines.FindAsync(a =>
            a.Id.ToLower().Equals(id.ToLower()));

          // Check airline exists
          var airline = airlineAsync.SingleOrDefault();

          if (airline == null) {
            return new DataResult { Error = 1 };
          }

          await _unitOfWork.Airlines.RemoveAsync(airline);
          await _unitOfWork.CompleteAsync();

          return new DataResult { };
        }
    }
}