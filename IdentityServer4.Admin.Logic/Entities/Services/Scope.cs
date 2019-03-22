using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class Scope
    {
        public Scope()
        {
            IdentityServer4.Models.Scope scope = new IdentityServer4.Models.Scope();
            Name = scope.Name;
            DisplayName = scope.DisplayName;
            Description = scope.Description;
            Required = scope.Required;
            Emphasize = scope.Emphasize;
            ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument;
            UserClaims = scope.UserClaims ?? new List<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public ICollection<string> UserClaims { get; set; }
    }
}
