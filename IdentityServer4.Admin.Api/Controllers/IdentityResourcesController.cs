using IdentityServer4.Admin.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Admin.Api.Mappers;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Entities.Services;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize("identityserver")]
	[Route("[controller]")]
	public class IdentityResourcesController : Controller
	{
		private readonly IIdentityResourceService service;

		public IdentityResourcesController(IIdentityResourceService service)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			this.service = service;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateIdentityResourceDto resource)
		{
			if (resource == null)
			{return this.BadRequest((object)"Resource cannot be null");
			}
			if (!this.ModelState.IsValid)
			{return this.BadRequest((object)ErrorDto.FromModelState(this.ModelState));
			}
			IdentityResult result = await service.Create(resource.ToService());
			if (!result.Succeeded)
			{return this.BadRequest((object)ErrorDto.FromIdentityResult(result));
			}
			IdentityResource createdResource = await service.GetByIdentityResourceName(resource.Name);
            return this.CreatedAtAction("GetById", "IdentityResources", (object)new
			{
				id = createdResource.Name
			}, (object)"Identity Resource Successfully Created");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{return this.BadRequest((object)"id cannot be null");
			}
			IdentityResource resource = await service.GetById(id);
			if (resource == null)
			{return this.NoContent();
			}
			IdentityResult result = await service.Delete(resource);
			if (!result.Succeeded)
			{IdentityError obj = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj != null) ? obj.Description : null));
			}return this.NoContent();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] string id)
		{
			IdentityResource resource = await service.GetById(id);
			if (resource == null)
			{return this.NotFound();
			}return this.Ok((object)resource.ToDto());
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string name = null)
		{
			IList<IdentityResource> resources = await service.Get(name);
			if (name == null)
			{}
			else
			{}
			return this.Ok((object)(from x in resources
				select x.ToDto() into o
				orderby o.DisplayName
				select o).ToList());
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] string id, [FromBody] IdentityResourceDto resource)
		{
			if (string.IsNullOrWhiteSpace(id))
			{return this.BadRequest((object)"id cannot be null");
			}
			if (resource == null)
			{return this.BadRequest((object)"Resource cannot be null");
			}
			if (!this.ModelState.IsValid)
			{return this.BadRequest((object)ErrorDto.FromModelState(this.ModelState));
			}
			IdentityResource foundResource = await service.GetById(id);
			if (foundResource == null)
			{return this.NotFound();
			}
			IdentityResult result = await service.Update(resource.ToService());
			if (!result.Succeeded)
			{IdentityError obj = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj != null) ? obj.Description : null));
			}return this.NoContent();
		}
	}
}
