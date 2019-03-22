





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace IdentityServer4.Admin.Logic.Logic.Extensions
{
  public static class ModelBuilderExtensions
  {
    public static ModelBuilder AddExtendedConfiguration(this ModelBuilder modelBuilder, ConfigurationStoreOptions storeOptions)
    {
      if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema))
        modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);
      modelBuilder.Entity<ExtendedClient>((Action<EntityTypeBuilder<ExtendedClient>>) (client =>
      {
        client.ToTable<ExtendedClient>("ExtendedClients");
        client.HasKey((Expression<Func<ExtendedClient, object>>) (x => x.Id));
        client.Property<string>((Expression<Func<ExtendedClient, string>>) (x => x.ClientId)).HasMaxLength(200).IsRequired(true);
        client.Property<string>((Expression<Func<ExtendedClient, string>>) (x => x.NormalizedClientId)).HasMaxLength(200).IsRequired(true);
        client.Property<string>((Expression<Func<ExtendedClient, string>>) (x => x.NormalizedClientName)).HasMaxLength(200);
        client.HasIndex((Expression<Func<ExtendedClient, object>>) (x => x.ClientId)).HasName("IdIndex").IsUnique(true);
        client.HasIndex((Expression<Func<ExtendedClient, object>>) (x => x.NormalizedClientId)).HasName("ClientIdIndex").IsUnique(true);
        client.HasIndex((Expression<Func<ExtendedClient, object>>) (x => x.NormalizedClientName)).HasName("ClientNameIndex").IsUnique(true);
      }));
      modelBuilder.Entity<ExtendedApiResource>((Action<EntityTypeBuilder<ExtendedApiResource>>) (resource =>
      {
        resource.ToTable<ExtendedApiResource>("ExtendedApiResources");
        resource.HasKey((Expression<Func<ExtendedApiResource, object>>) (x => x.Id));
        resource.Property<string>((Expression<Func<ExtendedApiResource, string>>) (x => x.ApiResourceName)).HasMaxLength(200).IsRequired(true);
        resource.Property<string>((Expression<Func<ExtendedApiResource, string>>) (x => x.NormalizedName)).HasMaxLength(200).IsRequired(true);
        resource.HasIndex((Expression<Func<ExtendedApiResource, object>>) (x => x.ApiResourceName)).HasName("ApiNameIndex").IsUnique(true);
        resource.HasIndex((Expression<Func<ExtendedApiResource, object>>) (x => x.NormalizedName)).HasName("ApiResourceNameIndex").IsUnique(true);
      }));
      modelBuilder.Entity<ExtendedIdentityResource>((Action<EntityTypeBuilder<ExtendedIdentityResource>>) (resource =>
      {
        resource.ToTable<ExtendedIdentityResource>("ExtendedIdentityResources");
        resource.HasKey((Expression<Func<ExtendedIdentityResource, object>>) (x => x.Id));
        resource.Property<string>((Expression<Func<ExtendedIdentityResource, string>>) (x => x.IdentityResourceName)).HasMaxLength(200).IsRequired(true);
        resource.Property<string>((Expression<Func<ExtendedIdentityResource, string>>) (x => x.NormalizedName)).HasMaxLength(200).IsRequired(true);
        resource.HasIndex((Expression<Func<ExtendedIdentityResource, object>>) (x => x.IdentityResourceName)).HasName("IdentityNameIndex").IsUnique(true);
        resource.HasIndex((Expression<Func<ExtendedIdentityResource, object>>) (x => x.NormalizedName)).HasName("IdentityResourceNameIndex").IsUnique(true);
      }));
      return modelBuilder;
    }
  }
}
