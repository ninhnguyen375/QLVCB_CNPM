using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveDateFlightDTO
    {
        [Required(ErrorMessage = "Mã chuyến bay không được để trống.")]
        public string FlightId { get; set; }

        [Required(ErrorMessage = "Mã ngày khởi hành không được để trống.")]
        public int DateId { get; set; }
        
        public int SeatsLeft { get; set; }

        // Status: 0 => Sold out, 1 => In Stock
        public int Status { get; set; }
    }
}