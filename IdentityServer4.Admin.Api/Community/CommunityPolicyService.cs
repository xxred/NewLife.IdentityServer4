
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Community
{
    public class CommunityPolicyService : IPolicyService
  {
    public Task<bool> CanUserModifyClaim(IEnumerable<Claim> userClaims, ClaimDto claimToModify)
    {
      return Task.FromResult<bool>(true);
    }

    public Task<bool> CanUserModifyRole(IEnumerable<Claim> userClaims, string roleName)
    {
      return Task.FromResult<bool>(true);
    }
  }
}
