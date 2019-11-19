using System.Collections.Generic;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface ILuggageService
    {
        IEnumerable<LuggageDTO> GetLuggages(Pagination pagination, SearchLuggage search);
        LuggageDTO GetLuggage(int id);
        DataResult PutLuggage(int id, SaveLuggageDTO saveLuggageDTO);
        DataResult PostLuggage(SaveLuggageDTO saveLuggageDTO);
        DataResult DeleteLuggage(int id);
    }
}