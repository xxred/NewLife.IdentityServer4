





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IdentityServer4.Admin.Logic.Logic.Extensions
{
  public static class QueriableExtensions
  {
    public static IOrderedQueryable<IdentityExpressUser> ApplyOrdering(this IQueryable<IdentityExpressUser> queryable, UserOrderBy orderBy)
    {
      switch (orderBy)
      {
        case UserOrderBy.UsernameAscending:
          return queryable.OrderBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.UserName));
        case UserOrderBy.UsernameDescending:
          return queryable.OrderByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.UserName));
        case UserOrderBy.EmailAscending:
          return queryable.OrderBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.Email));
        case UserOrderBy.EmailDescending:
          return queryable.OrderByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.Email));
        case UserOrderBy.NameAscending:
          return queryable.OrderBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.FirstName)).ThenBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.LastName));
        case UserOrderBy.NameDescending:
          return queryable.OrderByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.FirstName)).ThenByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.LastName));
        default:
          throw new ArgumentOutOfRangeException(nameof (orderBy), (object) orderBy, string.Format("Unrecognized orderby value recieve of {0}.", (object) orderBy));
      }
    }

    public static IOrderedQueryable<IdentityExpressUser> ApplyOrdering(this IOrderedQueryable<IdentityExpressUser> queryable, UserOrderBy orderBy)
    {
      switch (orderBy)
      {
        case UserOrderBy.UsernameAscending:
          return queryable.ThenBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.UserName));
        case UserOrderBy.UsernameDescending:
          return queryable.ThenByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.UserName));
        case UserOrderBy.EmailAscending:
          return queryable.ThenBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.Email));
        case UserOrderBy.EmailDescending:
          return queryable.ThenByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.Email));
        case UserOrderBy.NameAscending:
          return queryable.ThenBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.FirstName)).ThenBy<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.LastName));
        case UserOrderBy.NameDescending:
          return queryable.ThenByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.FirstName)).ThenByDescending<IdentityExpressUser, string>((Expression<Func<IdentityExpressUser, string>>) (x => x.LastName));
        default:
          return queryable;
      }
    }

    public static IOrderedQueryable<IdentityExpressRole> ApplyOrdering(this IQueryable<IdentityExpressRole> queryable, RoleOrderBy orderBy)
    {
      switch (orderBy)
      {
        case RoleOrderBy.NameAscending:
          return queryable.OrderBy<IdentityExpressRole, string>((Expression<Func<IdentityExpressRole, string>>) (x => x.Name));
        case RoleOrderBy.NameDescending:
          return queryable.OrderByDescending<IdentityExpressRole, string>((Expression<Func<IdentityExpressRole, string>>) (x => x.Name));
        default:
          throw new ArgumentOutOfRangeException(nameof (orderBy), (object) orderBy, string.Format("Unrecognized orderby value recieve of {0}.", (object) orderBy));
      }
    }

    public static IOrderedQueryable<IdentityExpressRole> ApplyOrdering(this IOrderedQueryable<IdentityExpressRole> queryable, RoleOrderBy orderBy)
    {
      switch (orderBy)
      {
        case RoleOrderBy.NameAscending:
          return queryable.ThenBy<IdentityExpressRole, string>((Expression<Func<IdentityExpressRole, string>>) (x => x.Name));
        case RoleOrderBy.NameDescending:
          return queryable.ThenByDescending<IdentityExpressRole, string>((Expression<Func<IdentityExpressRole, string>>) (x => x.Name));
        default:
          return queryable;
      }
    }

    public static IOrderedEnumerable<IdentityExpressClaimType> ApplyOrdering(this IEnumerable<IdentityExpressClaimType> queryable, ClaimTypeOrderBy orderBy)
    {
      switch (orderBy)
      {
        case ClaimTypeOrderBy.NameAscending:
          return queryable.OrderBy<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.Name));
        case ClaimTypeOrderBy.NameDescending:
          return queryable.OrderByDescending<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.Name));
        case ClaimTypeOrderBy.RequiredAcending:
          return queryable.OrderBy<IdentityExpressClaimType, bool>((Func<IdentityExpressClaimType, bool>) (x => x.Required));
        case ClaimTypeOrderBy.RequiredDescending:
          return queryable.OrderByDescending<IdentityExpressClaimType, bool>((Func<IdentityExpressClaimType, bool>) (x => x.Required));
        case ClaimTypeOrderBy.ValueTypeAscending:
          return queryable.OrderBy<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.ValueType.ToString()));
        case ClaimTypeOrderBy.ValueTypeDescending:
          return queryable.OrderByDescending<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.ValueType.ToString()));
        default:
          throw new ArgumentOutOfRangeException(nameof (orderBy), (object) orderBy, string.Format("Unrecognized orderby value recieve of {0}.", (object) orderBy));
      }
    }

    public static IOrderedEnumerable<IdentityExpressClaimType> ApplyOrdering(this IOrderedEnumerable<IdentityExpressClaimType> queryable, ClaimTypeOrderBy orderBy)
    {
      switch (orderBy)
      {
        case ClaimTypeOrderBy.NameAscending:
          return queryable.ThenBy<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.Name));
        case ClaimTypeOrderBy.NameDescending:
          return queryable.ThenByDescending<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.Name));
        case ClaimTypeOrderBy.RequiredAcending:
          return queryable.ThenBy<IdentityExpressClaimType, bool>((Func<IdentityExpressClaimType, bool>) (x => x.Required));
        case ClaimTypeOrderBy.RequiredDescending:
          return queryable.ThenByDescending<IdentityExpressClaimType, bool>((Func<IdentityExpressClaimType, bool>) (x => x.Required));
        case ClaimTypeOrderBy.ValueTypeAscending:
          return queryable.ThenBy<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.ValueType.ToString()));
        case ClaimTypeOrderBy.ValueTypeDescending:
          return queryable.ThenByDescending<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.ValueType.ToString()));
        default:
          throw new ArgumentOutOfRangeException(nameof (orderBy), (object) orderBy, string.Format("Unrecognized orderby value recieve of {0}.", (object) orderBy));
      }
    }

    public static bool Validate(this IList<UserOrderBy> orderBy)
    {
      return (!orderBy.Contains(UserOrderBy.UsernameAscending) || !orderBy.Contains(UserOrderBy.UsernameDescending)) && (!orderBy.Contains(UserOrderBy.EmailAscending) || !orderBy.Contains(UserOrderBy.EmailDescending)) && (!orderBy.Contains(UserOrderBy.NameAscending) || !orderBy.Contains(UserOrderBy.NameDescending));
    }

    public static bool Validate(this IList<RoleOrderBy> orderBy)
    {
      return !orderBy.Contains(RoleOrderBy.NameAscending) || !orderBy.Contains(RoleOrderBy.NameDescending);
    }

    public static bool Validate(this IList<ClaimTypeOrderBy> orderBy)
    {
      return (!orderBy.Contains(ClaimTypeOrderBy.NameAscending) || !orderBy.Contains(ClaimTypeOrderBy.NameDescending)) && (!orderBy.Contains(ClaimTypeOrderBy.RequiredAcending) || !orderBy.Contains(ClaimTypeOrderBy.RequiredDescending)) && (!orderBy.Contains(ClaimTypeOrderBy.ValueTypeAscending) || !orderBy.Contains(ClaimTypeOrderBy.ValueTypeDescending));
    }
  }
}
