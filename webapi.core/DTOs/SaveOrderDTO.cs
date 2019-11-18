using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveOrderDTO
    {
      public string Id { get; set; }
      
      public int TicketCount { get; set; }

      public decimal TotalPrice { get; set; }

      [DataType(DataType.DateTime)]
      public DateTime CreateAt { get; set; }

      // Status: 0 => New, 1 => Confirm, 2 => Unconfirm
      public int Status { get; set; } = 0;
      
      [Required(ErrorMessage = "Mã ngày khởi hành không được để trống.")]
      // Foreign key for DepartureDate
      public int DepartureDateId { get; set; }

      [Required(ErrorMessage = "Mã ngày về không được để trống.")]
      // Foreign key for ReturnDate
      public int? ReturnDateId { get; set; }

      [Required(ErrorMessage = "Mã khách đặt hàng không được để trống.")]
      // Foreign Key (N - 1)
      public string CustomerId { get; set; } // Để thao tác trong Controller

      // Foreign Key (N - 1)
      public int? UserId { get; set; } // Để thao tác trong Controller
    }
}