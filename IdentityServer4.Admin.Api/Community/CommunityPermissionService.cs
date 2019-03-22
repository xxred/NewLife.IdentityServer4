
using IdentityServer4.Admin.Api.Policy;
using IdentityExpress.Manager.BusinessLogic.Entities.Configuration;
using IdentityExpress.Manager.BusinessLogic.Entities.Services;
using IdentityServer4.Admin.Logic.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Community
{
    public class CommunityPermissionService : IPermissionService
  {
    public bool HasPermission(IEnumerable<Claim> claims, Permission[] permissions)
    {
      return true;
    }

    public IEnumerable<Permission> MapClaimsToPermissions(IEnumerable<Claim> userClaims)
    {
      return (IEnumerable<Permission>) new List<Permission>() { Permission.All };
    }

    public Task<IEnumerable<ClaimDto>> GetPolicyClaimsAsClaims()
    {
      List<PolicyClaim> policyClaimList = new List<PolicyClaim>() { new PolicyClaim() { Permission = Permission.All, Type = "role", Value = "AdminUI Administrator" } };
      AccessPolicyDefinition policyDefinition = new AccessPolicyDefinition() { PolicyClaims = policyClaimList, Version = DateTime.UtcNow.ToString() };
      List<ClaimDto> claimDtoList = new List<ClaimDto>();
      foreach (PolicyClaim policyClaim in policyDefinition.PolicyClaims)
        claimDtoList.Add(new ClaimDto(policyClaim.Type, policyClaim.Value));
      return Task.FromResult<IEnumerable<ClaimDto>>((IEnumerable<ClaimDto>) claimDtoList);
    }
  }
}
