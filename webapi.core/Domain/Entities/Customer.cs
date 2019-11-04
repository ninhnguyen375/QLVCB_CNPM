using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Customer : IAggregateRoot
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}