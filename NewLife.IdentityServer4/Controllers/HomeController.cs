using System.Threading.Tasks;
using Easy.Admin.Areas.Admin.Controllers;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewLife.IdentityServer4.Attributes;

namespace NewLife.IdentityServer4.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    [NotMenu]
    public class HomeController : BaseController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;

        public HomeController(IIdentityServerInteractionService interaction, IHostingEnvironment environment, ILogger<HomeController> logger)
        {
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        [HttpGet]
        public async Task<ErrorMessage> Error(string errorId)
        {
            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return message;
        }
    }
}