using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class OrderDTO
    {
      public string Id { get; set; }
      
      public int TicketCount { get; set; }

      public decimal TotalPrice { get; set; }

      public DateTime CreateAt { get; set; }

      // Status: 0 => Unconfirm, 1 => Confirm
      public int Status { get; set; }
      
      // Foreign key for DepartureDate
      public int DepartureDateId { get; set; }
      public Date DepartureDate { get; set; }

      // Foreign key for ReturnDate
      public int? ReturnDateId { get; set; }
      public Date ReturnDate { get; set; }

      // Foreign Key (N - 1)
      public string CustomerId { get; set; } // Để thao tác trong Controller
      public CustomerDTO Customer { get; set; }

      // Foreign Key (N - 1)
      public int? UserId { get; set; } // Để thao tác trong Controller
      public UserDTO User { get; set; }

      public virtual ICollection<TicketDTO> Tickets { get; set; }
    }
}