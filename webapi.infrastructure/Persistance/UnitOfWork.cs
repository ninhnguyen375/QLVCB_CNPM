using webapi.core.Interfaces;
using webapi.infrastructure.Persistance.Repositories;

namespace webapi.infrastructure.Persistance {
    public class UnitOfWork : IUnitOfWork {
        private readonly AppDbContext _context;

        public UnitOfWork (AppDbContext context) {
            Users = new UserRepository (context);
            _context = context;
        }

        public IUserRepository Users { get; private set; }

        public int Complete () {
            return _context.SaveChanges ();
        }

        public void Dispose () {
            _context.Dispose ();
        }
    }
}