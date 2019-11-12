namespace webapi.core.UseCases
{
    public class SearchFlight
    {
        public string Id { get; set; } = "";
        public int? StartTime { get; set; }
        public int? FlightTime { get; set; }
        public string AirportFrom { get; set; } = "";
        public string AirportTo { get; set; } = "";
        public string AirlineName { get; set; } = "";
        public int? Status { get; set; }
        public string sortAsc { get; set; } = "";
        public string sortDesc { get; set; } = "";

    }
}