using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveTicketDTO
    {
      public string Id { get; set; }

      public decimal Price { get; set; }

      [Required(ErrorMessage = "Họ tên hành khách không được để trống.")]
      public string PassengerName { get; set; }

      // 1: Male, 2: Female, 3: other 
      public int PassengerGender { get; set; }

      [Required(ErrorMessage = "Mã hành lý không được để trống.")]
      // Foreign key for Luggage
      public int LuggageId { get; set; }
      
      [Required(ErrorMessage = "Mã chuyến bay không được để trống.")]
      // Foreign key for Flight
      public string FlightId { get; set; }
  
      [Required(ErrorMessage = "Mã ngày khởi hành không được để trống.")]
      // Foreign key for Date
      public int DateId { get; set; }
      
      [Required(ErrorMessage = "Mã hóa đơn không được để trống.")]
      // Foreign key for Order
      public string OrderId { get; set; }
      
      [Required(ErrorMessage = "Mã loại vé khnog6 được để trống.")]
      // Foreign key for TicketCategory
      public int TicketCategoryId { get; set; }
    }
}