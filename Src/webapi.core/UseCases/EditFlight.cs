using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.UseCases
{
    public class EditFlight
    {
      public string Id { get; set; }
      public int StartTime { get; set; }
      public int FlightTime { get; set; }
      public string AirportFrom { get; set; }
      public string AirportTo { get; set; }
      public int SeatsCount { get; set; }
      public int Status { get; set; }
      public string AirlineId { get; set; }
      public virtual ICollection<FlightTicketCategory> FlightTicketCategories { get; set; } 
    }
}