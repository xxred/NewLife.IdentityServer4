





using RSK.Audit;
using System;

namespace IdentityServer4.Admin.Logic.Logic.Services.AuditQueries
{
  public static class AuditQueryBuilderExtensions
  {
    public static IAuditQuery WithStringQueryArgument(this IAuditQuery query, string queryValue, Func<Matches, string, IAuditQuery> filter)
    {
      if (!string.IsNullOrEmpty(queryValue))
        query = filter(Matches.StartsWith, queryValue);
      return query;
    }
  }
}
