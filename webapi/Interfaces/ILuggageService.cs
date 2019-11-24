using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ILuggageService
    {
        Task<IEnumerable<LuggageDTO>> GetLuggagesAsync(Pagination pagination, SearchLuggage search);
        Task<LuggageDTO> GetLuggageAsync(int id);
        Task<DataResult> PutLuggageAsync(int id, SaveLuggageDTO saveLuggageDTO);
        Task<DataResult> PostLuggageAsync(SaveLuggageDTO saveLuggageDTO);
        Task<DataResult> DeleteLuggageAsync(int id);
    }
}