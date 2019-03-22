using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class IdentityResourceDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> AllowedClaims { get; set; }

        public bool NonEditable { get; set; }
    }
}
