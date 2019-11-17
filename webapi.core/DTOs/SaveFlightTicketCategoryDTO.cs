using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveFlightTicketCategoryDTO
    {
        [Required(ErrorMessage = "Mã chuyến bay không được để trống.")]
        public string FlightId { get; set; }

        [Required(ErrorMessage = "Loại vé không được để trống.")]
        public int TicketCategoryId { get; set; }
        
        [Required(ErrorMessage = "Giá vé không được để trống")]
        public decimal Price { get; set; }
    }
}