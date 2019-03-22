





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.IdentityServer
{
  public interface IExtendedApiResourceRepository : IRepository<ExtendedApiResource, string>
  {
    Task<ExtendedApiResource> GetByApiResourceName(string apiResourceName);
  }
}
