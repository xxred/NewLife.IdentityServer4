using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Admin.Api.Mappers;
using IdentityServer4.Admin.Logic.Interfaces.Services;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize("identityserver")]
    [Route("[controller]")]
    public class ProtectedResourcesController : Controller
    {
        private readonly IApiResourceService service;

        public ProtectedResourcesController(IApiResourceService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProtectedResourceDto resource)
        {
            if (resource == null)
            {
                return this.BadRequest((object)"Resource cannot be null");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest((object)ErrorDto.FromModelState(this.ModelState));
            }
            if (await service.GetApiResourceByScopeName(resource.DisplayName) != null)
            {
                return this.BadRequest((object)"Cannot create ProtectedResource as generated scope name conflicts with existing scope");
            }
            IdentityResult result = await service.Create(resource.ToService());
            if (!result.Succeeded)
            {
                return this.BadRequest((object)ErrorDto.FromIdentityResult(result));
            }
            ApiResource createdResource = await service.GetByApiResourceName(resource.Name); return this.CreatedAtAction("GetById", "ProtectedResources", (object)new
            {
                id = createdResource.Name
            }, (object)"Protected Resource Successfully Created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            ApiResource resource = await service.GetById(id);
            if (resource == null)
            {
                return this.NoContent();
            }
            IdentityResult result = await service.Delete(resource);
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            ApiResource resource = await service.GetById(id);
            if (resource == null)
            {
                return this.NotFound();
            }
            return this.Ok((object)resource.ToDto());
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = null)
        {
            IList<ApiResource> resources = await service.Get(name);
            if (name != null)
            { }
            else
            { }
            return this.Ok((object)(from x in resources
                                    select x.ToDto() into o
                                    orderby o.DisplayName
                                    select o).ToList());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ProtectedResourceDto resource)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            if (resource == null)
            {
                return this.BadRequest((object)"Resource cannot be null");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest((object)ErrorDto.FromModelState(this.ModelState));
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NotFound();
            }
            IdentityResult result = await service.Update(resource.ToService());
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.NoContent();
        }

        [HttpPost("{id}/secrets")]
        public async Task<IActionResult> AddSecret([FromRoute] string id, [FromBody] CreateSecretDto secret)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            if (secret == null)
            {
                return this.BadRequest((object)"Model cannot be null");
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NotFound();
            }
            IdentityResult result = await service.AddSecret(id, secret.ToService());
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.Ok();
        }

        [HttpPut("{id}/secrets/{secretId}")]
        public async Task<IActionResult> UpdateSecret([FromRoute] string id, [FromRoute] int secretId, [FromBody] SecretDto secret)
        {
            if (id == null)
            {
                return this.BadRequest((object)"id cannot be null");
            }
            if (secretId < 0)
            {
                return this.BadRequest((object)"secret id is invalid");
            }
            if (secret == null)
            {
                return this.BadRequest((object)"secret cannot be null");
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NotFound();
            }
            if (foundResource.ApiSecrets.All((Secret x) => x.Id != secretId))
            {
                return this.NotFound();
            }
            PlainTextSecret plainTextSecret = secret.ToService();
            plainTextSecret.Id = secretId;
            IdentityResult result = await service.UpdateSecret(id, plainTextSecret);
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.NoContent();
        }

        [HttpDelete("{id}/secrets/{secretId}")]
        public async Task<IActionResult> DeleteSecret([FromRoute] string id, [FromRoute] int secretId)
        {
            if (id == null)
            {
                return this.BadRequest((object)"id cannot be null");
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NoContent();
            }
            if (secretId < 0)
            {
                return this.BadRequest((object)"secret id is invalid");
            }
            Secret foundSecret = foundResource.ApiSecrets.FirstOrDefault((Secret x) => x.Id == secretId);
            if (foundSecret == null)
            {
                return this.NoContent();
            }
            IdentityResult result = await service.DeleteSecret(id, foundSecret);
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.NoContent();
        }

        [HttpPost("{id}/scopes")]
        public async Task<IActionResult> AddScope([FromRoute] string id, [FromBody] ScopeDto scope)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NotFound();
            }
            if (scope == null)
            {
                return this.BadRequest((object)"Model cannot be null");
            }
            IdentityResult result = await service.AddScope(id, scope.ToService());
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.Ok();
        }

        [HttpPut("{id}/scopes/{scopeId}")]
        public async Task<IActionResult> UpdateScope([FromRoute] string id, [FromRoute] int scopeId, [FromBody] ScopeDto scope)
        {
            if (id == null)
            {
                return this.BadRequest((object)"id cannot be null");
            }
            if (scopeId < 0)
            {
                return this.BadRequest((object)"secret id is invalid");
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NotFound();
            }
            if (scope == null)
            {
                return this.BadRequest((object)"scope cannot be null");
            }
            if (foundResource.Scopes.All((Scope x) => x.Id != scopeId))
            {
                return this.NotFound();
            }
            Scope mappedScope = scope.ToService();
            mappedScope.Id = scopeId;
            IdentityResult result = await service.UpdateScope(id, mappedScope);
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.NoContent();
        }

        [HttpDelete("{id}/scopes/{scopeId}")]
        public async Task<IActionResult> DeleteScope([FromRoute] string id, [FromRoute] int scopeId)
        {
            if (id == null)
            {
                return this.BadRequest((object)"id cannot be null");
            }
            ApiResource foundResource = await service.GetById(id);
            if (foundResource == null)
            {
                return this.NoContent();
            }
            if (scopeId < 0)
            {
                return this.BadRequest((object)"secret id is invalid");
            }
            Scope foundScope = foundResource.Scopes.FirstOrDefault((Scope x) => x.Id == scopeId);
            if (foundScope == null)
            {
                return this.NoContent();
            }
            IdentityResult result = await service.DeleteScope(id, foundScope);
            if (!result.Succeeded)
            {
                IdentityError obj = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj != null) ? obj.Description : null));
            }
            return this.NoContent();
        }
    }
}
