using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories {
  public class Repository<T> : IRepository<T> where T : class, IAggregateRoot {
    protected AppDbContext Context { get; private set; }

    public Repository (AppDbContext context) {
      Context = context;
    }

    public async Task AddAsync (T entity) {
      await Context.AddAsync (entity);
    }

    public async Task AddRangeAsync (IEnumerable<T> entities) {
      await Context.AddRangeAsync (entities);
    }

    public async Task<IEnumerable<T>> FindAsync (Expression<Func<T, bool>> predicate) {
      return await Context.Set<T> ().Where (predicate).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync () {
      return await Context.Set<T> ().ToListAsync ();
    }
    
    public async Task<T> GetByAsync (string id) {
      return await Context.Set<T> ().FindAsync (id);
    }
    public async Task<T> GetByAsync (int id) {
      return await Context.Set<T> ().FindAsync (id);
    }
    
    public async Task RemoveAsync (T entity) {
      await Task.Run(() => Context.Set<T> ().Remove (entity));
    }

    public async Task RemoveRangeAsync (IEnumerable<T> entities) {
      await Task.Run(() => Context.Set<T> ().RemoveRange (entities));
    }
  }
}