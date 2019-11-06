using System;

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
        ITicketCategoryRepository TicketCatogories { get; }
        IFlightTicketCategoryRepository FlightTicketCategories { get; }
        int Complete ();
    }
}