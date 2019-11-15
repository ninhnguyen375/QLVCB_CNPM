using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using webapi.core.Domain.Entities;

namespace webapi.core.DTOs
{
    public class SaveDateDTO
    {
        [Required(ErrorMessage = "Ngày khởi hành không được để trống.")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        public virtual ICollection<DateFlight> DateFlights { get; set; }
    }
}