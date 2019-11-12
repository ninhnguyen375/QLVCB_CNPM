using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.UseCases
{
    public class AddDateFlight
    {
        public ICollection<DateFlight> DateFlights { get; set; } 
    }
}