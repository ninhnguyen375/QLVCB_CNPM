using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using webapi.core.Domain.Entities;
using webapi.core.Interfaces;

namespace webapi.infrastructure.Persistance.Repositories {
  public class UserRepository : Repository<User>, IUserRepository {
    public UserRepository (AppDbContext context) : base (context) {

    }

    protected AppDbContext AppDbContext {
      get { return Context as AppDbContext; }
    }
  }
}