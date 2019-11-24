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
    public class LuggageService : ILuggageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LuggageService(IUnitOfWork unitOfWork, IMapper mapper) {
          _unitOfWork = unitOfWork;
          _mapper = mapper;
        }

        public async Task<IEnumerable<LuggageDTO>> GetLuggagesAsync(Pagination pagination, SearchLuggage search) {
          // Mapping: Luggage
          var luggagesSource = await _unitOfWork.Luggages.GetAllAsync();
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

        public async Task<LuggageDTO> GetLuggageAsync(int id) {
          // Mapping: Luggage
          var luggageSource = await _unitOfWork.Luggages.GetByAsync(id);
          var luggage = _mapper.Map<Luggage, LuggageDTO>(luggageSource);

          return luggage;
        }

        public async Task<DataResult> PutLuggageAsync(int id, SaveLuggageDTO saveLuggageDTO) {
          var luggage = await _unitOfWork.Luggages.GetByAsync(id);

          // Check luggage exists
          if (luggage == null) {
            return new DataResult { Error = 1 };
          }

          // Check weight of luggage exists except self
          var luggageExist = await _unitOfWork.Luggages.FindAsync(l =>
            l.LuggageWeight == saveLuggageDTO.LuggageWeight &&
            l.Id != id);

          if (luggageExist.Count() != 0 ) {
            return new DataResult { Error = 2 };
          }

          // Mapping: SaveLuggage
          _mapper.Map<SaveLuggageDTO, Luggage>(saveLuggageDTO, luggage);
          
          await _unitOfWork.CompleteAsync();

          return new DataResult { Data = luggage };
        }

        public async Task<DataResult> PostLuggageAsync(SaveLuggageDTO saveLuggageDTO) {
          // Mapping: SaveLuggage
          var luggage = _mapper.Map<SaveLuggageDTO, Luggage>(saveLuggageDTO);

          // Check weight of luggage exists
          var luggageExist = await _unitOfWork.Luggages.FindAsync(l => 
                l.LuggageWeight.Equals(luggage.LuggageWeight));

          if(luggageExist.Count() != 0) {
            return new DataResult { Error = 1 };
          }

          await _unitOfWork.Luggages.AddAsync(luggage);
          await _unitOfWork.CompleteAsync();

          return new DataResult { };
        }
        
        public async Task<DataResult> DeleteLuggageAsync(int id) {
          var luggage = await _unitOfWork.Luggages.GetByAsync(id);

          // Check luggage exists
          if (luggage == null) {
            return new DataResult { Error = 1 };
          }

          await _unitOfWork.Luggages.RemoveAsync(luggage);
          await _unitOfWork.CompleteAsync();

          return new DataResult { };
        }
    }
}