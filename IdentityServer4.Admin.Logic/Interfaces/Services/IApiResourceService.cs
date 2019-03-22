using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
    public interface IApiResourceService
  {
    Task<IdentityResult> Create(ApiResource resource);

    Task<IdentityResult> Delete(ApiResource resource);

    Task<ApiResource> GetById(string id);

    Task<ApiResource> GetByApiResourceName(string apiResourceName);

    Task<ApiResource> GetApiResourceByScopeName(string scopeName);

    Task<IList<ApiResource>> Get(string name = null);

    Task<IdentityResult> Update(ApiResource resource);

    Task<IdentityResult> AddSecret(string id, PlainTextSecret secret);

    Task<IdentityResult> UpdateSecret(string id, PlainTextSecret secret);

    Task<IdentityResult> DeleteSecret(string id, Secret secret);

    Task<IdentityResult> AddScope(string id, Scope scope);

    Task<IdentityResult> UpdateScope(string id, Scope scope);

    Task<IdentityResult> DeleteScope(string id, Scope scope);
  }
}
