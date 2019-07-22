using System.ComponentModel;
using Easy.Admin.Areas.Admin.Controllers;
using Easy.Admin.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewLife.IdentityServer4.Models;
using NewLife.IdentityServer4.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using Easy.Admin.Entities;
using User = NewLife.IdentityServer4.Models.User;

namespace NewLife.IdentityServer4.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [DisplayName("用户")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : EntityController<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        public UsersController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public UserInfo GetUserInfo()
        {
            var user = User;
            var identity = user.Identity as ClaimsIdentity;

            var userInfo = new UserInfo
            {
                Name = identity?.Name,
                Avatar = user.FindFirst(f => f.ValueType.Contains("avatar"))?.Value,
                DisplayName = identity?.Label,
                Roles = new[] { "admin" }
            };

            return userInfo;
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<JwtToken> Login([FromQuery]string username, [FromQuery]string password, [FromQuery]bool rememberMe = false)
        {
            var result = await _signInManager.PasswordSignInAsync(
                username, password, rememberMe, false);

            if (result.Succeeded)
            {
                var jwtToken = HttpContext.Features.Get<JwtToken>();
                return jwtToken;
            }

            throw new ApiException(2, "登陆错误");
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ApiResult> Logout()
        {
            Response.Cookies.Delete("Admin-Token");

            // 退出登陆逻辑，删除登陆信息
            //await _signInManager.SignOutAsync();

            return ApiResult.Ok("退出登陆成功");
        }

        [HttpPost]
        public override ApiResult Post(User value)
        {
            var result = _userManager.CreateAsync(value, value.PasswordHash).GetAwaiter().GetResult();
            return ApiResult.Ok(value.Id);
        }
    }
}
