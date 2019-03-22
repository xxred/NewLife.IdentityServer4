





using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Entities.Services;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services.UserQueries
{
    public interface IUserQuery
  {
    Task<PagedResult<User>> GetUsers(IUserManager userManager);
  }
}
