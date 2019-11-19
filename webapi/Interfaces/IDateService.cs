using System.Collections.Generic;
using webapi.core.DTOs;
using webapi.core.UseCases;

namespace webapi.Interfaces
{
    public interface IDateService
    {
      IEnumerable<DateDTO> GetDates(Pagination pagination, SearchDate search);
      DateDTO GetDate(int id);
      DataResult PutDate(int id, SaveDateDTO saveDateDTO);
      DataResult PostDate(SaveDateDTO saveDateDTO);
      DataResult DeleteDate(int id);
      DataResult PostFlight(int id, AddDateFlight values);
      DataResult DeleteFlight(int id, RemoveFlight values);
      DataResult SearchFlights(SearchFlightFE values);
    }
}