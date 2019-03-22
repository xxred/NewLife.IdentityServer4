using IdentityServer4.Admin.Logic.Logic.Services;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class ProtectedResourceDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        [IgnorePropertyComparison]
        public List<SecretDto> Secrets { get; set; }

        public List<ScopeDto> Scopes { get; set; }

        public List<string> AllowedClaims { get; set; }

        public DateTime Created { get; set; }

        public bool NonEditable { get; set; }
    }
}
