using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class DateFlight : IAggregateRoot
    {
      [Key, Column(Order = 0)]
      public int DateId { get; set; }

      [Key, Column(Order = 1)]
      public string FlightId { get; set; }

      public int SeatsLeft { get; set; }
      public virtual Flight Flight { get; set; }
      public virtual Date Date { get; set; }

      // Status: 0 => Sold out, 1 => In Stock
      public int Status { get; set; }
      
    }
}