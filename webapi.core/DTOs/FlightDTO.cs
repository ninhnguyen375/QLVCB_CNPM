using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class FlightDTO
    {
      public string Id { get; set; }
      public int StartTime { get; set; }
      public int FlightTime { get; set; }

      public string AirportFrom { get; set; }
      [ForeignKey("AirportFrom")]
      public AirportDTO AirportFromData { get; set; }

      public string AirportTo { get; set; }
      [ForeignKey("AirportTo")]
      public AirportDTO AirportToData { get; set; }

      public int SeatsCount { get; set; }

      // Status: 1 => Active, 0 => Inactive
      public int Status { get; set; }
      
      // Foreign Key (N - 1)
      public string AirlineId { get; set; }
      public AirlineDTO Airline { get; set; }


      public virtual ICollection<DateFlight> DateFlights { get; set; } 

      public virtual ICollection<FlightTicketCategory> FlightTicketCategories { get; set; } 
    }
}