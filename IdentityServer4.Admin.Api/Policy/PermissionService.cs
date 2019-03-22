using IdentityExpress.Manager.BusinessLogic.Entities.Configuration;
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Configuration;
using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public class PermissionService : IPermissionService
	{
		private readonly IProvideClaimsToPermissionsPolicy provideClaimsToPermissionsPolicy;

		public PermissionService(IProvideClaimsToPermissionsPolicy provideClaimsToPermissionsPolicy)
		{
			if (provideClaimsToPermissionsPolicy == null)
			{
				throw new ArgumentNullException("provideClaimsToPermissionsPolicy");
			}
			this.provideClaimsToPermissionsPolicy = provideClaimsToPermissionsPolicy;
		}

		public async Task<IEnumerable<ClaimDto>> GetPolicyClaimsAsClaims()
		{
			AccessPolicyDefinition accessPolicy = await provideClaimsToPermissionsPolicy.GetAccessPolicyDefinition();
			List<ClaimDto> policyClaimsAsClaims = new List<ClaimDto>();
			foreach (PolicyClaim policyClaim in accessPolicy.PolicyClaims)
			{
				policyClaimsAsClaims.Add(new ClaimDto(policyClaim.Type, policyClaim.Value));
			}
			return policyClaimsAsClaims;
		}

		public bool HasPermission(IEnumerable<Claim> userClaims, Permission[] permissions)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (userClaims == null)
			{
				throw new ArgumentNullException("userClaims");
			}
			List<Permission> list = MapClaimsToPermissions(userClaims).ToList();
			foreach (Permission item in permissions)
			{
				if (list.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<Permission> MapClaimsToPermissions(IEnumerable<Claim> userClaims)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			if (userClaims == null)
			{
				throw new ArgumentNullException("userClaims");
			}
			List<Permission> list = new List<Permission>();
			List<PolicyClaim> policyClaims = provideClaimsToPermissionsPolicy.GetAccessPolicyDefinition().Result.PolicyClaims;
			foreach (PolicyClaim policy in policyClaims)
			{
				if (userClaims.Any((Claim x) => x.Type == policy.Type && x.Value == policy.Value) && !list.Contains(policy.Permission))
				{
					list.Add(policy.Permission);
				}
			}
			return list;
		}
	}
}
