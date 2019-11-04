using webapi.core.Interfaces;
using webapi.infrastructure.Persistance.Repositories;

namespace webapi.infrastructure.Persistance {
    public class UnitOfWork : IUnitOfWork {
        private readonly AppDbContext _context;

        public UnitOfWork (AppDbContext context) {
            Users = new UserRepository (context);
            Airports = new AirportRepository(context);
            Airlines = new AirlineRepository(context);
            Luggages = new LuggageRepository(context);
            Customers = new CustomerRepository(context);
            _context = context;
        }

        public IUserRepository Users { get; private set; }
        public IAirportRepository Airports { get; private set; }
        public IAirlineRepository Airlines { get; private set; }
        public ILuggageRepository Luggages { get; private set; }
        public ICustomerRepository Customers { get; private set; } 
        public int Complete () {
            return _context.SaveChanges ();
        }

        public void Dispose () {
            _context.Dispose ();
        }
    }
}