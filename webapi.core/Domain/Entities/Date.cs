using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using webapi.core.Interfaces;

namespace webapi.core.Domain.Entities
{
    public class Date : IAggregateRoot
    {
      [Key]
      public int Id { get; set; }

      [DataType(DataType.Date)]
      public DateTime DateFlight { get; set; }
      
      public virtual ICollection<DateFlight> DateFlights { get; set; } 
    }
}