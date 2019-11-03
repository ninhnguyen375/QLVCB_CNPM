using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories {
  public class Repository<T> : IRepository<T> where T : class, IAggregateRoot {
    protected AppDbContext Context { get; private set; }

    public Repository (AppDbContext context) {
      Context = context;
    }

    public void Add (T entity) {
      Context.Add (entity);
    }

    public void AddRange (IEnumerable<T> entities) {
      Context.AddRange (entities);
    }

    public IEnumerable<T> Find (Expression<Func<T, bool>> predicate) {
      return Context.Set<T> ().Where (predicate);
    }

    public IEnumerable<T> GetAll () {
      return Context.Set<T> ().ToList ();
    }

    public T GetBy (string id) {
      return Context.Set<T> ().Find (id);
    }
    public T GetBy (int id) {
      return Context.Set<T> ().Find (id);
    }
    public void Remove (T entity) {
      Context.Set<T> ().Remove (entity);
    }

    public void RemoveRange (IEnumerable<T> entities) {
      Context.Set<T> ().RemoveRange (entities);
    }
  }
}