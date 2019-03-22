using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Admin.Logic.Constants;
using IdentityServer4.Admin.Api.Mappers;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize("public")]
	[Route("[controller]")]
	public class UserSettingsController : Controller
	{
		private readonly IUserClaimsService userClaimsService;

		private readonly IUserService userService;

		private readonly ILogger<UserSettingsController> logger;

		private readonly IHttpContextAccessor contextAccessor;

		public UserSettingsController(IUserService userService, IUserClaimsService userClaimsService, ILogger<UserSettingsController> logger, IHttpContextAccessor contextAccessor)
		{
			if (userService == null)
			{
				throw new ArgumentNullException("userService");
			}
			this.userService = userService;
			if (userClaimsService == null)
			{
				throw new ArgumentNullException("userClaimsService");
			}
			this.userClaimsService = userClaimsService;
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			this.logger = logger;
			if (contextAccessor == null)
			{
				throw new ArgumentNullException("contextAccessor");
			}
			this.contextAccessor = contextAccessor;
		}

		[HttpGet]
		[Route("{subject}")]
		public async Task<IActionResult> Get([FromRoute] string subject)
		{
			if (contextAccessor.HttpContext.User != null)
			{
				string currentSubject = contextAccessor.HttpContext.User.FindFirst((Claim x) => x.Type == "sub")
					.Value;
					LoggerExtensions.LogInformation(logger, (1002), MessageConstants.FindingUserBySubjectAndVariable, new object[1]
					{
						subject
					});
					User user = await userService.GetBySubject(subject);
					if (currentSubject != subject)
					{
						LoggerExtensions.LogError(logger, "User is unathorized to perform this request", Array.Empty<object>());return this.Forbid();
					}
					if (user != null)
					{
						LoggerExtensions.LogInformation(logger, "Getting user editable claims for " + subject, Array.Empty<object>());
						UserClaim claims = await userClaimsService.GetUserEditableClaims(user);return this.Ok((object)claims.ToDto());
					}
				}
				LoggerExtensions.LogInformation(logger, (4002), MessageConstants.UserIsNullCannotGetBySubject, new object[1]
				{
					subject
				});return this.Forbid();
			}

			[HttpPut]
			public async Task<IActionResult> Update([FromBody] UserClaimDto userClaims)
			{
				if (userClaims.Claims.Any((ClaimDto x) => x.Value == null))
				{return this.BadRequest((object)"Cant update claim with null value");
				}
				if (contextAccessor.HttpContext.User != null)
				{
					string currentSubject = contextAccessor.HttpContext.User.FindFirst((Claim x) => x.Type == "sub")
						.Value;
						LoggerExtensions.LogInformation(logger, (1002), MessageConstants.FindingUserBySubjectAndVariable, new object[1]
						{
							currentSubject
						});
						User user = await userService.GetBySubject(currentSubject);
						if (currentSubject != userClaims.Subject)
						{
							LoggerExtensions.LogError(logger, "User is unathorized to perform this request", Array.Empty<object>());return this.Forbid();
						}
						if (user != null)
						{
							LoggerExtensions.LogInformation(logger, "Updating user claims for " + currentSubject, Array.Empty<object>());
							IdentityResult result = await userClaimsService.UpdateUserEditableClaims(userClaims.ToService());
							if (!result.Succeeded)
							{IdentityError obj = result.Errors.FirstOrDefault();
								return this.BadRequest((object)((obj != null) ? obj.Description : null));
							}
							LoggerExtensions.LogInformation(logger, (4002), MessageConstants.UserIsNullCannotGetBySubject, new object[1]
							{
								currentSubject
							});return this.NoContent();
						}
					}return this.Forbid();
				}
			}
		}
