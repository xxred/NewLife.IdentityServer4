using IdentityExpress.Manager.BusinessLogic.Constants;
using IdentityExpress.Manager.BusinessLogic.Logic.Services;
using IdentityServer4.Admin.Api.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
	{
		private readonly IPermissionService permissionService;

		private readonly IRecordAuditEventsService audit;

		private readonly IHttpContextAccessor httpContextAccessor;

		public PermissionRequirementHandler(IPermissionService permissionService, IRecordAuditEventsService audit, IHttpContextAccessor httpContextAccessor)
		{
			this.permissionService = permissionService;
			this.audit = audit;
			this.httpContextAccessor = httpContextAccessor;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<Claim> claims = context.User.Claims;
			if (permissionService.HasPermission(claims, requirement.Permissions))
			{
				context.Succeed(requirement);
			}
			else
			{
				HttpRequest request = httpContextAccessor.HttpContext.Request;context.Fail();
			}
			return Task.CompletedTask;
		}
	}
}
