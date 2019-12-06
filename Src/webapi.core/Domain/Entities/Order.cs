using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Order : IAggregateRoot
    {
      [Key]
      public string Id { get; set; }
      
      [Required]
      public int TicketCount { get; set; }

      [Required]
      public decimal TotalPrice { get; set; }

      [DataType(DataType.DateTime)]
      public DateTime CreateAt { get; set; }

      // Status: 0 => New, 1 => Confirm, 2 => Unconfirm
      public int Status { get; set; } = 0;
      
      // Foreign key for DepartureDate
      public int DepartureDateId { get; set; }
      public Date DepartureDate { get; set; }

      // Foreign key for ReturnDate
      public int? ReturnDateId { get; set; }
      public Date ReturnDate { get; set; }

      // Foreign Key (N - 1)
      public string CustomerId { get; set; } // Để thao tác trong Controller
      public Customer Customer { get; set; }

      // Foreign Key (N - 1)
      public int? UserId { get; set; } // Để thao tác trong Controller
      public User User { get; set; }
    }
}