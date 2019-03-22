





using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
  public abstract class UserQueryFactoryBase : IUserQueryFactory
  {
    private readonly string dbProvider;

    protected abstract Dictionary<string, Func<string, UserState, Pagination, IUserQuery>> ProviderToQueryMap { get; }

    public UserQueryFactoryBase(string dbProvider)
    {
      if (string.IsNullOrWhiteSpace(dbProvider))
        throw new ArgumentException("Must provide a valid Database Provider", nameof (dbProvider));
      if (!this.ProviderToQueryMap.Keys.Contains<string>(dbProvider))
        throw new ArgumentOutOfRangeException(nameof (dbProvider), (object) dbProvider, "Provider is not supported");
      this.dbProvider = dbProvider;
    }

    public virtual IUserQuery Create(string match, UserState matchingUserStates, Pagination pagination)
    {
      if (matchingUserStates == null)
        throw new ArgumentNullException(nameof (matchingUserStates));
      if (pagination == null)
        throw new ArgumentNullException(nameof (pagination));
      if (string.IsNullOrWhiteSpace(match))
        return (IUserQuery) new AllUserQuery(matchingUserStates, pagination);
      return this.ProviderToQueryMap[this.dbProvider](match, matchingUserStates, pagination);
    }
  }
}
