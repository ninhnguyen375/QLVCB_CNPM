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

      // Status: 0 => Unconfirm, 1 => Confirm
      public int Status { get; set; } = 0;
      
      // Foreign Key (N - 1)
      public string CustomerId { get; set; } // Để thao tác trong Controller
      public Customer Customer { get; set; }
    }
}