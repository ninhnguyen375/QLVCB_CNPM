using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Airport : IAggregateRoot
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }
    }
}