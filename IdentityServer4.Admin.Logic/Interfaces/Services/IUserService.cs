using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
    public interface IUserService
    {
        Task<IdentityResult> Create(User user, string password);

        Task<IdentityResult> Delete(User user);

        Task<User> GetBySubject(string subject);

        Task<PagedResult<User>> Get(Pagination pagination = null, UserQueryBy query = null, IList<UserOrderBy> ordering = null, UserState state = null, bool includeRelationships = true);

        Task<PagedResult<User>> FindFast(Pagination pagination, string query, UserState state);

        Task<IdentityResult> Update(User user);

        Task<IdentityResult> AddRoles(User user, IList<string> roles);

        Task<IdentityResult> DeleteRoles(User user, IList<string> roles);

        Task<IdentityResult> AddClaim(User user, Claim claim);

        Task<IdentityResult> DeleteClaim(User user, Claim claim);

        Task<IdentityResult> EditClaim(User user, Claim oldClaim, Claim newClaim);

        Task<IdentityResult> ResetPassword(User user);

        Task<IList<UserLoginInfo>> GetUserLogins(string subject);

        Task<IdentityResult> DeleteUserLogin(string subject, string loginProvider, string providerKey);
    }
}
