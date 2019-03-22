using IdentityExpress.Manager.BusinessLogic.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer4.Admin.Api.Policy
{
	public class PermissionRequirement : IAuthorizationRequirement
	{
		public Permission[] Permissions
		{
			get;
		}

		public PermissionRequirement(Permission[] permissions)
		{
			Permissions = permissions;
		}
	}
}
