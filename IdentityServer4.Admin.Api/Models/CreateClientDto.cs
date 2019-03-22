using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Logic.Services;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class CreateClientDto
    {
        public GenericClientType ClientType { get; set; }

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }

        public string Description { get; set; }

        public bool RequireConsent { get; set; }

        public IList<string> RedirectUris { get; set; } = (IList<string>)new List<string>();

        public IList<string> PostLogoutRedirectUris { get; set; } = (IList<string>)new List<string>();

        public IList<string> AllowedScopes { get; set; } = (IList<string>)new List<string>();

        public IList<string> AllowedCorsOrigins { get; set; } = (IList<string>)new List<string>();

        [IgnorePropertyComparison]
        public IList<CreateSecretDto> ClientSecrets { get; set; } = (IList<CreateSecretDto>)new List<CreateSecretDto>();
    }
}
