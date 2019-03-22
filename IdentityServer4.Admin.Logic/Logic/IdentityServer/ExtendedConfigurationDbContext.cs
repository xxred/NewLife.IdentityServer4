





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Logic.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityServer4.Admin.Logic.Logic.IdentityServer
{
  public class ExtendedConfigurationDbContext : DbContext
  {
    private readonly ConfigurationStoreOptions storeOptions;

    public ExtendedConfigurationDbContext(DbContextOptions<ExtendedConfigurationDbContext> options, ConfigurationStoreOptions storeOptions)
      : base((DbContextOptions) options)
    {
      ConfigurationStoreOptions configurationStoreOptions = storeOptions;
      if (configurationStoreOptions == null)
        throw new ArgumentNullException(nameof (storeOptions));
      this.storeOptions = configurationStoreOptions;
    }

    public DbSet<ExtendedClient> ExtendedClients { get; set; }

    public DbSet<ExtendedApiResource> ExtendedApiResources { get; set; }

    public DbSet<ExtendedIdentityResource> ExtendedIdentityResources { get; set; }

    public DbSet<ConfigurationEntry> ConfigurationEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.AddExtendedConfiguration(this.storeOptions);
      base.OnModelCreating(modelBuilder);
    }
  }
}
