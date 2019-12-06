using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Flight : IAggregateRoot
    {
      [Key]
      public string Id { get; set; }
      [Required]
      public int StartTime { get; set; }
      [Required]
      public int FlightTime { get; set; }
      [Required]
      public string AirportFrom { get; set; }
      [ForeignKey("AirportFrom")]
      public Airport AirportFromData { get; set; }
      [Required]
      public string AirportTo { get; set; }
      [ForeignKey("AirportTo")]
      public Airport AirportToData { get; set; }

      [Required]
      public int SeatsCount { get; set; }

      // Status: 1 => Active, 0 => Inactive
      public int Status { get; set; }
      
      // Foreign Key (N - 1)
      public string AirlineId { get; set; }
      public Airline Airline { get; set; }


      public virtual ICollection<DateFlight> DateFlights { get; set; } 

      public virtual ICollection<FlightTicketCategory> FlightTicketCategories { get; set; } 
    }
}