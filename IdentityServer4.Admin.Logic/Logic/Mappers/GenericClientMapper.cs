





using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Mappers;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Logic.Mappers
{
    public class GenericClientMapper : IMapper<GenericClient, IdentityServer4.Admin.Logic.Entities.Services.Client>
  {
    public IdentityServer4.Admin.Logic.Entities.Services.Client Map(GenericClient input)
    {
      if (input == null)
        return (IdentityServer4.Admin.Logic.Entities.Services.Client) null;
      IdentityServer4.Admin.Logic.Entities.Services.Client client = new IdentityServer4.Admin.Logic.Entities.Services.Client()
      {
        ClientId = input.ClientId,
        ClientName = input.ClientName,
        ClientUri = input.ClientUri,
        LogoUri = input.LogoUri,
        Description = input.Description,
        ProtocolType = "oidc",
        RequireConsent = input.RequireConsent
      };
      switch (input.ClientType)
      {
        case GenericClientType.ClientSide:
          client.AllowedGrantTypes = (ICollection<string>) GrantTypes.Implicit.ToList<string>();
          client.AllowAccessTokensViaBrowser = true;
          client.AllowOfflineAccess = false;
          client.RedirectUris = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.RedirectUris ?? (IList<string>) new List<string>()));
          client.PostLogoutRedirectUris = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.PostLogoutRedirectUris ?? (IList<string>) new List<string>()));
          client.AllowedScopes = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedScopes ?? (IList<string>) new List<string>()));
          client.AllowedCorsOrigins = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedCorsOrigins ?? (IList<string>) new List<string>()));
          break;
        case GenericClientType.ServerSide:
          client.AllowedGrantTypes = (ICollection<string>) GrantTypes.Hybrid.ToList<string>();
          client.ClientSecrets = (ICollection<Secret>) new HashSet<Secret>((IEnumerable<Secret>) input.Secrets);
          client.RedirectUris = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.RedirectUris ?? (IList<string>) new List<string>()));
          client.PostLogoutRedirectUris = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.PostLogoutRedirectUris ?? (IList<string>) new List<string>()));
          client.AllowedScopes = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedScopes ?? (IList<string>) new List<string>()));
          client.AllowedCorsOrigins = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedCorsOrigins ?? (IList<string>) new List<string>()));
          break;
        case GenericClientType.Machine:
          client.AllowedGrantTypes = (ICollection<string>) GrantTypes.ClientCredentials.ToList<string>();
          client.ClientSecrets = (ICollection<Secret>) new HashSet<Secret>((IEnumerable<Secret>) input.Secrets);
          client.AllowedScopes = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedScopes ?? (IList<string>) new List<string>()));
          break;
        case GenericClientType.Native:
          client.AllowedGrantTypes = (ICollection<string>) GrantTypes.Hybrid.ToList<string>();
          client.ClientSecrets = (ICollection<Secret>) new HashSet<Secret>((IEnumerable<Secret>) input.Secrets);
          client.RedirectUris = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.RedirectUris ?? (IList<string>) new List<string>()));
          client.AllowedScopes = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedScopes ?? (IList<string>) new List<string>()));
          break;
        case GenericClientType.Device:
          client.AllowedGrantTypes = (ICollection<string>) GrantTypes.DeviceFlow.ToList<string>();
          client.AllowedScopes = (ICollection<string>) new HashSet<string>((IEnumerable<string>) (input.AllowedScopes ?? (IList<string>) new List<string>()));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return client;
    }
  }
}
