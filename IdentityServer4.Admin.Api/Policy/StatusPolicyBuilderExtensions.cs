using IdentityServer4.Admin.Logic.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer4.Admin.Api.Policy
{
	public static class StatusPolicyBuilderExtensions
	{
		public static AuthorizationPolicyBuilder RequirePermission(this AuthorizationPolicyBuilder builder, Permission[] permissions)
		{
			builder.AddRequirements((IAuthorizationRequirement[])new IAuthorizationRequirement[1]
			{
				new PermissionRequirement(permissions)
			});
			return builder;
		}
	}
}
