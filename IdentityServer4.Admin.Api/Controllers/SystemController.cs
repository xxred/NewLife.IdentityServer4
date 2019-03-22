using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Admin.Api.Controllers
{
	[AllowAnonymous]
	[Route("[controller]")]
	public class SystemController : Controller
	{
		[HttpGet("nop")]
		public IActionResult Nop()
		{
			return this.Ok((object)true);
		}

		public SystemController()
		{
		}
	}
}
