using IdentityExpress.Manager.BusinessLogic.Entities.Configuration;
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public class PolicyService : IPolicyService
	{
		private readonly IProvideClaimsToPermissionsPolicy provideClaimsToPermissionsPolicy;

		private readonly IPermissionService permissionService;

		public PolicyService(IProvideClaimsToPermissionsPolicy provideClaimsToPermissionsPolicy, IPermissionService permissionService)
		{
			this.provideClaimsToPermissionsPolicy = provideClaimsToPermissionsPolicy;
			this.permissionService = permissionService;
		}

		public async Task<bool> CanUserModifyClaim(IEnumerable<Claim> userClaims, ClaimDto claimToModify)
		{
			if (userClaims == null)
			{
				throw new ArgumentNullException("userClaims");
			}
			if (claimToModify == null)
			{
				throw new ArgumentNullException("claimToModify");
			}
			if (permissionService.HasPermission(userClaims, (Permission[])new Permission[1]
			{
				4
			}))
			{
				return true;
			}
			PolicyClaim claimExistsInPolicy = (await provideClaimsToPermissionsPolicy.GetAccessPolicyDefinition()).PolicyClaims.FirstOrDefault((PolicyClaim x) => x.Type == claimToModify.Type && x.Value == claimToModify.Value);
			return claimExistsInPolicy == null;
		}

		public Task<bool> CanUserModifyRole(IEnumerable<Claim> userClaims, string roleName)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (userClaims == null)
			{
				throw new ArgumentNullException("userClaims");
			}
			if (roleName == null)
			{
				throw new ArgumentNullException("roleName");
			}
			ClaimDto claimToModify = new ClaimDto("role", roleName);
			return CanUserModifyClaim(userClaims, claimToModify);
		}
	}
}
