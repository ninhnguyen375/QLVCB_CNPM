using System.ComponentModel.DataAnnotations;
namespace webapi.core.UseCases {
    public class ChangePassword {
        [Required]
        public string oldPassword { get; set; }
        [Required]
        public string newPassword { get; set; }
    }
}