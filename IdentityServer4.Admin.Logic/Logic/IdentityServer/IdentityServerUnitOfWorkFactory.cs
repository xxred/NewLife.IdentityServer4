





using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityServer4.Admin.Logic.Logic.IdentityServer
{
  public class IdentityServerUnitOfWorkFactory : IIdentityServerUnitOfWorkFactory
  {
    private readonly DbContextOptions<ConfigurationDbContext> configurationContext;
    private readonly DbContextOptions<ExtendedConfigurationDbContext> extendedConfigurationContext;
    private readonly DbContextOptions<PersistedGrantDbContext> operationalContext;
    private readonly ConfigurationStoreOptions storeOptions;
    private readonly OperationalStoreOptions opStoreOptions;

    public IdentityServerUnitOfWorkFactory(DbContextOptions<ConfigurationDbContext> configurationContext, DbContextOptions<ExtendedConfigurationDbContext> extendedConfigurationContext, DbContextOptions<PersistedGrantDbContext> operationalContext, ConfigurationStoreOptions storeOptions, OperationalStoreOptions opStoreOptions)
    {
      if (configurationContext == null)
        throw new ArgumentNullException(nameof (configurationContext));
      if (extendedConfigurationContext == null)
        throw new ArgumentNullException(nameof (extendedConfigurationContext));
      if (operationalContext == null)
        throw new ArgumentNullException(nameof (operationalContext));
      if (storeOptions == null)
        throw new ArgumentNullException(nameof (storeOptions));
      if (opStoreOptions == null)
        throw new ArgumentNullException(nameof (opStoreOptions));
      this.configurationContext = configurationContext;
      this.extendedConfigurationContext = extendedConfigurationContext;
      this.operationalContext = operationalContext;
      this.storeOptions = storeOptions;
      this.opStoreOptions = opStoreOptions;
    }

    public IIdentityServerUnitOfWork Create()
    {
      return (IIdentityServerUnitOfWork) new IdentityServerUnitOfWork(this.configurationContext, this.extendedConfigurationContext, this.operationalContext, this.storeOptions, this.opStoreOptions);
    }
  }
}
