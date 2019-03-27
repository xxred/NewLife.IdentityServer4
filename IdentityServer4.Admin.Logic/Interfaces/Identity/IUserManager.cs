using Extensions.Identity.Stores.XCode;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Identity
{
    public interface IUserManager
    {
        bool SupportsUserAuthenticationTokens { get; }

        bool SupportsUserTwoFactor { get; }

        bool SupportsUserPassword { get; }

        bool SupportsUserSecurityStamp { get; }

        bool SupportsUserRole { get; }

        bool SupportsUserLogin { get; }

        bool SupportsUserEmail { get; }

        bool SupportsUserPhoneNumber { get; }

        bool SupportsUserClaim { get; }

        bool SupportsUserLockout { get; }

        bool SupportsQueryableUsers { get; }

        IQueryable<IdentityUser> Users { get; }

        string GetUserName(ClaimsPrincipal principal);

        string GetUserId(ClaimsPrincipal principal);

        Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal);

        Task<string> GenerateConcurrencyStampAsync(IdentityUser user);

        Task<IdentityResult> CreateAsync(IdentityUser user);

        Task<IdentityResult> UpdateAsync(IdentityUser user);

        Task<IdentityResult> DeleteAsync(IdentityUser user);

        Task<IdentityUser> FindByIdAsync(string userId);

        Task<IdentityUser> FindByNameAsync(string userName);

        Task<IdentityResult> CreateAsync(IdentityUser user, string password);

        string NormalizeKey(string key);

        Task UpdateNormalizedUserNameAsync(IdentityUser user);

        Task<string> GetUserNameAsync(IdentityUser user);

        Task<IdentityResult> SetUserNameAsync(IdentityUser user, string userName);

        Task<string> GetUserIdAsync(IdentityUser user);

        Task<bool> CheckPasswordAsync(IdentityUser user, string password);

        Task<bool> HasPasswordAsync(IdentityUser user);

        Task<IdentityResult> AddPasswordAsync(IdentityUser user, string password);

        Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword);

        Task<string> GetSecurityStampAsync(IdentityUser user);

        Task<IdentityResult> UpdateSecurityStampAsync(IdentityUser user);

        Task<string> GeneratePasswordResetTokenAsync(IdentityUser user);

        Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword);

        Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey);

        Task<IdentityResult> RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey);

        Task<IdentityResult> AddLoginAsync(IdentityUser user, UserLoginInfo login);

        Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user);

        Task<IdentityResult> AddClaimAsync(IdentityUser user, Claim claim);

        Task<IdentityResult> AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims);

        Task<IdentityResult> ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim);

        Task<IdentityResult> RemoveClaimAsync(IdentityUser user, Claim claim);

        Task<IdentityResult> RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims);

        Task<IList<Claim>> GetClaimsAsync(IdentityUser user);

        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role);

        Task<IdentityResult> AddToRolesAsync(IdentityUser user, IEnumerable<string> roles);

        Task<IdentityResult> RemoveFromRoleAsync(IdentityUser user, string role);

        Task<IdentityResult> RemoveFromRolesAsync(IdentityUser user, IEnumerable<string> roles);

        Task<IList<string>> GetRolesAsync(IdentityUser user);

        Task<bool> IsInRoleAsync(IdentityUser user, string role);

        Task<string> GetEmailAsync(IdentityUser user);

        Task<IdentityResult> SetEmailAsync(IdentityUser user, string email);

        Task<IdentityUser> FindByEmailAsync(string email);

        Task UpdateNormalizedEmailAsync(IdentityUser user);

        Task<string> GenerateEmailConfirmationTokenAsync(IdentityUser user);

        Task<IdentityResult> ConfirmEmailAsync(IdentityUser user, string token);

        Task<bool> IsEmailConfirmedAsync(IdentityUser user);

        Task<string> GenerateChangeEmailTokenAsync(IdentityUser user, string newEmail);

        Task<IdentityResult> ChangeEmailAsync(IdentityUser user, string newEmail, string token);

        Task<string> GetPhoneNumberAsync(IdentityUser user);

        Task<IdentityResult> SetPhoneNumberAsync(IdentityUser user, string phoneNumber);

        Task<IdentityResult> ChangePhoneNumberAsync(IdentityUser user, string phoneNumber, string token);

        Task<bool> IsPhoneNumberConfirmedAsync(IdentityUser user);

        Task<string> GenerateChangePhoneNumberTokenAsync(IdentityUser user, string phoneNumber);

        Task<bool> VerifyChangePhoneNumberTokenAsync(IdentityUser user, string token, string phoneNumber);

        Task<bool> VerifyUserTokenAsync(IdentityUser user, string tokenProvider, string purpose, string token);

        Task<string> GenerateUserTokenAsync(IdentityUser user, string tokenProvider, string purpose);

        void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<IdentityUser> provider);

        Task<IList<string>> GetValidTwoFactorProvidersAsync(IdentityUser user);

        Task<bool> VerifyTwoFactorTokenAsync(IdentityUser user, string tokenProvider, string token);

        Task<string> GenerateTwoFactorTokenAsync(IdentityUser user, string tokenProvider);

        Task<bool> GetTwoFactorEnabledAsync(IdentityUser user);

        Task<IdentityResult> SetTwoFactorEnabledAsync(IdentityUser user, bool enabled);

        Task<bool> IsLockedOutAsync(IdentityUser user);

        Task<IdentityResult> SetLockoutEnabledAsync(IdentityUser user, bool enabled);

        Task<bool> GetLockoutEnabledAsync(IdentityUser user);

        Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser user);

        Task<IdentityResult> SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd);

        Task<IdentityResult> AccessFailedAsync(IdentityUser user);

        Task<IdentityResult> ResetAccessFailedCountAsync(IdentityUser user);

        Task<int> GetAccessFailedCountAsync(IdentityUser user);

        Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim);

        Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName);

        Task<string> GetAuthenticationTokenAsync(IdentityUser user, string loginProvider, string tokenName);

        Task<IdentityResult> SetAuthenticationTokenAsync(IdentityUser user, string loginProvider, string tokenName, string tokenValue);

        Task<IdentityResult> RemoveAuthenticationTokenAsync(IdentityUser user, string loginProvider, string tokenName);

        // [return: TupleElementNames(new string[] {"users", "totalMatches"})]
        // Task<ValueTuple<IEnumerable<IdentityUser>, int>>
        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindAllMatchingUsers(string toMatch, Pagination pagination);

        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindDeletedMatchingUsers(string toMatch, Pagination pagination);

        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindBlockedMatchingUsers(string toMatch, Pagination pagination);

        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindActiveMatchingUsers(string toMatch, Pagination pagination);

        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindActiveOrBlockedMatchingUsers(string toMatch, Pagination pagination);

        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindActiveOrDeletedMatchingUsers(string toMatch, Pagination pagination);

        Task<(IEnumerable<IdentityUser> users, int totalMatches)> FindDeletedOrBlockedMatchingUsers(string toMatch, Pagination pagination);
    }
}
