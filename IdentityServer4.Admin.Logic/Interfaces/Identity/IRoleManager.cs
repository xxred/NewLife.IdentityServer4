using Extensions.Identity.Stores.XCode;
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

    IQueryable<IdentityRole> Roles { get; }

    Task<IdentityResult> CreateAsync(IdentityRole role);

    Task UpdateNormalizedRoleNameAsync(IdentityRole role);

    Task<IdentityResult> UpdateAsync(IdentityRole role);

    Task<IdentityResult> DeleteAsync(IdentityRole role);

    Task<bool> RoleExistsAsync(string roleName);

    string NormalizeKey(string key);

    Task<IdentityRole> FindByIdAsync(string roleId);

    Task<string> GetRoleNameAsync(IdentityRole role);

    Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string name);

    Task<string> GetRoleIdAsync(IdentityRole role);

    Task<IdentityRole> FindByNameAsync(string roleName);

    Task<IdentityResult> AddClaimAsync(IdentityRole role, Claim claim);

    Task<IdentityResult> RemoveClaimAsync(IdentityRole role, Claim claim);

    Task<IList<Claim>> GetClaimsAsync(IdentityRole role);
  }
}
