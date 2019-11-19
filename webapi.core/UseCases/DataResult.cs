using System;
using System.Collections.Generic;
using webapi.core.DTOs;

namespace webapi.core.UseCases
{
    public class DataResult
    {
        public int Error { get; set; } = 0; // Success
        public Object Data { get; set; }
        public IEnumerable<FlightDTO> DepartureFlights { get; set; }
        public IEnumerable<FlightDTO> ReturnFlights { get; set; }

    }
}