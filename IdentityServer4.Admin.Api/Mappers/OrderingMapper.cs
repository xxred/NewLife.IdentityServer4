using IdentityServer4.Admin.Logic.Entities.Services;
using System;

namespace IdentityServer4.Admin.Api.Mappers
{
  public static class OrderingMapper
  {
    public static UserOrderBy ToUserOrder(this string order)
    {
      if (string.Compare("username", order, StringComparison.OrdinalIgnoreCase) == 0)
        return UserOrderBy.UsernameAscending;
      if (string.Compare("-username", order, StringComparison.OrdinalIgnoreCase) == 0)
        return UserOrderBy.UsernameDescending;
      if (string.Compare("email", order, StringComparison.OrdinalIgnoreCase) == 0)
        return UserOrderBy.EmailAscending;
      if (string.Compare("-email", order, StringComparison.OrdinalIgnoreCase) == 0)
        return UserOrderBy.EmailDescending;
      if (string.Compare("name", order, StringComparison.OrdinalIgnoreCase) == 0)
        return UserOrderBy.NameAscending;
      if (string.Compare("-name", order, StringComparison.OrdinalIgnoreCase) == 0)
        return UserOrderBy.NameDescending;
      throw new ArgumentOutOfRangeException(order, "Unrecongized order parameter of " + order);
    }

    public static RoleOrderBy ToRoleOrder(this string order)
    {
      if (string.Compare("name", order, StringComparison.OrdinalIgnoreCase) == 0)
        return RoleOrderBy.NameAscending;
      if (string.Compare("-name", order, StringComparison.OrdinalIgnoreCase) == 0)
        return RoleOrderBy.NameDescending;
      throw new ArgumentOutOfRangeException(order, "Unrecongized order parameter of " + order);
    }

    public static RoleFields ToRoleField(this string field)
    {
      if (string.Compare("id", field, StringComparison.OrdinalIgnoreCase) == 0)
        return RoleFields.Id;
      if (string.Compare("name", field, StringComparison.OrdinalIgnoreCase) == 0)
        return RoleFields.Name;
      if (string.Compare("description", field, StringComparison.OrdinalIgnoreCase) == 0)
        return RoleFields.Description;
      if (string.Compare("users", field, StringComparison.OrdinalIgnoreCase) == 0)
        return RoleFields.Users;
      throw new ArgumentOutOfRangeException(field, "Unrecongized order parameter of " + field);
    }

    public static ClaimTypeOrderBy ToClaimTypeOrder(this string order)
    {
      if (string.Compare("name", order, StringComparison.OrdinalIgnoreCase) == 0)
        return ClaimTypeOrderBy.NameAscending;
      if (string.Compare("-name", order, StringComparison.OrdinalIgnoreCase) == 0)
        return ClaimTypeOrderBy.NameDescending;
      if (string.Compare("required", order, StringComparison.OrdinalIgnoreCase) == 0)
        return ClaimTypeOrderBy.RequiredAcending;
      if (string.Compare("-required", order, StringComparison.OrdinalIgnoreCase) == 0)
        return ClaimTypeOrderBy.RequiredDescending;
      if (string.Compare("valueType", order, StringComparison.OrdinalIgnoreCase) == 0)
        return ClaimTypeOrderBy.ValueTypeAscending;
      if (string.Compare("-valueType", order, StringComparison.OrdinalIgnoreCase) == 0)
        return ClaimTypeOrderBy.ValueTypeDescending;
      throw new ArgumentOutOfRangeException(order, "Unrecongized order parameter of " + order);
    }
  }
}
