





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Admin.Logic.Logic.Mappers
{
    public static class DomainMappers
  {
    public static IdentityExpressClaimType ToDomain(this ClaimType claimType, ILookupNormalizer normalizer)
    {
      IdentityExpressClaimType expressClaimType = new IdentityExpressClaimType()
      {
        Id = claimType.Id,
        Name = claimType.Name,
        NormalizedName = normalizer.Normalize(claimType.Name),
        Description = claimType.Description,
        Required = claimType.Required,
        Reserved = claimType.Reserved,
        Rule = claimType.Rule,
        RuleValidationFailureDescription = claimType.RuleValidationFailureDescription,
        ValueType = IdentityExpressClaimValueType.String,
        UserEditable = claimType.UserEditable
      };
      if (!string.IsNullOrWhiteSpace(claimType.ValueType))
      {
        IdentityExpressClaimValueType result;
        Enum.TryParse<IdentityExpressClaimValueType>(claimType.ValueType, out result);
        expressClaimType.ValueType = result;
      }
      return expressClaimType;
    }

    public static IdentityServer4.EntityFramework.Entities.Client ToDomain(this IdentityServer4.Admin.Logic.Entities.Services.Client client)
    {
      if (client == null)
        return (IdentityServer4.EntityFramework.Entities.Client) null;
      return new IdentityServer4.EntityFramework.Entities.Client()
      {
        ClientId = client.ClientId,
        ClientName = client.ClientName,
        Enabled = client.Enabled,
        AllowedGrantTypes = client.AllowedGrantTypes.Select<string, ClientGrantType>((Func<string, ClientGrantType>) (x => new ClientGrantType()
        {
          GrantType = x
        })).ToList<ClientGrantType>(),
        AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
        AccessTokenLifetime = client.AccessTokenLifetime,
        AccessTokenType = (int) client.AccessTokenType,
        AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
        AllowOfflineAccess = client.AllowOfflineAccess,
        AllowPlainTextPkce = client.AllowPlainTextPkce,
        EnableLocalLogin = client.EnableLocalLogin,
        IdentityTokenLifetime = client.IdentityTokenLifetime,
        AllowedScopes = client.AllowedScopes.Select<string, ClientScope>((Func<string, ClientScope>) (x => new ClientScope()
        {
          Scope = x
        })).ToList<ClientScope>(),
        ClientSecrets = client.ClientSecrets.Select<Secret, ClientSecret>((Func<Secret, ClientSecret>) (x => x.ToClientSecret())).ToList<ClientSecret>(),
        AlwaysSendClientClaims = client.AlwaysSendClientClaims,
        AllowRememberConsent = client.AllowRememberConsent,
        AllowedCorsOrigins = client.AllowedCorsOrigins.Select<string, ClientCorsOrigin>((Func<string, ClientCorsOrigin>) (x => new ClientCorsOrigin()
        {
          Origin = x
        })).ToList<ClientCorsOrigin>(),
        AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
        AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
        Claims = client.Claims.Select<Claim, ClientClaim>((Func<Claim, ClientClaim>) (x => new ClientClaim()
        {
          Type = x.Type,
          Value = x.Value
        })).ToList<ClientClaim>(),
        ClientClaimsPrefix = client.ClientClaimsPrefix,
        ClientUri = client.ClientUri,
        FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired,
        FrontChannelLogoutUri = client.FrontChannelLogoutUri,
        IdentityProviderRestrictions = client.IdentityProviderRestrictions.Select<string, ClientIdPRestriction>((Func<string, ClientIdPRestriction>) (x => new ClientIdPRestriction()
        {
          Provider = x
        })).ToList<ClientIdPRestriction>(),
        IncludeJwtId = client.IncludeJwtId,
        LogoUri = client.LogoUri,
        PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select<string, ClientPostLogoutRedirectUri>((Func<string, ClientPostLogoutRedirectUri>) (x => new ClientPostLogoutRedirectUri()
        {
          PostLogoutRedirectUri = x
        })).ToList<ClientPostLogoutRedirectUri>(),
        ProtocolType = client.ProtocolType,
        RedirectUris = client.RedirectUris.Select<string, ClientRedirectUri>((Func<string, ClientRedirectUri>) (x => new ClientRedirectUri()
        {
          RedirectUri = x
        })).ToList<ClientRedirectUri>(),
        RefreshTokenExpiration = (int) client.RefreshTokenExpiration,
        RefreshTokenUsage = (int) client.RefreshTokenUsage,
        RequireClientSecret = client.RequireClientSecret,
        RequireConsent = client.RequireConsent,
        RequirePkce = client.RequirePkce,
        SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime,
        UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh,
        BackChannelLogoutUri = client.BackChannelLogoutUri,
        Description = client.Description,
        ConsentLifetime = client.ConsentLifetime,
        PairWiseSubjectSalt = client.PairWiseSubjectSalt,
        BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired,
        Properties = client.Properties.Select<KeyValuePair<string, string>, ClientProperty>((Func<KeyValuePair<string, string>, ClientProperty>) (x =>
        {
          return new ClientProperty()
          {
            Key = x.Key,
            Value = x.Value
          };
        })).ToList<ClientProperty>(),
        UserSsoLifetime = client.UserSsoLifetime,
        Updated = new DateTime?(client.Updated),
        LastAccessed = new DateTime?(client.LastAccessed),
        DeviceCodeLifetime = client.DeviceCodeLifetime,
        UserCodeType = client.UserCodeType,
        NonEditable = client.NonEditable
      };
    }

    public static IdentityServer4.EntityFramework.Entities.ApiResource ToDomain(this IdentityServer4.Admin.Logic.Entities.Services.ApiResource resource)
    {
      if (resource == null)
        return (IdentityServer4.EntityFramework.Entities.ApiResource) null;
      return new IdentityServer4.EntityFramework.Entities.ApiResource()
      {
        Name = resource.Name,
        DisplayName = resource.DisplayName,
        Description = resource.Description,
        Enabled = resource.Enabled,
        Scopes = resource.Scopes.Select<Scope, ApiScope>((Func<Scope, ApiScope>) (x => new ApiScope()
        {
          Id = x.Id,
          Name = x.Name,
          DisplayName = x.DisplayName,
          Description = x.Description,
          Emphasize = x.Emphasize,
          Required = x.Required,
          ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
          UserClaims = x.UserClaims.Select<string, ApiScopeClaim>((Func<string, ApiScopeClaim>) (y =>
          {
            return new ApiScopeClaim() { Type = y };
          })).ToList<ApiScopeClaim>()
        })).ToList<ApiScope>(),
        Secrets = resource.ApiSecrets.Select<Secret, ApiSecret>((Func<Secret, ApiSecret>) (x => x.ToApiSecret())).ToList<ApiSecret>(),
        UserClaims = resource.UserClaims.Select<string, ApiResourceClaim>((Func<string, ApiResourceClaim>) (x =>
        {
          return new ApiResourceClaim() { Type = x };
        })).ToList<ApiResourceClaim>(),
        NonEditable = resource.NonEditable
      };
    }

    public static ExtendedClient ToExtendedClient(this IdentityServer4.Admin.Logic.Entities.Services.Client client, ILookupNormalizer normalizer)
    {
      return new ExtendedClient()
      {
        Id = client.Id,
        ClientId = client.ClientId,
        NormalizedClientId = normalizer.Normalize(client.ClientId),
        NormalizedClientName = normalizer.Normalize(client.ClientName),
        Description = client.Description,
        Reserved = client.Reserved
      };
    }

    public static ExtendedApiResource ToExtendedApiResource(this IdentityServer4.Admin.Logic.Entities.Services.ApiResource resource, ILookupNormalizer normalizer)
    {
      return new ExtendedApiResource()
      {
        Id = resource.Id,
        ApiResourceName = resource.Name,
        NormalizedName = normalizer.Normalize(resource.Name),
        Reserved = resource.NonEditable
      };
    }

    public static ExtendedIdentityResource ToExtendedIdentityResource(this IdentityServer4.Admin.Logic.Entities.Services.IdentityResource resource, ILookupNormalizer normalizer)
    {
      return new ExtendedIdentityResource()
      {
        Id = resource.Id,
        IdentityResourceName = resource.Name,
        NormalizedName = normalizer.Normalize(resource.Name),
        Reserved = resource.NonEditable
      };
    }

    public static ApiSecret ToApiSecret(this Secret secret)
    {
      string type = secret.Type.Replace(" ", "");
      ApiSecret apiSecret = new ApiSecret();
      apiSecret.Id = secret.Id;
      apiSecret.Type = type;
      apiSecret.Value = secret.Value.HashSecret(type);
      apiSecret.Description = secret.Description;
      apiSecret.Expiration = secret.Expiration;
      return apiSecret;
    }

    public static ApiSecret ToApiSecret(this PlainTextSecret secret)
    {
      string type = secret.Type.Replace(" ", "");
      ApiSecret apiSecret = new ApiSecret();
      apiSecret.Id = secret.Id;
      apiSecret.Type = type;
      apiSecret.Value = secret.Value.HashSecret(type);
      apiSecret.Description = secret.Description;
      apiSecret.Expiration = secret.Expiration;
      return apiSecret;
    }

    public static ClientSecret ToClientSecret(this Secret secret)
    {
      string type = secret.Type.Replace(" ", "");
      ClientSecret clientSecret = new ClientSecret();
      clientSecret.Id = secret.Id;
      clientSecret.Type = type;
      clientSecret.Value = secret.Value.HashSecret(type);
      clientSecret.Description = secret.Description;
      clientSecret.Expiration = secret.Expiration;
      return clientSecret;
    }

    public static ClientSecret ToClientSecret(this PlainTextSecret secret)
    {
      string type = secret.Type.Replace(" ", "");
      ClientSecret clientSecret = new ClientSecret();
      clientSecret.Id = secret.Id;
      clientSecret.Type = type;
      clientSecret.Value = secret.Value.HashSecret(type);
      clientSecret.Description = secret.Description;
      clientSecret.Expiration = secret.Expiration;
      return clientSecret;
    }

    private static string HashSecret(this string value, string type)
    {
      if (string.Compare(type, "SharedSecret", StringComparison.CurrentCultureIgnoreCase) == 0)
        return value.Sha512();
      return value;
    }
  }
}
