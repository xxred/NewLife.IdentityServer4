





using IdentityServer4.Admin.Logic.Interfaces;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.IdentityServer
{
  public class IdentityServerUnitOfWork : IIdentityServerUnitOfWork, IDisposable
  {
    private readonly ConfigurationDbContext configurationContext;
    private readonly ExtendedConfigurationDbContext extendedConfigurationContext;
    private readonly PersistedGrantDbContext persistedGrantDbContext;
    private bool disposed;

    public IRepository<Client, string> ClientRepository { get; }

    public IExtendedClientRepository ExtendedClientRepository { get; }

    public IExtendedApiResourceRepository ExtendedApiResourceRepository { get; }

    public IRepository<ApiResource, string> ApiResourceRepository { get; }

    public IExtendedIdentityResourceRepository ExtendedIdentityResourceRepository { get; }

    public IRepository<IdentityResource, string> IdentityResourceRepository { get; }

    public IPersistedGrantRepository PersistedGrantRepository { get; }

    public IdentityServerUnitOfWork(DbContextOptions<ConfigurationDbContext> configurationOptions, DbContextOptions<ExtendedConfigurationDbContext> extendedConfigurationOptions, DbContextOptions<PersistedGrantDbContext> operationalOptions, ConfigurationStoreOptions storeOptions, OperationalStoreOptions opStoreOptions)
    {
      if (configurationOptions == null)
        throw new ArgumentNullException(nameof (configurationOptions));
      if (extendedConfigurationOptions == null)
        throw new ArgumentNullException(nameof (extendedConfigurationOptions));
      if (operationalOptions == null)
        throw new ArgumentNullException(nameof (operationalOptions));
      if (storeOptions == null)
        throw new ArgumentNullException(nameof (storeOptions));
      if (opStoreOptions == null)
        throw new ArgumentNullException(nameof (opStoreOptions));
      this.configurationContext = new ConfigurationDbContext(configurationOptions, storeOptions);
      this.extendedConfigurationContext = new ExtendedConfigurationDbContext(extendedConfigurationOptions, storeOptions);
      this.persistedGrantDbContext = new PersistedGrantDbContext(operationalOptions, opStoreOptions);
      this.ClientRepository = (IRepository<Client, string>) new IdentityServer4.Admin.Logic.Logic.IdentityServer.ClientRepository(this.configurationContext);
      this.ExtendedClientRepository = (IExtendedClientRepository) new IdentityServer4.Admin.Logic.Logic.IdentityServer.ExtendedClientRepository(this.extendedConfigurationContext);
      this.ExtendedApiResourceRepository = (IExtendedApiResourceRepository) new IdentityServer4.Admin.Logic.Logic.IdentityServer.ExtendedApiResourceRepository(this.extendedConfigurationContext);
      this.ApiResourceRepository = (IRepository<ApiResource, string>) new IdentityServer4.Admin.Logic.Logic.IdentityServer.ApiResourceRepository(this.configurationContext);
      this.ExtendedIdentityResourceRepository = (IExtendedIdentityResourceRepository) new IdentityServer4.Admin.Logic.Logic.IdentityServer.ExtendedIdentityResourceRepository(this.extendedConfigurationContext);
      this.IdentityResourceRepository = (IRepository<IdentityResource, string>) new IdentityServer4.Admin.Logic.Logic.IdentityServer.IdentityResourceRepository(this.configurationContext);
      this.PersistedGrantRepository = (IPersistedGrantRepository) new IdentityServer4.Admin.Logic.Logic.IdentityServer.PersistedGrantRepository((IPersistedGrantDbContext) this.persistedGrantDbContext);
    }

    public async Task<IdentityResult> Commit()
    {
      try
      {
        int num1 = await this.configurationContext.SaveChangesAsync();
        int num2 = await this.extendedConfigurationContext.SaveChangesAsync(new CancellationToken());
        int num3 = await this.persistedGrantDbContext.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Description = "Optimistic concurrency failure, object has been modified."
        });
      }
      return IdentityResult.Success;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        this.configurationContext?.Dispose();
        this.extendedConfigurationContext?.Dispose();
        this.persistedGrantDbContext?.Dispose();
      }
      this.disposed = true;
    }
  }
}
