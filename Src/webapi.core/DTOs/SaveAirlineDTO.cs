using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveAirlineDTO
    {
        [Required(ErrorMessage = "Mã hãng hàng không không được để trống.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Mã hãng hàng không phải có 2 ký tự.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tên hãng hàng không không được để trống.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Tên hãng hàng không phải có ít nhất nhất 4 ký tự.")]
        public string Name { get; set; }
    }
}