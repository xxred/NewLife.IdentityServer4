using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
    public interface IUserClaimsService
  {
    Task<UserClaim> GetUserEditableClaims(User user);

    Task<IdentityResult> UpdateUserEditableClaims(UserClaim userClaim);
  }
}
