using webapi.core.DTOs;

namespace webapi.core.UseCases
{
    public class SearchAirport : AirportDTO
    {
        public string sortAsc { get; set; }
        public string sortDesc { get; set; }
    }
}