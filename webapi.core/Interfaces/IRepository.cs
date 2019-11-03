using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace webapi.core.Interfaces {
    public interface IRepository<T> where T : IAggregateRoot {
        T GetBy (string id);
        T GetBy (int id);
        IEnumerable<T> GetAll ();
        IEnumerable<T> Find (Expression<Func<T, bool>> predicate);

        void Add (T entity);
        void AddRange (IEnumerable<T> entities);

        void Remove (T entity);
        void RemoveRange (IEnumerable<T> entities);
    }
}