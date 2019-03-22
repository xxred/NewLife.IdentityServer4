





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.IdentityServer
{
  public interface IExtendedClientRepository : IRepository<ExtendedClient, string>
  {
    Task<ExtendedClient> GetByClientId(string clientid);

    Task<ExtendedClient> GetByNormalizedClientName(string normalizedClientName);
  }
}
