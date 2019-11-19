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
    public class LuggageService : ILuggageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LuggageService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public IEnumerable<LuggageDTO> GetLuggages(Pagination pagination, SearchLuggage search) {
          // Mapping: Luggage
          var luggagesSource = _unitOfWork.Luggages.GetAll();
          var luggages = _mapper.Map<IEnumerable<Luggage>, IEnumerable<LuggageDTO>>(luggagesSource);
          
          // Search by LuggageWeight:
          if (search.LuggageWeight != null) {
            luggages = luggages.Where(l =>
              l.LuggageWeight == search.LuggageWeight);
          }

          // Search by Price:
          if (search.PriceFrom != null && search.PriceTo != null) {
            luggages = luggages.Where(l =>
              l.Price >= search.PriceFrom &&
              l.Price <= search.PriceTo);
          } else if (search.PriceFrom != null && search.PriceTo == null) {
            luggages = luggages.Where(l =>
              l.Price >= search.PriceFrom);
          } else if (search.PriceFrom == null && search.PriceTo != null) {
            luggages = luggages.Where(l =>
              l.Price <= search.PriceTo);
          }

          // Sort Asc:
          if (search.sortAsc != "") {
            luggages = luggages.OrderBy(l =>
              l.GetType().GetProperty(search.sortAsc).GetValue(l));
          }

          // Sort Desc:
          if (search.sortDesc != "") {
            luggages = luggages.OrderByDescending(l =>
              l.GetType().GetProperty(search.sortDesc).GetValue(l));
          }

          return luggages;
        }

        public LuggageDTO GetLuggage(int id) {
          // Mapping: Luggage
          var luggageSource = _unitOfWork.Luggages.GetBy(id);
          var luggage = _mapper.Map<Luggage, LuggageDTO>(luggageSource);

          return luggage;
        }

        public DataResult PutLuggage(int id, SaveLuggageDTO saveLuggageDTO) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return new DataResult { Error = 1 };
          }

          if (_unitOfWork.Luggages.Find(l =>
                l.LuggageWeight == saveLuggageDTO.LuggageWeight &&
                l.Id != id)
                .Count() != 0 ) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveLuggage
          _mapper.Map<SaveLuggageDTO, Luggage>(saveLuggageDTO, luggage);
          
          _unitOfWork.Complete();

          return new DataResult { Data = luggage };
        }

        public DataResult PostLuggage(SaveLuggageDTO saveLuggageDTO) {
          // Mapping: SaveLuggage
          var luggage = _mapper.Map<SaveLuggageDTO, Luggage>(saveLuggageDTO);

          if(_unitOfWork.Luggages.Find(l => 
                l.LuggageWeight.Equals(luggage.LuggageWeight))
                .Count() != 0) {
            return new DataResult { Error = 1 };
          }

          _unitOfWork.Luggages.Add(luggage);
          _unitOfWork.Complete();

          return new DataResult { };
        }
        
        public DataResult DeleteLuggage(int id) {
          var luggage = _unitOfWork.Luggages.GetBy(id);

          if (luggage == null) {
            return new DataResult { Error = 1 };
          }

          _unitOfWork.Luggages.Remove(luggage);
          _unitOfWork.Complete();

          return new DataResult { };
        }
    }
}