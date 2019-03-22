





using IdentityExpress.Identity;
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

    IQueryable<IdentityExpressUser> Users { get; }

    string GetUserName(ClaimsPrincipal principal);

    string GetUserId(ClaimsPrincipal principal);

    Task<IdentityExpressUser> GetUserAsync(ClaimsPrincipal principal);

    Task<string> GenerateConcurrencyStampAsync(IdentityExpressUser user);

    Task<IdentityResult> CreateAsync(IdentityExpressUser user);

    Task<IdentityResult> UpdateAsync(IdentityExpressUser user);

    Task<IdentityResult> DeleteAsync(IdentityExpressUser user);

    Task<IdentityExpressUser> FindByIdAsync(string userId);

    Task<IdentityExpressUser> FindByNameAsync(string userName);

    Task<IdentityResult> CreateAsync(IdentityExpressUser user, string password);

    string NormalizeKey(string key);

    Task UpdateNormalizedUserNameAsync(IdentityExpressUser user);

    Task<string> GetUserNameAsync(IdentityExpressUser user);

    Task<IdentityResult> SetUserNameAsync(IdentityExpressUser user, string userName);

    Task<string> GetUserIdAsync(IdentityExpressUser user);

    Task<bool> CheckPasswordAsync(IdentityExpressUser user, string password);

    Task<bool> HasPasswordAsync(IdentityExpressUser user);

    Task<IdentityResult> AddPasswordAsync(IdentityExpressUser user, string password);

    Task<IdentityResult> ChangePasswordAsync(IdentityExpressUser user, string currentPassword, string newPassword);

    Task<string> GetSecurityStampAsync(IdentityExpressUser user);

    Task<IdentityResult> UpdateSecurityStampAsync(IdentityExpressUser user);

    Task<string> GeneratePasswordResetTokenAsync(IdentityExpressUser user);

    Task<IdentityResult> ResetPasswordAsync(IdentityExpressUser user, string token, string newPassword);

    Task<IdentityExpressUser> FindByLoginAsync(string loginProvider, string providerKey);

    Task<IdentityResult> RemoveLoginAsync(IdentityExpressUser user, string loginProvider, string providerKey);

    Task<IdentityResult> AddLoginAsync(IdentityExpressUser user, UserLoginInfo login);

    Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityExpressUser user);

    Task<IdentityResult> AddClaimAsync(IdentityExpressUser user, Claim claim);

    Task<IdentityResult> AddClaimsAsync(IdentityExpressUser user, IEnumerable<Claim> claims);

    Task<IdentityResult> ReplaceClaimAsync(IdentityExpressUser user, Claim claim, Claim newClaim);

    Task<IdentityResult> RemoveClaimAsync(IdentityExpressUser user, Claim claim);

    Task<IdentityResult> RemoveClaimsAsync(IdentityExpressUser user, IEnumerable<Claim> claims);

    Task<IList<Claim>> GetClaimsAsync(IdentityExpressUser user);

    Task<IdentityResult> AddToRoleAsync(IdentityExpressUser user, string role);

    Task<IdentityResult> AddToRolesAsync(IdentityExpressUser user, IEnumerable<string> roles);

    Task<IdentityResult> RemoveFromRoleAsync(IdentityExpressUser user, string role);

    Task<IdentityResult> RemoveFromRolesAsync(IdentityExpressUser user, IEnumerable<string> roles);

    Task<IList<string>> GetRolesAsync(IdentityExpressUser user);

    Task<bool> IsInRoleAsync(IdentityExpressUser user, string role);

    Task<string> GetEmailAsync(IdentityExpressUser user);

    Task<IdentityResult> SetEmailAsync(IdentityExpressUser user, string email);

    Task<IdentityExpressUser> FindByEmailAsync(string email);

    Task UpdateNormalizedEmailAsync(IdentityExpressUser user);

    Task<string> GenerateEmailConfirmationTokenAsync(IdentityExpressUser user);

    Task<IdentityResult> ConfirmEmailAsync(IdentityExpressUser user, string token);

    Task<bool> IsEmailConfirmedAsync(IdentityExpressUser user);

    Task<string> GenerateChangeEmailTokenAsync(IdentityExpressUser user, string newEmail);

    Task<IdentityResult> ChangeEmailAsync(IdentityExpressUser user, string newEmail, string token);

    Task<string> GetPhoneNumberAsync(IdentityExpressUser user);

    Task<IdentityResult> SetPhoneNumberAsync(IdentityExpressUser user, string phoneNumber);

    Task<IdentityResult> ChangePhoneNumberAsync(IdentityExpressUser user, string phoneNumber, string token);

    Task<bool> IsPhoneNumberConfirmedAsync(IdentityExpressUser user);

    Task<string> GenerateChangePhoneNumberTokenAsync(IdentityExpressUser user, string phoneNumber);

    Task<bool> VerifyChangePhoneNumberTokenAsync(IdentityExpressUser user, string token, string phoneNumber);

    Task<bool> VerifyUserTokenAsync(IdentityExpressUser user, string tokenProvider, string purpose, string token);

    Task<string> GenerateUserTokenAsync(IdentityExpressUser user, string tokenProvider, string purpose);

    void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<IdentityExpressUser> provider);

    Task<IList<string>> GetValidTwoFactorProvidersAsync(IdentityExpressUser user);

    Task<bool> VerifyTwoFactorTokenAsync(IdentityExpressUser user, string tokenProvider, string token);

    Task<string> GenerateTwoFactorTokenAsync(IdentityExpressUser user, string tokenProvider);

    Task<bool> GetTwoFactorEnabledAsync(IdentityExpressUser user);

    Task<IdentityResult> SetTwoFactorEnabledAsync(IdentityExpressUser user, bool enabled);

    Task<bool> IsLockedOutAsync(IdentityExpressUser user);

    Task<IdentityResult> SetLockoutEnabledAsync(IdentityExpressUser user, bool enabled);

    Task<bool> GetLockoutEnabledAsync(IdentityExpressUser user);

    Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityExpressUser user);

    Task<IdentityResult> SetLockoutEndDateAsync(IdentityExpressUser user, DateTimeOffset? lockoutEnd);

    Task<IdentityResult> AccessFailedAsync(IdentityExpressUser user);

    Task<IdentityResult> ResetAccessFailedCountAsync(IdentityExpressUser user);

    Task<int> GetAccessFailedCountAsync(IdentityExpressUser user);

    Task<IList<IdentityExpressUser>> GetUsersForClaimAsync(Claim claim);

    Task<IList<IdentityExpressUser>> GetUsersInRoleAsync(string roleName);

    Task<string> GetAuthenticationTokenAsync(IdentityExpressUser user, string loginProvider, string tokenName);

    Task<IdentityResult> SetAuthenticationTokenAsync(IdentityExpressUser user, string loginProvider, string tokenName, string tokenValue);

    Task<IdentityResult> RemoveAuthenticationTokenAsync(IdentityExpressUser user, string loginProvider, string tokenName);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindAllMatchingUsers(string toMatch, Pagination pagination);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindDeletedMatchingUsers(string toMatch, Pagination pagination);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindBlockedMatchingUsers(string toMatch, Pagination pagination);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveMatchingUsers(string toMatch, Pagination pagination);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveOrBlockedMatchingUsers(string toMatch, Pagination pagination);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindActiveOrDeletedMatchingUsers(string toMatch, Pagination pagination);

    [return: TupleElementNames(new string[] {"users", "totalMatches"})]
    Task<ValueTuple<IEnumerable<IdentityExpressUser>, int>> FindDeletedOrBlockedMatchingUsers(string toMatch, Pagination pagination);
  }
}
