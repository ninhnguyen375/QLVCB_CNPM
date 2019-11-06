using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Ticket : IAggregateRoot
    {
      [Key]
      public string Id { get; set; }
      [Required]
      public decimal Price { get; set; }
      [Required]
      public string PassengerName { get; set; }
      // 1: Male, 2: Female, 3: other 
      [Required]
      public int PassengerGender { get; set; }

      // Foreign key for Luggage
      public int LuggageId { get; set; }
      public Luggage Luggage { get; set; }
      
      // Foreign key for Flight
      public string FlightId { get; set; }
      public Flight Flight { get; set; } 
      
      // Foreign key for Order
      public string OrderId { get; set; }
      public Order Order { get; set; } 
      
      // Foreign key for TicketCategory
      public int TicketCategoryId { get; set; }
      public TicketCategory TicketCategory { get; set; } 
      
    }
}