using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveLuggageDTO
    {
        [Required(ErrorMessage = "Khối lượng hành lý không được để trống.")]
        public int LuggageWeight { get; set; }
        
        [Required(ErrorMessage = "Giá hành lý không được để trống.")]
        public decimal Price { get; set; }
    }
}