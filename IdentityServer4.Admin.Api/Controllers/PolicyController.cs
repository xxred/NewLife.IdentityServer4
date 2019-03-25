using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize("all")]
    [Route("[controller]")]
    public class PolicyController : Controller
    {
        private readonly IAccessPolicyStore policyConfiguration;

        private readonly ILogger<PolicyController> logger;

        public PolicyController(IAccessPolicyStore policyConfiguration, ILogger<PolicyController> logger)
        {
            if (policyConfiguration == null)
            {
                throw new ArgumentNullException("policyConfiguration");
            }
            this.policyConfiguration = policyConfiguration;
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentPolicy()
        {
            AccessPolicyDefinition accessPolicy = await policyConfiguration.GetAccessPolicyDefinition(); return this.Ok((object)accessPolicy);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePolicy([FromBody] AccessPolicyDefinitionDto accessPolicy)
        {
            if (accessPolicy == null)
            {
                return this.BadRequest((object)"Missing Policy Configuration");
            }
            if (accessPolicy.Version == null)
            {
                return this.BadRequest((object)"Missing Version");
            }
            List<PolicyClaim> policyClaims = new List<PolicyClaim>();
            PolicyClaimDto[] policyClaims2 = accessPolicy.PolicyClaims;
            foreach (PolicyClaimDto policyClaim in policyClaims2)
            {
                if (!Enum.TryParse<Permission>(policyClaim.Permission, out Permission permission))
                {
                    return this.BadRequest((object)("Permission Not Recognised: " + policyClaim.Permission));
                }
                if (string.IsNullOrWhiteSpace(policyClaim.Type))
                {
                    return this.BadRequest((object)"Missing Policy Claim Type");
                }
                if (string.IsNullOrWhiteSpace(policyClaim.Value))
                {
                    return this.BadRequest((object)"Missing Policy Claim Value");
                }
                List<PolicyClaim> list = policyClaims;
                PolicyClaim val = new PolicyClaim();
                val.Permission = permission;
                val.Type = policyClaim.Type;
                val.Value = policyClaim.Value;
                list.Add(val);
            }
            AccessPolicyDefinition updatedPolicyDefinition;
            try
            {
                IAccessPolicyStore accessPolicyStore = policyConfiguration;
                AccessPolicyDefinition val2 = new AccessPolicyDefinition();
                val2.PolicyClaims = policyClaims;
                val2.Version = accessPolicy.Version;
                updatedPolicyDefinition = await accessPolicyStore.UpdateAccessPolicy(val2);
            }
            catch (PolicyConcurrencyException e)
            {
                LoggerExtensions.LogError(logger, e.Message, Array.Empty<object>()); return this.StatusCode(409);
            }
            return this.Ok((object)updatedPolicyDefinition);
        }
    }
}
