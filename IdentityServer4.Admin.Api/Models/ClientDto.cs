
using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Logic.Services;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class ClientDto
  {
    public string Id { get; set; }

    public bool Enabled { get; set; }

    public string ClientId { get; set; }

    public string Description { get; set; }

    public string ProtocolType { get; set; }

    [IgnorePropertyComparison]
    public ICollection<SecretDto> ClientSecrets { get; set; }

    public bool RequireClientSecret { get; set; }

    public string ClientName { get; set; }

    public string ClientUri { get; set; }

    public string LogoUri { get; set; }

    public bool RequireConsent { get; set; }

    public bool AllowRememberConsent { get; set; }

    public IList<string> AllowedGrantTypes { get; set; }

    public bool RequirePkce { get; set; }

    public bool AllowPlainTextPkce { get; set; }

    public bool AllowAccessTokensViaBrowser { get; set; }

    public IList<string> RedirectUris { get; set; }

    public IList<string> PostLogoutRedirectUris { get; set; }

    public string FrontChannelLogoutUri { get; set; }

    public bool FrontChannelLogoutSessionRequired { get; set; }

    public bool AllowOfflineAccess { get; set; }

    public IList<string> AllowedScopes { get; set; }

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

    public IList<string> IdentityProviderRestrictions { get; set; }

    public bool IncludeJwtId { get; set; }

    public ICollection<ClaimDto> Claims { get; set; }

    public bool AlwaysSendClientClaims { get; set; }

    public string ClientClaimsPrefix { get; set; }

    public IList<string> AllowedCorsOrigins { get; set; }

    public IList<PropertyDto> Properties { get; set; }

    public string PairWiseSubjectSalt { get; set; }

    public int? ConsentLifetime { get; set; }

    public bool BackChannelLogoutSessionRequired { get; set; }

    public string BackChannelLogoutUri { get; set; }

    public int? UserSSOLifetime { get; set; }

    public bool NonEditable { get; set; }

    public int? DeviceCodeLifetime { get; set; }

    public string UserCodeType { get; set; }
  }
}
