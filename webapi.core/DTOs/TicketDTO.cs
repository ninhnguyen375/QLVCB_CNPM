using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class TicketDTO
    {
      public string Id { get; set; }
      public decimal Price { get; set; }
      public string PassengerName { get; set; }
      // 1: Male, 2: Female, 3: other 
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