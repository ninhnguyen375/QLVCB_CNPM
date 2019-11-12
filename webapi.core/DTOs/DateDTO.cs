using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class DateDTO
    {
      public int Id { get; set; }

      public DateTime DepartureDate { get; set; }
    }
}