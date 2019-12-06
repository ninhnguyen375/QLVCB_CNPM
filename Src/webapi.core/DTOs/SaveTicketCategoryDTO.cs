using System.ComponentModel.DataAnnotations;

namespace webapi.core.DTOs
{
    public class SaveTicketCategoryDTO
    {
        [Required(ErrorMessage = "Tên loại vé không được để trống.")]
        public string Name { get; set; }
    }
}