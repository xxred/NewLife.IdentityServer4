using IdentityServer4.Admin.Api.Mappers;
using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize("user")]
	[Route("[controller]")]
	public class RolesController : Controller
	{
		private readonly IRoleService service;

		private readonly IPolicyService policyService;

		public RolesController(IRoleService service, IPolicyService policyService)
        {
			this.service = service;
			this.policyService = policyService;
		}

        [HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateRoleDto role)
		{
			if (role == null)
			{
				return this.BadRequest((object)"role is required.");
			}
			IdentityResult result = await service.Create(role.ToService());
			if (!result.Succeeded)
			{
				IdentityError obj = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj != null) ? obj.Description : null));
			}
			Role createdRole = (await service.Get(role.Name, (IList<RoleOrderBy>)null, (IList<RoleFields>)null)).First((Role x) => string.CompareOrdinal(x.Name, role.Name) == 0);

            return this.CreatedAtAction("GetById", "Roles", (object)new
			{
				id = createdRole.Id
			}, (object)"Role Successfully Created");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				
				return this.BadRequest((object)"id is required.");
			}
			Role role = await service.GetById(id);
			if (role == null)
			{
				return this.NoContent();
			}
			IPolicyService obj = policyService;
			HttpContext obj2 = HttpContext;
			if (!(await obj.CanUserModifyRole((obj2 == null) ? null : obj2.User?.Claims, role.Name)))
			{
				
				return this.Unauthorized();
			}
			IdentityResult result = await service.Delete(role);
			if (!result.Succeeded)
			{
				
				IdentityError obj3 = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj3 != null) ? obj3.Description : null));
			}

			return this.NoContent();
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string name = null, [FromQuery] string sort = null, [FromQuery] string fields = null)
		{
			List<RoleOrderBy> roleOrderBy = new List<RoleOrderBy>();
			List<RoleFields> roleFields = new List<RoleFields>();
			if (!string.IsNullOrWhiteSpace(sort))
			{
				roleOrderBy.AddRange((from x in sort.Split(',')
					select x.ToRoleOrder()).ToList());
			}
			if (!string.IsNullOrWhiteSpace(fields))
			{
				roleFields.AddRange((from x in fields.Split(',')
					select x.ToRoleField()).ToList());
			}
			IList<Role> roles = await service.Get(name, (IList<RoleOrderBy>)roleOrderBy, (IList<RoleFields>)roleFields);

			return this.Ok((object)(from x in roles
				select new RoleDto
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Reserved = x.Reserved
				}).ToList());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				
				return this.BadRequest((object)"id is required.");
			}
			Role role = await service.GetById(id);
			if (role == null)
			{
				
				return this.NotFound();
			}

			return this.Ok((object)role.ToDto());
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleDto role)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				
				return this.BadRequest((object)"id cannot be null");
			}
			if (role == null)
			{
				
				return this.BadRequest((object)"role is required.");
			}
			Role foundRole = await service.GetById(id);
			if (foundRole == null)
			{
				
				return this.NotFound();
			}
			IPolicyService obj = policyService;
			HttpContext obj2 = HttpContext;
			if (!(await obj.CanUserModifyRole((obj2 == null) ? null : obj2.User?.Claims, foundRole.Name)))
			{
				
				return this.Unauthorized();
			}
			IdentityResult result = await service.Update(role.ToService());
			if (!result.Succeeded)
			{
				
				IdentityError obj3 = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj3 != null) ? obj3.Description : null));
			}

			return this.NoContent();
		}

		[HttpPost("{id}/users")]
		public async Task<IActionResult> AddUsers([FromRoute] string id, [FromBody] IList<UserDto> users)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				
				return this.BadRequest((object)"id cannot be null");
			}
			Role foundRole = await service.GetById(id);
			if (foundRole == null)
			{
				
				return this.NotFound();
			}
			if (users == null || !users.Any())
			{
				
				return this.BadRequest((object)"A list of roles is required");
			}
			IPolicyService obj = policyService;
			HttpContext obj2 = HttpContext;
			if (!(await obj.CanUserModifyRole((obj2 == null) ? null : obj2.User?.Claims, foundRole.Name)))
			{
				
				return this.Unauthorized();
			}
			IdentityResult result = await service.AddUsers(foundRole, (IList<User>)(from x in users
				select x.ToService()).ToList());
			if (!result.Succeeded)
			{
				
				IdentityError obj3 = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj3 != null) ? obj3.Description : null));
			}

			return this.Ok();
		}

		[HttpDelete("{id}/users")]
		public async Task<IActionResult> RemoveUsers([FromRoute] string id, [FromBody] IList<UserDto> users)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				
				return this.BadRequest((object)"id cannot be null");
			}
			Role foundRole = await service.GetById(id);
			if (foundRole == null)
			{
				
				return this.NotFound();
			}
			if (users == null || !users.Any())
			{
				
				return this.BadRequest((object)"A list of roles is required");
			}
			IPolicyService obj = policyService;
			HttpContext obj2 = HttpContext;
			if (!(await obj.CanUserModifyRole((obj2 == null) ? null : obj2.User?.Claims, foundRole.Name)))
			{
				
				return this.Unauthorized();
			}
			IdentityResult result = await service.RemoveUsers(foundRole, (IList<User>)(from x in users
				select x.ToService()).ToList());
			if (!result.Succeeded)
			{
				
				IdentityError obj3 = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj3 != null) ? obj3.Description : null));
			}

			return this.Ok();
		}
	}
}
