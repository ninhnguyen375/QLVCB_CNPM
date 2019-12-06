using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class FlightTicketCategory : IAggregateRoot
    {
      [Key, Column(Order = 0)]
      public string FlightId { get; set; }

      [Key, Column(Order = 1)]
      public int TicketCategoryId { get; set; }

      public virtual Flight Flight { get; set; }
      public virtual TicketCategory TicketCategory { get; set; }

      [Required]
      public decimal Price { get; set; }

      
    }
}