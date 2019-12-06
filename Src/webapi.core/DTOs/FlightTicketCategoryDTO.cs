using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class FlightTicketCategoryDTO
    {
      public string FlightId { get; set; }

      public int TicketCategoryId { get; set; }
      public TicketCategoryDTO TicketCategory { get; set; }

      public decimal Price { get; set; }
    }
}