using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ClientsController : Controller
    {
        private readonly IClientService service;

        private readonly IPolicyService policyService;

        public ClientsController(IClientService service, IPolicyService policyService)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            this.service = service;
            this.policyService = policyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto client)
        {
            if (client == null)
            {
                return this.BadRequest((object)"Model cannot be null");
            }
            GenericClient genericClient = client.ToService();
            IdentityResult result = await service.Create(genericClient);
            if (!result.Succeeded)
            {
                if (result.Errors.Any((IdentityError x) => x.Code == "420"))
                {
                    IdentityError obj = result.Errors.First((IdentityError error) => error.Code == "420");
                    ObjectResult val = new ObjectResult((object)((obj != null) ? obj.Description : null));
                    val.StatusCode = 420;
                    return val;
                }
                IdentityError obj2 = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj2 != null) ? obj2.Description : null));
            }
            Client createdClient = await service.GetByClientId(client.ClientId); return this.CreatedAtAction("GetById", "Clients", (object)new
            {
                id = createdClient.ClientId
            }, (object)"Client Successfully Created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            Client client = await service.GetById(id);
            if (client == null)
            {
                return this.NoContent();
            }
            IdentityResult result = await service.Delete(client);
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
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            Client client = await service.GetById(id);
            if (client == null)
            {
                return this.NotFound();
            }
            return this.Ok((object)client.ToDto());
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = null)
        {
            IList<Client> resources = await service.Get(name);
            if (name == null)
            { }
            else
            { }
            return this.Ok((object)(from x in resources
                                    select x.ToDto() into o
                                    orderby o.ClientName
                                    select o).ToList());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ClientDto client)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest((object)"id cannot be null");
            }
            if (client == null)
            {
                return this.BadRequest((object)"Model cannot be null");
            }
            Client foundClient = await service.GetById(id);
            if (foundClient == null)
            {
                return this.NotFound();
            }
            ICollection<ClaimDto> changedClaims = new List<ClaimDto>();
            ICollection<ClaimDto> foundClientAsDtoClaims = foundClient.ToDto().Claims;
            ICollection<ClaimDto> updatedClientClaims = client.Claims ?? new List<ClaimDto>();
            foreach (ClaimDto foundClientClaim in foundClientAsDtoClaims)
            {
                if (updatedClientClaims.FirstOrDefault((ClaimDto x) => x.Type == foundClientClaim.Type && x.Value == foundClientClaim.Value) == null)
                {
                    changedClaims.Add(foundClientClaim);
                }
            }
            foreach (ClaimDto updatedClientClaim in updatedClientClaims)
            {
                if (foundClientAsDtoClaims.FirstOrDefault((ClaimDto x) => x.Type == updatedClientClaim.Type && x.Value == updatedClientClaim.Value) == null)
                {
                    changedClaims.Add(updatedClientClaim);
                }
            }
            foreach (ClaimDto claim in changedClaims)
            {
                IPolicyService obj = policyService;
                HttpContext obj2 = HttpContext;
                if (!(await obj.CanUserModifyClaim((obj2 == null) ? null : obj2.User?.Claims, claim)))
                {
                    return this.Unauthorized();
                }
            }
            IdentityResult result = await service.Update(client.ToService());
            if (!result.Succeeded)
            {
                IdentityError obj3 = result.Errors.FirstOrDefault();
                return this.BadRequest((object)((obj3 != null) ? obj3.Description : null));
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
            Client foundClient = await service.GetById(id);
            if (foundClient == null)
            {
                return this.NotFound();
            }
            if (secret == null)
            {
                return this.BadRequest((object)"Model cannot be null");
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
            Client foundClient = await service.GetById(id);
            if (foundClient == null)
            {
                return this.NotFound();
            }
            if (secretId < 0)
            {
                return this.BadRequest((object)"secret id invalid");
            }
            if (secret == null)
            {
                return this.BadRequest((object)"secret cannot be null");
            }
            if (foundClient.ClientSecrets.All((Secret x) => x.Id != secretId))
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
            Client foundClient = await service.GetById(id);
            if (foundClient == null)
            {
                return this.NoContent();
            }
            if (secretId < 0)
            {
                return this.BadRequest((object)"secret id invalid");
            }
            Secret foundSecret = foundClient.ClientSecrets.FirstOrDefault((Secret x) => x.Id == secretId);
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
    }
}
