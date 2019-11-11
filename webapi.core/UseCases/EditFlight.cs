namespace webapi.core.UseCases
{
    public class EditFlight
    {
      public int StartTime { get; set; }
      public int FlightTime { get; set; }
      public string AirportFrom { get; set; }
      public string AirportTo { get; set; }
      public int SeatsCount { get; set; }
      public int Status { get; set; }
      public string AirlineId { get; set; }
    }
}