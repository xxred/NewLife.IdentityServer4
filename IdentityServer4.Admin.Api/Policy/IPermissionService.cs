using IdentityServer4.Admin.Logic.Entities.Configuration;
using IdentityServer4.Admin.Logic.Entities.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public interface IPermissionService
    {
        bool HasPermission(IEnumerable<Claim> claims, Permission[] permissions);

        IEnumerable<Permission> MapClaimsToPermissions(IEnumerable<Claim> claims);

        Task<IEnumerable<ClaimDto>> GetPolicyClaimsAsClaims();
    }
}
