using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class TicketCategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}