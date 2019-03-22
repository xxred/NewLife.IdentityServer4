





using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.IdentityServer
{
  public interface IIdentityServerUnitOfWork : IDisposable
  {
    IRepository<Client, string> ClientRepository { get; }

    IExtendedClientRepository ExtendedClientRepository { get; }

    IExtendedApiResourceRepository ExtendedApiResourceRepository { get; }

    IExtendedIdentityResourceRepository ExtendedIdentityResourceRepository { get; }

    IRepository<ApiResource, string> ApiResourceRepository { get; }

    IRepository<IdentityResource, string> IdentityResourceRepository { get; }

    IPersistedGrantRepository PersistedGrantRepository { get; }

    Task<IdentityResult> Commit();
  }
}
