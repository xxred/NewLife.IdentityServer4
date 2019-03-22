using IdentityServer4.Admin.Logic.Logic.Services;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Models
{
    public class ScopeDto
    {
        [IgnorePropertyComparison]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; } = false;

        public bool Emphasize { get; set; } = false;

        public bool ShowInDiscoveryDocument { get; set; } = true;

        public List<string> UserClaims { get; set; }

        protected bool Equals(ScopeDto other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && string.Equals(DisplayName, other.DisplayName) && string.Equals(Description, other.Description) && Required == other.Required && Emphasize == other.Emphasize && ShowInDiscoveryDocument == other.ShowInDiscoveryDocument && Equals(UserClaims, other.UserClaims);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ScopeDto)obj);
        }

        public override int GetHashCode()
        {
            int num1 = ((((Id * 397 ^ (Name != null ? Name.GetHashCode() : 0)) * 397 ^ (DisplayName != null ? DisplayName.GetHashCode() : 0)) * 397 ^ (Description != null ? Description.GetHashCode() : 0)) * 397 ^ Required.GetHashCode()) * 397;
            bool flag = Emphasize;
            int hashCode1 = flag.GetHashCode();
            int num2 = (num1 ^ hashCode1) * 397;
            flag = ShowInDiscoveryDocument;
            int hashCode2 = flag.GetHashCode();
            return (num2 ^ hashCode2) * 397 ^ (UserClaims != null ? UserClaims.GetHashCode() : 0);
        }
    }
}
