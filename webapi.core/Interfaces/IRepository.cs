using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace webapi.core.Interfaces {
    public interface IRepository<T> where T : IAggregateRoot {
        Task<T> GetByAsync (string id);
        Task<T> GetByAsync (int id);
        Task<IEnumerable<T>> GetAllAsync ();
        Task<IEnumerable<T>> FindAsync (Expression<Func<T, bool>> predicate);

        Task AddAsync (T entity);
        Task AddRangeAsync (IEnumerable<T> entities);

        Task RemoveAsync (T entity);
        Task RemoveRangeAsync (IEnumerable<T> entities);
    }
}