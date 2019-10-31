using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities {
    public class User : IAggregateRoot {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Identifier { get; set; }

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // 1: active, 2: banned, 3: deleted
        [Required]
        public int Status { get; set; }

        // 1: ADMIN, 2: STAFF
        [Required]
        public int Role { get; set; } = 1;

        [Required]
        public string Password { get; set; }
    }
}