





using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Identity
{
  public interface IRoleManager : IDisposable
  {
    bool SupportsQueryableRoles { get; }

    bool SupportsRoleClaims { get; }

    IQueryable<IdentityExpressRole> Roles { get; }

    Task<IdentityResult> CreateAsync(IdentityExpressRole role);

    Task UpdateNormalizedRoleNameAsync(IdentityExpressRole role);

    Task<IdentityResult> UpdateAsync(IdentityExpressRole role);

    Task<IdentityResult> DeleteAsync(IdentityExpressRole role);

    Task<bool> RoleExistsAsync(string roleName);

    string NormalizeKey(string key);

    Task<IdentityExpressRole> FindByIdAsync(string roleId);

    Task<string> GetRoleNameAsync(IdentityExpressRole role);

    Task<IdentityResult> SetRoleNameAsync(IdentityExpressRole role, string name);

    Task<string> GetRoleIdAsync(IdentityExpressRole role);

    Task<IdentityExpressRole> FindByNameAsync(string roleName);

    Task<IdentityResult> AddClaimAsync(IdentityExpressRole role, Claim claim);

    Task<IdentityResult> RemoveClaimAsync(IdentityExpressRole role, Claim claim);

    Task<IList<Claim>> GetClaimsAsync(IdentityExpressRole role);
  }
}
