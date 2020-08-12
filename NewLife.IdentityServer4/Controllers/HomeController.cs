using System.Threading.Tasks;
using Easy.Admin.Areas.Admin.Controllers;
using Easy.Admin.Entities;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewLife.IdentityServer4.Attributes;
using NewLife.Log;

namespace NewLife.IdentityServer4.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    [NotMenu]
    public class HomeController : AdminControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger)
        {
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        [HttpGet]
        public async Task<ApiResult<string>> Error(string errorId)
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

                XTrace.WriteLine($"授权出错：{message.Error}。描述：{message.ErrorDescription}");

                throw ApiException.Common(message.Error, 402);
            }
            else
            {
                return ApiResult<string>.Ok("没有错误，但是跳到了'/home/error'这里");
            }
        }
    }
}