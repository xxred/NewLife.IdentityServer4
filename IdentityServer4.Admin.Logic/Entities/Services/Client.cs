





using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class Client
  {
    public Client()
    {
      IdentityServer4.Models.Client client = new IdentityServer4.Models.Client();
      this.Enabled = client.Enabled;
      this.ClientId = client.ClientId;
      this.ProtocolType = client.ProtocolType;
      this.ClientSecrets = (ICollection<Secret>) client.ClientSecrets.Select<IdentityServer4.Models.Secret, Secret>((Func<IdentityServer4.Models.Secret, Secret>) (x => new Secret()
      {
        Type = x.Type,
        Value = x.Value,
        Description = x.Description,
        Expiration = x.Expiration
      })).ToList<Secret>();
      this.RequireClientSecret = client.RequireClientSecret;
      this.ClientName = client.ClientName;
      this.ClientName = client.ClientName;
      this.LogoUri = client.LogoUri;
      this.RequireConsent = client.RequireConsent;
      this.AllowRememberConsent = client.AllowRememberConsent;
      this.AllowedGrantTypes = (ICollection<string>) client.AllowedGrantTypes.ToList<string>();
      this.RequirePkce = client.RequirePkce;
      this.AllowPlainTextPkce = client.AllowPlainTextPkce;
      this.AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
      this.RedirectUris = (ICollection<string>) client.RedirectUris.ToList<string>();
      this.PostLogoutRedirectUris = (ICollection<string>) client.PostLogoutRedirectUris.ToList<string>();
      this.FrontChannelLogoutUri = client.FrontChannelLogoutUri;
      this.FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired;
      this.AllowOfflineAccess = client.AllowOfflineAccess;
      this.AllowedScopes = (ICollection<string>) client.AllowedScopes.ToList<string>();
      this.AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken;
      this.IdentityTokenLifetime = client.IdentityTokenLifetime;
      this.AccessTokenLifetime = client.AccessTokenLifetime;
      this.AuthorizationCodeLifetime = client.AuthorizationCodeLifetime;
      this.AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
      this.SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
      this.RefreshTokenUsage = client.RefreshTokenUsage;
      this.UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh;
      this.RefreshTokenExpiration = client.RefreshTokenExpiration;
      this.AccessTokenType = client.AccessTokenType;
      this.EnableLocalLogin = client.EnableLocalLogin;
      this.IdentityProviderRestrictions = (ICollection<string>) client.IdentityProviderRestrictions.ToList<string>();
      this.IncludeJwtId = client.IncludeJwtId;
      this.Claims = (ICollection<Claim>) client.Claims.ToList<Claim>();
      this.AlwaysSendClientClaims = client.AlwaysSendClientClaims;
      this.AllowedCorsOrigins = (ICollection<string>) client.AllowedCorsOrigins.ToList<string>();
      this.BackChannelLogoutUri = client.BackChannelLogoutUri;
      this.BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired;
      this.ConsentLifetime = client.ConsentLifetime;
      this.PairWiseSubjectSalt = client.PairWiseSubjectSalt;
      this.Properties = client.Properties;
      this.DeviceCodeLifetime = client.DeviceCodeLifetime;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public bool Enabled { get; set; }

    public string ClientId { get; set; }

    public string ProtocolType { get; set; }

    public ICollection<Secret> ClientSecrets { get; set; }

    public bool RequireClientSecret { get; set; }

    public string ClientName { get; set; }

    public string ClientUri { get; set; }

    public string LogoUri { get; set; }

    public bool RequireConsent { get; set; }

    public bool AllowRememberConsent { get; set; }

    public ICollection<string> AllowedGrantTypes { get; set; }

    public bool RequirePkce { get; set; }

    public bool AllowPlainTextPkce { get; set; }

    public bool AllowAccessTokensViaBrowser { get; set; }

    public ICollection<string> RedirectUris { get; set; }

    public ICollection<string> PostLogoutRedirectUris { get; set; }

    public string FrontChannelLogoutUri { get; set; }

    public bool FrontChannelLogoutSessionRequired { get; set; }

    public bool AllowOfflineAccess { get; set; }

    public ICollection<string> AllowedScopes { get; set; }

    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    public int IdentityTokenLifetime { get; set; }

    public int AccessTokenLifetime { get; set; }

    public int AuthorizationCodeLifetime { get; set; }

    public int AbsoluteRefreshTokenLifetime { get; set; }

    public int SlidingRefreshTokenLifetime { get; set; }

    public TokenUsage RefreshTokenUsage { get; set; }

    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    public TokenExpiration RefreshTokenExpiration { get; set; }

    public AccessTokenType AccessTokenType { get; set; }

    public bool EnableLocalLogin { get; set; }

    public ICollection<string> IdentityProviderRestrictions { get; set; }

    public bool IncludeJwtId { get; set; }

    public ICollection<Claim> Claims { get; set; }

    public bool AlwaysSendClientClaims { get; set; }

    public string ClientClaimsPrefix { get; set; }

    public ICollection<string> AllowedCorsOrigins { get; set; }

    public string Description { get; set; }

    public bool Reserved { get; set; }

    public IDictionary<string, string> Properties { get; set; }

    public string PairWiseSubjectSalt { get; set; }

    public int? ConsentLifetime { get; set; }

    public bool BackChannelLogoutSessionRequired { get; set; }

    public string BackChannelLogoutUri { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public DateTime LastAccessed { get; set; }

    public int? UserSsoLifetime { get; set; }

    public string UserCodeType { get; set; }

    public int DeviceCodeLifetime { get; set; }

    public bool NonEditable { get; set; }
  }
}
