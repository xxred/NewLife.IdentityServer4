





using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
  public interface IIdentityResourceService
  {
    Task<IdentityResult> Create(IdentityResource resource);

    Task<IdentityResult> Delete(IdentityResource resource);

    Task<IdentityResource> GetById(string id);

    Task<IdentityResource> GetByIdentityResourceName(string identityResourceName);

    Task<IList<IdentityResource>> Get(string name = null);

    Task<IdentityResult> Update(IdentityResource resource);
  }
}
