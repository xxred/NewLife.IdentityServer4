





using IdentityServer4.Admin.Logic.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class ApiResource
  {
    public ApiResource()
    {
      IdentityServer4.Models.ApiResource apiResource = new IdentityServer4.Models.ApiResource();
      this.Name = apiResource.Name;
      this.DisplayName = apiResource.DisplayName;
      this.Enabled = apiResource.Enabled;
      this.Description = apiResource.Description;
      this.UserClaims = apiResource.UserClaims;
      this.Scopes = (ICollection<Scope>) apiResource.Scopes.Select<IdentityServer4.Models.Scope, Scope>((Func<IdentityServer4.Models.Scope, Scope>) (x => new Scope()
      {
        Name = x.Name,
        DisplayName = x.DisplayName,
        Description = x.Description,
        Required = x.Required,
        Emphasize = x.Emphasize,
        ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
        UserClaims = x.UserClaims ?? (ICollection<string>) new List<string>()
      })).ToList<Scope>();
      this.ApiSecrets = (ICollection<Secret>) apiResource.ApiSecrets.Select<IdentityServer4.Models.Secret, Secret>((Func<IdentityServer4.Models.Secret, Secret>) (x => new Secret()
      {
        Type = x.Type,
        Value = x.Value,
        Description = x.Description,
        Expiration = x.Expiration
      })).ToList<Secret>();
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public bool Enabled { get; set; }

    public string Description { get; set; }

    public bool NonEditable { get; set; }

    public ICollection<string> UserClaims { get; set; }

    public ICollection<Scope> Scopes { get; set; }

    public ICollection<Secret> ApiSecrets { get; set; }

    public DateTime Created { get; set; }
  }
}
