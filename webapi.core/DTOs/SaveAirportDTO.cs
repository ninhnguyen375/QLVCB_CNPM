using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveAirportDTO
    {
        [Required(ErrorMessage = "Mã sân bay không được để trống.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Mã sân bay phải có 3 ký tự.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tên sân bay không được để trống.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Tên sân bay phải có ít nhất 4 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tỉnh/Thành phố không được để trống.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Tên sân bay phải có ít nhất 4 ký tự.")]
        public string Location { get; set; }
    }
}