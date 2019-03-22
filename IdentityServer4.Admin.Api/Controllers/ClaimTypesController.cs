using IdentityServer4.Admin.Api.Mappers;
using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Route("[controller]")]
	public class ClaimTypesController : Controller
	{
		private readonly IClaimTypeService service;

		public ClaimTypesController(IClaimTypeService service)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			this.service = service;
		}

		[Authorize("identityserver")]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateClaimTypeDto claimType)
		{
			if (claimType == null)
			{return this.BadRequest((object)"claimType is required.");
			}
			IdentityResult result = await service.Create(claimType.ToService());
			if (!result.Succeeded)
			{IdentityError obj = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj != null) ? obj.Description : null));
			}
			ClaimType createdClaimType = (await service.Get(claimType.Name, (IList<ClaimTypeOrderBy>)null)).First();return this.CreatedAtAction("GetById", "ClaimTypes", (object)new
			{
				id = createdClaimType.Id
			}, (object)"Claim Type Successfully Created");
		}

		[Authorize("identityserver")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{return this.BadRequest((object)"id is required.");
			}
			ClaimType claimType = await service.GetById(id);
			if (claimType == null)
			{return this.NoContent();
			}
			IdentityResult result = await service.Delete(claimType);
			if (!result.Succeeded)
			{IdentityError obj = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj != null) ? obj.Description : null));
			}return this.NoContent();
		}

		[Authorize("anypermission")]
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string name = null, [FromQuery] string sort = null)
		{
			List<ClaimTypeOrderBy> claimTypeOrderBy = new List<ClaimTypeOrderBy>();
			if (!string.IsNullOrWhiteSpace(sort))
			{
				claimTypeOrderBy.AddRange((from x in sort.Split(',')
					select x.ToClaimTypeOrder()).ToList());
			}
			IList<ClaimType> claimTypes = await service.Get(name, (IList<ClaimTypeOrderBy>)claimTypeOrderBy);return this.Ok((object)(from x in claimTypes
				select x.ToDto()).ToList());
		}

		[Authorize("identityserver")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{return this.BadRequest((object)"id is required.");
			}
			ClaimType claimType = await service.GetById(id);
			if (claimType == null)
			{return this.NotFound();
			}return this.Ok((object)claimType.ToDto());
		}

		[Authorize("identityserver")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ClaimTypeDto claimType)
		{
			if (string.IsNullOrWhiteSpace(id))
			{return this.BadRequest((object)"id cannot be null");
			}
			if (claimType == null)
			{return this.BadRequest((object)"claimType is required.");
			}
			ClaimType foundClaimType = await service.GetById(id);
			if (foundClaimType == null)
			{return this.NotFound();
			}
			IdentityResult result = await service.Update(claimType.ToService());
			if (!result.Succeeded)
			{IdentityError obj = result.Errors.FirstOrDefault();
				return this.BadRequest((object)((obj != null) ? obj.Description : null));
			}return this.NoContent();
		}
	}
}
