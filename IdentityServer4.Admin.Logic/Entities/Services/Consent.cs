using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class Consent
    {
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientUrl { get; set; }

        public string ClientLogoUrl { get; set; }

        public string ClientDescription { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Expires { get; set; }

        public List<string> Scopes { get; set; }
    }
}
