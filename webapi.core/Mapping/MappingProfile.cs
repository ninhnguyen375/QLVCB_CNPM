using AutoMapper;
using webapi.core.DTOs;
using webapi.core.Domain.Entities;

namespace webapi.core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile () {
            CreateMap<Flight, FlightDTO>();

            CreateMap<Airport, AirportDTO>();

            CreateMap<Airline, AirlineDTO>();

            CreateMap<Customer, CustomerDTO>();

            CreateMap<Date, DateDTO>();

            CreateMap<DateFlight, DateFlightDTO>();

            CreateMap<FlightTicketCategory, FlightTicketCategoryDTO>();
            
            CreateMap<Luggage, LuggageDTO>();

            CreateMap<Order, OrderDTO>();

            CreateMap<TicketCategory, TicketCategoryDTO>();

            CreateMap<Ticket, TicketDTO>();

            CreateMap<User, UserDTO>();
        }
    }
}