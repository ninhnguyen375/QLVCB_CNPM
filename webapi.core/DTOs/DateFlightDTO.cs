using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class DateFlightDTO
    {
      public int DateId { get; set; }

      public string FlightId { get; set; }

      public int SeatsLeft { get; set; }

      // Status: 0 => Sold out, 1 => In Stock
      public int Status { get; set; }
      
    }
}