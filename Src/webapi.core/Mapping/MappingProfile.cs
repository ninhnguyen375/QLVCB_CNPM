using AutoMapper;
using webapi.core.DTOs;
using webapi.core.Domain.Entities;
using webapi.core.UseCases;

namespace webapi.core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile () {
            // 1. Flight
            CreateMap<Flight, FlightDTO>();
            CreateMap<SaveFlightDTO, Flight>();

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

            // 6. DateFlight
            CreateMap<DateFlight, DateFlightDTO>();
            CreateMap<SaveDateFlightDTO, DateFlight>();

            // 7. FlightTicketCategory
            CreateMap<FlightTicketCategory, FlightTicketCategoryDTO>();
            CreateMap<SaveFlightTicketCategoryDTO, FlightTicketCategory>();
            
            // 8. Luggage
            CreateMap<Luggage, LuggageDTO>();
            CreateMap<SaveLuggageDTO, Luggage>();

            // 9. Order
            CreateMap<Order, OrderDTO>();
            CreateMap<SaveOrderDTO, Order>();

            // 10. TicketCategory
            CreateMap<TicketCategory, TicketCategoryDTO>();
            CreateMap<SaveTicketCategoryDTO, TicketCategory>();

            // 11. Ticket
            CreateMap<Ticket, TicketDTO>();
            CreateMap<SaveTicketDTO, Ticket>();

            // 12. User
            CreateMap<User, UserDTO>();
            CreateMap<SaveUserDTO, User>();

            // 13. UseCase: Passenger
            CreateMap<PassengerDF, PassengerDTO>(); 
        }
    }
}