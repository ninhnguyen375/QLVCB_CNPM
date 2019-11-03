using System;

namespace webapi.core.Interfaces {
    public interface IUnitOfWork : IDisposable {
        IUserRepository Users { get; }
        IAirportRepository Airports {get; }
        int Complete ();
    }
}