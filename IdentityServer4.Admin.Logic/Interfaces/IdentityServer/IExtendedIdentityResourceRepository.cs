





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.IdentityServer
{
  public interface IExtendedIdentityResourceRepository : IRepository<ExtendedIdentityResource, string>
  {
    Task<ExtendedIdentityResource> GetByIdentityResourceName(string identityResourceName);
  }
}
