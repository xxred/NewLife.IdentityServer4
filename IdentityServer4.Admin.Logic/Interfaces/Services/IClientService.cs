using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
    public interface IClientService
  {
    Task<IdentityResult> Create(GenericClient client);

    Task<IdentityResult> Delete(Client client);

    Task<Client> GetById(string id);

    Task<Client> GetByClientId(string clientId);

    Task<IList<Client>> Get(string name = null);

    Task<IdentityResult> Update(Client client);

    Task<IdentityResult> AddSecret(string id, PlainTextSecret secret);

    Task<IdentityResult> UpdateSecret(string id, PlainTextSecret secret);

    Task<IdentityResult> DeleteSecret(string id, Secret secret);
  }
}
