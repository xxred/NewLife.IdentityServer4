using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
    public interface IRoleService
  {
    Task<IdentityResult> Create(Role role);

    Task<IdentityResult> Delete(Role role);

    Task<Role> GetById(string id);

    Task<IList<Role>> Get(string name = null, IList<RoleOrderBy> ordering = null, IList<RoleFields> fields = null);

    Task<IdentityResult> Update(Role role);

    Task<IdentityResult> AddUsers(Role role, IList<User> users);

    Task<IdentityResult> RemoveUsers(Role role, IList<User> users);
  }
}
