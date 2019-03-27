using Extensions.Identity.Stores.XCode;
using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Admin.Logic.Logic.Mappers
{
    public static class ServiceMappers
  {
    public static Role ToService(this IdentityRole role)
    {
      if (role == null)
        return (Role) null;
      return new Role()
      {
        Id = role.Id.ToString(),
        Name = role.Name,
        Description = role.Remark
      };
    }

    public static User ToService(this IdentityUser user, bool includeCollections = true)
    {
      if (user == null)
        return (User) null;
      User user1 = new User()
      {
        Subject = user.Id.ToString(),
        Username = user.UserName,
        Email = user.Email,
        IsDeleted = user.Enable,
        Claims = new List<Claim>(),
        Roles = new List<Role>()
      };
      if (includeCollections)
      {
        foreach (IdentityUserRole role in (IEnumerable<IdentityUserRole>) user.Roles)
          user1.Roles.Add(role.Role.ToService());
        foreach (IdentityClaim claim in (IEnumerable<IdentityClaim>) user.Claims)
          user1.Claims.Add(claim.ToClaim());
      }
      bool flag;
      if (user.EmailConfirmed)
      {
        IList<Claim> claims = user1.Claims;
        string type = "email_verified";
        flag = user.EmailConfirmed;
        string lowerInvariant = flag.ToString().ToLowerInvariant();
        Claim claim = new Claim(type, lowerInvariant);
        claims.Add(claim);
      }
      if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        user1.Claims.Add(new Claim("phone_number", user.PhoneNumber));
      if (user.PhoneNumberConfirmed)
      {
        IList<Claim> claims = user1.Claims;
        string type = "phone_number_verified";
        flag = user.PhoneNumberConfirmed;
        string lowerInvariant = flag.ToString().ToLowerInvariant();
        Claim claim = new Claim(type, lowerInvariant);
        claims.Add(claim);
      }
      return user1;
    }

    public static ClaimType ToService(this IdentityClaimType claimType)
    {
      if (claimType == null)
        return (ClaimType) null;
      return new ClaimType()
      {
        Id = claimType.Id,
        Name = claimType.Name,
        Description = claimType.Description,
        Required = claimType.Required,
        Reserved = claimType.Reserved,
        ValueType = claimType.ValueType.ToString(),
        Rule = claimType.Rule,
        RuleValidationFailureDescription = claimType.RuleValidationFailureDescription,
        UserEditable = claimType.UserEditable
      };
    }

    public static IdentityServer4.Admin.Logic.Entities.Services.ApiResource ToService(this IdentityServer4.XCode.Entities.ApiResource resource, ExtendedApiResource extendedResource)
    {
      if (resource == null || extendedResource == null)
        return (IdentityServer4.Admin.Logic.Entities.Services.ApiResource) null;
      IdentityServer4.Admin.Logic.Entities.Services.ApiResource apiResource = new IdentityServer4.Admin.Logic.Entities.Services.ApiResource()
      {
        Id = extendedResource.Id,
        Name = resource.Name,
        DisplayName = resource.DisplayName,
        Description = resource.Description,
        Enabled = resource.Enabled,
        NonEditable = resource.NonEditable
      };
      apiResource.ApiSecrets = resource.Secrets != null ? (ICollection<Secret>) resource.Secrets.Select<ApiSecret, Secret>((Func<ApiSecret, Secret>) (x => new Secret()
      {
        Id = x.Id,
        Type = x.Type,
        Value = x.Value,
        Description = x.Description,
        Expiration = x.Expiration
      })).ToList<Secret>() : (ICollection<Secret>) new List<Secret>();
      apiResource.UserClaims = resource.UserClaims != null ? (ICollection<string>) resource.UserClaims.Select<ApiResourceClaim, string>((Func<ApiResourceClaim, string>) (x => x.Type)).ToList<string>() : (ICollection<string>) new List<string>();
      apiResource.Scopes = resource.Scopes != null ? (ICollection<Scope>) resource.Scopes.Select<ApiScope, Scope>((Func<ApiScope, Scope>) (x => new Scope()
      {
        Id = x.Id,
        Name = x.Name,
        DisplayName = x.DisplayName,
        Description = x.Description,
        Required = x.Required,
        Emphasize = x.Emphasize,
        ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
        UserClaims = x.UserClaims != null ? (ICollection<string>) x.UserClaims.Select<ApiScopeClaim, string>((Func<ApiScopeClaim, string>) (y => y.Type)).ToList<string>() : (ICollection<string>) new List<string>()
      })).ToList<Scope>() : (ICollection<Scope>) new List<Scope>();
      return apiResource;
    }

    public static IdentityServer4.Admin.Logic.Entities.Services.IdentityResource ToService(this IdentityServer4.EntityFramework.Entities.IdentityResource resource, ExtendedIdentityResource extendedResource)
    {
      if (resource == null || extendedResource == null)
        return (IdentityServer4.Admin.Logic.Entities.Services.IdentityResource) null;
      IdentityServer4.Admin.Logic.Entities.Services.IdentityResource identityResource = new IdentityServer4.Admin.Logic.Entities.Services.IdentityResource();
      identityResource.Id = extendedResource.Id;
      identityResource.Name = resource.Name;
      identityResource.DisplayName = resource.DisplayName;
      identityResource.Description = resource.Description;
      identityResource.Enabled = resource.Enabled;
      identityResource.Emphasize = resource.Emphasize;
      identityResource.Required = resource.Required;
      identityResource.ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument;
      identityResource.UserClaims = resource.UserClaims != null ? (ICollection<string>) resource.UserClaims.Select<IdentityClaim, string>((Func<IdentityClaim, string>) (y => y.Type)).ToList<string>() : (ICollection<string>) new List<string>();
      identityResource.NonEditable = resource.NonEditable;
      return identityResource;
    }

    public static IdentityServer4.Admin.Logic.Entities.Services.Client ToService(this IdentityServer4.EntityFramework.Entities.Client client, ExtendedClient extendedClient)
    {
      if (client == null || extendedClient == null)
        return (IdentityServer4.Admin.Logic.Entities.Services.Client) null;
      IdentityServer4.Admin.Logic.Entities.Services.Client client1 = new IdentityServer4.Admin.Logic.Entities.Services.Client()
      {
        Id = extendedClient.Id,
        ClientId = client.ClientId,
        ClientName = client.ClientName,
        Description = extendedClient.Description,
        Enabled = client.Enabled,
        AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
        AccessTokenLifetime = client.AccessTokenLifetime,
        AccessTokenType = (AccessTokenType) client.AccessTokenType,
        AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
        AllowOfflineAccess = client.AllowOfflineAccess,
        AllowPlainTextPkce = client.AllowPlainTextPkce,
        AllowRememberConsent = client.AllowRememberConsent,
        AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
        AlwaysSendClientClaims = client.AlwaysSendClientClaims,
        AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
        ClientClaimsPrefix = client.ClientClaimsPrefix,
        ClientUri = client.ClientUri,
        EnableLocalLogin = client.EnableLocalLogin,
        IdentityTokenLifetime = client.IdentityTokenLifetime,
        IncludeJwtId = client.IncludeJwtId,
        LogoUri = client.LogoUri,
        FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired,
        FrontChannelLogoutUri = client.FrontChannelLogoutUri,
        ProtocolType = client.ProtocolType,
        RefreshTokenExpiration = (TokenExpiration) client.RefreshTokenExpiration,
        RefreshTokenUsage = (TokenUsage) client.RefreshTokenUsage,
        RequireClientSecret = client.RequireClientSecret,
        RequireConsent = client.RequireConsent,
        RequirePkce = client.RequirePkce,
        SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime,
        UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh,
        BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired,
        BackChannelLogoutUri = client.BackChannelLogoutUri,
        ConsentLifetime = client.ConsentLifetime,
        PairWiseSubjectSalt = client.PairWiseSubjectSalt,
        UserSsoLifetime = client.UserSsoLifetime,
        DeviceCodeLifetime = client.DeviceCodeLifetime,
        UserCodeType = client.UserCodeType,
        NonEditable = client.NonEditable
      };
      client1.AllowedCorsOrigins = client.AllowedCorsOrigins != null ? (ICollection<string>) client.AllowedCorsOrigins.Select<ClientCorsOrigin, string>((Func<ClientCorsOrigin, string>) (x => x.Origin)).ToList<string>() : (ICollection<string>) new List<string>();
      client1.AllowedGrantTypes = client.AllowedGrantTypes != null ? (ICollection<string>) client.AllowedGrantTypes.Select<ClientGrantType, string>((Func<ClientGrantType, string>) (x => x.GrantType)).ToList<string>() : (ICollection<string>) new List<string>();
      client1.AllowedScopes = client.AllowedScopes != null ? (ICollection<string>) client.AllowedScopes.Select<ClientScope, string>((Func<ClientScope, string>) (x => x.Scope)).ToList<string>() : (ICollection<string>) new List<string>();
      client1.Claims = client.Claims != null ? (ICollection<Claim>) client.Claims.Select<ClientClaim, Claim>((Func<ClientClaim, Claim>) (x => new Claim(x.Type, x.Value))).ToList<Claim>() : (ICollection<Claim>) new List<Claim>();
      client1.ClientSecrets = client.ClientSecrets != null ? (ICollection<Secret>) client.ClientSecrets.Select<ClientSecret, Secret>((Func<ClientSecret, Secret>) (x => new Secret()
      {
        Id = x.Id,
        Type = x.Type,
        Value = x.Value,
        Description = x.Description,
        Expiration = x.Expiration
      })).ToList<Secret>() : (ICollection<Secret>) new List<Secret>();
      client1.IdentityProviderRestrictions = client.IdentityProviderRestrictions != null ? (ICollection<string>) client.IdentityProviderRestrictions.Select<ClientIdPRestriction, string>((Func<ClientIdPRestriction, string>) (x => x.Provider)).ToList<string>() : (ICollection<string>) new List<string>();
      client1.PostLogoutRedirectUris = client.PostLogoutRedirectUris != null ? (ICollection<string>) client.PostLogoutRedirectUris.Select<ClientPostLogoutRedirectUri, string>((Func<ClientPostLogoutRedirectUri, string>) (x => x.PostLogoutRedirectUri)).ToList<string>() : (ICollection<string>) new List<string>();
      client1.RedirectUris = client.RedirectUris != null ? (ICollection<string>) client.RedirectUris.Select<ClientRedirectUri, string>((Func<ClientRedirectUri, string>) (x => x.RedirectUri)).ToList<string>() : (ICollection<string>) new List<string>();
      client1.Properties = client.Properties != null ? (IDictionary<string, string>) client.Properties.ToDictionary<ClientProperty, string, string>((Func<ClientProperty, string>) (x => x.Key), (Func<ClientProperty, string>) (x => x.Value)) : (IDictionary<string, string>) new Dictionary<string, string>();
      return client1;
    }

    public static ClaimDto ToService(this IdentityExpressClaim identityExpressClaim)
    {
      return new ClaimDto(identityExpressClaim.ClaimType, identityExpressClaim.ClaimValue);
    }
  }
}
