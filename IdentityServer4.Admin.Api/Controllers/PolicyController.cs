using IdentityServer4.Admin.Api.Policy;
using IdentityExpress.Manager.BusinessLogic.Constants;
using IdentityExpress.Manager.BusinessLogic.Entities.Configuration;
using IdentityExpress.Manager.BusinessLogic.Logic.Services;
using IdentityServer4.Admin.Logic.Entities.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSK.Audit;
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

		private readonly IRecordAuditEventsService audit;

		private readonly ILogger<PolicyController> logger;

		public PolicyController(IAccessPolicyStore policyConfiguration, IRecordAuditEventsService audit, ILogger<PolicyController> logger)
			: this()
		{
			if (policyConfiguration == null)
			{
				throw new ArgumentNullException("policyConfiguration");
			}
			this.policyConfiguration = policyConfiguration;
			if (audit == null)
			{
				throw new ArgumentNullException("audit");
			}
			this.audit = audit;
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			this.logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetCurrentPolicy()
		{
			AccessPolicyDefinition accessPolicy = await policyConfiguration.GetAccessPolicyDefinition();return this.Ok((object)accessPolicy);
		}

		[HttpPut]
		public async Task<IActionResult> UpdatePolicy([FromBody] AccessPolicyDefinitionDto accessPolicy)
		{
			if (accessPolicy == null)
			{return this.BadRequest((object)"Missing Policy Configuration");
			}
			if (accessPolicy.Version == null)
			{return this.BadRequest((object)"Missing Version");
			}
			List<PolicyClaim> policyClaims = new List<PolicyClaim>();
			PolicyClaimDto[] policyClaims2 = accessPolicy.PolicyClaims;
			foreach (PolicyClaimDto policyClaim in policyClaims2)
			{
				if (!Enum.TryParse<Permission>(policyClaim.Permission, out Permission permission))
				{return this.BadRequest((object)("Permission Not Recognised: " + policyClaim.Permission));
				}
				if (string.IsNullOrWhiteSpace(policyClaim.Type))
				{return this.BadRequest((object)"Missing Policy Claim Type");
				}
				if (string.IsNullOrWhiteSpace(policyClaim.Value))
				{return this.BadRequest((object)"Missing Policy Claim Value");
				}
				List<PolicyClaim> list = policyClaims;
				PolicyClaim val = new PolicyClaim();
				val.set_Permission(permission);
				val.set_Type(policyClaim.Type);
				val.set_Value(policyClaim.Value);
				list.Add(val);
			}
			AccessPolicyDefinition updatedPolicyDefinition;
			try
			{
				IAccessPolicyStore accessPolicyStore = policyConfiguration;
				AccessPolicyDefinition val2 = new AccessPolicyDefinition();
				val2.set_PolicyClaims(policyClaims);
				val2.set_Version(accessPolicy.Version);
				updatedPolicyDefinition = await accessPolicyStore.UpdateAccessPolicy(val2);
			}
			catch (PolicyConcurrencyException e)
			{
				LoggerExtensions.LogError(logger, e.Message, Array.Empty<object>());return this.StatusCode(409);
			}return this.Ok((object)updatedPolicyDefinition);
		}
	}
}
