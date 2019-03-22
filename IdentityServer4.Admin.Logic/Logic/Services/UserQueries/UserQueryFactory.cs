





using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
  public class UserQueryFactory : UserQueryFactoryBase
  {
    protected override Dictionary<string, Func<string, UserState, Pagination, IUserQuery>> ProviderToQueryMap
    {
      get
      {
        return new Dictionary<string, Func<string, UserState, Pagination, IUserQuery>>()
        {
          ["Sqlite"] = (Func<string, UserState, Pagination, IUserQuery>) ((m, s, p) => (IUserQuery) new StandardQueryTermUserQuery(m, s, p)),
          ["MySql"] = (Func<string, UserState, Pagination, IUserQuery>) ((m, s, p) => (IUserQuery) new StandardQueryTermUserQuery(m, s, p)),
          ["PostgreSql"] = (Func<string, UserState, Pagination, IUserQuery>) ((m, s, p) => (IUserQuery) new StandardQueryTermUserQuery(m, s, p)),
          ["SqlServer"] = (Func<string, UserState, Pagination, IUserQuery>) ((m, s, p) => (IUserQuery) new StoredProcedureUserQuery(m, s, p))
        };
      }
    }

    public UserQueryFactory(string dbProvider)
      : base(dbProvider)
    {
    }
  }
}
