using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize]
	[Route("[controller]")]
	public class UIConfigurationController : Controller
	{
		private readonly WebhookOptions options;

		public UIConfigurationController(IOptions<WebhookOptions> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			this.options = options.Value;
		}

		[HttpGet]
		public IActionResult Get()
		{
			UIOptions uIOptions = new UIOptions
			{
				CanResetPassword = !string.IsNullOrEmpty(options.PasswordResetEndpoint)
			};
			return this.Ok((object)uIOptions);
		}
	}
}
