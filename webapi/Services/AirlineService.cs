using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public IEnumerable<AirlineDTO> GetAirlines([FromQuery] Pagination pagination, [FromQuery] SearchAirline search) {
          // Mapping: Airline
          var airlinesSource = _unitOfWork.Airlines.GetAll();
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

        public AirlineDTO GetAirline(string id) {
          // Mapping: Airline
          var airlineSource = _unitOfWork.Airlines.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();
          var airline = _mapper.Map<Airline, AirlineDTO>(airlineSource);

          return airline;
        }

        public DataResult PutAirline(string id, SaveAirlineDTO saveAirlineDTO) {
          var airline = _unitOfWork.Airlines.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airline == null) {
            return new DataResult { Error = 1 };
          }
          
          if (_unitOfWork.Airlines.Find(a =>
                a.Name.ToLower().Equals(saveAirlineDTO.Name.ToLower()) &&
                !a.Id.ToLower().Equals(id.ToLower()))
                .Count() != 0) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveAirline
          _mapper.Map<SaveAirlineDTO, Airline>(saveAirlineDTO, airline);

          _unitOfWork.Complete();

          return new DataResult { Data = airline };
        }

        public DataResult PostAirline(SaveAirlineDTO saveAirlineDTO) {
          // Mapping: SaveAirline
          var airline = _mapper.Map<SaveAirlineDTO, Airline>(saveAirlineDTO);

          // Check id đã tồn tại trong Database chưa
          if(_unitOfWork.Airlines.Find(a => 
              a.Id.ToLower().Equals(airline.Id.ToLower()))
              .Count() != 0) {
            return new DataResult { Error = 1 };
          }

          // Check name đã tồn tại trong Database chưa
          if(_unitOfWork.Airlines.Find(a => 
              a.Name.ToLower().Equals(airline.Name.ToLower()))
              .Count() != 0) {
            return new DataResult { Error = 2 };
          }

          _unitOfWork.Airlines.Add(airline);
          _unitOfWork.Complete();

          return new DataResult { };
        }

        public DataResult DeleteAirline(string id) {
          var airline = _unitOfWork.Airlines.Find(a =>
            a.Id.ToLower().Equals(id.ToLower())).SingleOrDefault();

          if (airline == null) {
            return new DataResult { Error = 1 };
          }

          _unitOfWork.Airlines.Remove(airline);
          _unitOfWork.Complete();

          return new DataResult { };
        }
    }
}