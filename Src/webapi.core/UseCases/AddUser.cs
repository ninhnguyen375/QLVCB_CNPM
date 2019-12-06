using System.ComponentModel.DataAnnotations;
namespace webapi.core.UseCases
{
    public class AddUser
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Identifier { get; set; }

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}