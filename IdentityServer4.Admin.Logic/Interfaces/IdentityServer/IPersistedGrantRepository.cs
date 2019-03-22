





using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.IdentityServer
{
  public interface IPersistedGrantRepository : IRepository<PersistedGrant, string>
  {
    Task<IdentityResult> DeleteBySubject(string subject);

    Task<IdentityResult> DeleteBySubjectAndClient(string subject, string clientId);
  }
}
