using System;
using System.Threading.Tasks;

namespace webapi.core.Interfaces {
    public interface IUnitOfWork : IDisposable {
        IUserRepository Users { get; }
        IAirportRepository Airports { get; }
        IAirlineRepository Airlines { get; }
        ILuggageRepository Luggages { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IFlightRepository Flights { get; }
        IDateRepository Dates { get; }
        IDateFlightRepository DateFlights { get; }
        ITicketCategoryRepository TicketCategories { get; }
        IFlightTicketCategoryRepository FlightTicketCategories { get; }
        ITicketRepository Tickets { get; }
        Task<int> CompleteAsync ();
    }
}