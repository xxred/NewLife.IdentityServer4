using IdentityServer4.Admin.Logic.Entities.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public interface IPolicyService
    {
        Task<bool> CanUserModifyClaim(IEnumerable<Claim> userClaims, ClaimDto claimToModify);

        Task<bool> CanUserModifyRole(IEnumerable<Claim> userClaims, string roleName);
    }
}
