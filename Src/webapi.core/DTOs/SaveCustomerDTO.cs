using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveCustomerDTO
    {
        [Required(ErrorMessage = "CMND không được để trống.")]
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ tên phải có ít nhất 2 ký tự.")]
        public string FullName { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại không chính xác.")]
        public string Phone { get; set; }
    }
}