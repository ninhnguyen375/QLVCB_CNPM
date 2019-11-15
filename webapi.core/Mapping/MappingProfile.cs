using AutoMapper;
using webapi.core.DTOs;
using webapi.core.Domain.Entities;

namespace webapi.core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile () {
            CreateMap<Flight, FlightDTO>();

            // 2. Airport
            CreateMap<Airport, AirportDTO>();
            CreateMap<SaveAirportDTO, Airport>();

            // 3. Airline
            CreateMap<Airline, AirlineDTO>();
            CreateMap<SaveAirlineDTO, Airline>();

            // 4. Customer
            CreateMap<Customer, CustomerDTO>();
            CreateMap<SaveCustomerDTO, Customer>();

            // 5. Date
            CreateMap<Date, DateDTO>();
            CreateMap<SaveDateDTO, Date>();

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