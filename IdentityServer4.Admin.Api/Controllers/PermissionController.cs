using IdentityServer4.Admin.Api.Controllers;
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize]
	[Route("[controller]")]
	public class PermissionController : Controller
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		private readonly IPermissionService permissionService;

		public PermissionController(IHttpContextAccessor httpContextAccessor, IPermissionService permissionService)
		{
			if (httpContextAccessor == null)
			{
				throw new ArgumentNullException("httpContextAccessor");
			}
			this.httpContextAccessor = httpContextAccessor;
			if (permissionService == null)
			{
				throw new ArgumentNullException("permissionService");
			}
			this.permissionService = permissionService;
		}

        [HttpGet]
		public async Task<IActionResult> GetPermissionsForUser()
		{
			IEnumerable<Claim> userClaims = httpContextAccessor.HttpContext.User.Claims;
			if (userClaims == null)
			{
				return this.BadRequest((object)"No Claims Found For User");
			}
			IEnumerable<Permission> permissions = permissionService.MapClaimsToPermissions(userClaims);
			return this.Ok((object)new UserPermissions(permissions, await permissionService.GetPolicyClaimsAsClaims()));
		}

		[HttpGet("avaliable")]
		public IActionResult GetAvaliablePermissions()
		{
			return this.Ok((object)(from x in Enum.GetNames(typeof(Permission))
				where x != Enum.GetName(typeof(Permission), 0)
				select x).ToArray());
		}
	}
}
