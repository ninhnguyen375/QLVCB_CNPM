using System;

namespace webapi.core.Interfaces {
    public interface IUnitOfWork : IDisposable {
        IUserRepository Users { get; }
        IAirportRepository Airports { get; }
        IAirlineRepository Airlines { get; }
        ILuggageRepository Luggages { get; }
        ICustomerRepository Customers { get; }
        int Complete ();
    }
}