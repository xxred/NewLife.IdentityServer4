using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Easy.Admin.Authentication.OAuthSignIn;
using Easy.Admin.Services;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using XCode.Membership;

namespace NewLife.IdentityServer4.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IProfileService" />
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ProfileService> _logger;

        /// <summary>
        /// </summary>
        public ProfileService(IUserService userService, ILogger<ProfileService> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No sub claim present");

            await GetProfileDataAsync(context, sub);
        }

        /// <summary>
        /// Called to get the claims for the subject based on the profile request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, string subjectId)
        {
            var user = await FindUserAsync(subjectId);
            if (user != null)
            {
                await GetProfileDataAsync(context, user);
            }
        }

        /// <summary>
        /// Called to get the claims for the user based on the profile request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, IUser user)
        {
            var claims = await GetUserClaimsAsync(user);
            context.AddRequestedClaims(claims);
        }

        /// <summary>
        /// Gets the claims for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual Task<List<Claim>> GetUserClaimsAsync(IUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(OAuthSignInAuthenticationDefaults.Avatar, user.Avatar ?? ""),
                new Claim(OAuthSignInAuthenticationDefaults.Gender,user.Sex.ToInt().ToString()),
                new Claim(OAuthSignInAuthenticationDefaults.GivenName, user.DisplayName ?? user.Name),
                new Claim(OAuthSignInAuthenticationDefaults.UniqueName, user.Name),
            };

            return Task.FromResult(claims);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No subject Id claim present");

            context.IsActive = await FindUserAsync(sub) != null;
        }

        /// <summary>
        /// Loads the user by the subject id.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task<IUser> FindUserAsync(string subjectId)
        {
            var user = await _userService.FindByIdAsync(subjectId);
            if (user == null)
            {
                _logger?.LogWarning("No user found matching subject Id: {subjectId}", subjectId);
            }

            return user;
        }
    }
}
