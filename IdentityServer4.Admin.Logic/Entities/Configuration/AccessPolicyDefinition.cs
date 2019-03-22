using IdentityServer4.Admin.Logic.Entities.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Entities.Configuration
{
    public class AccessPolicyDefinition
    {
        public List<PolicyClaim> PolicyClaims { get; set; }

        public string Version { get; set; }

        protected bool Equals(AccessPolicyDefinition other)
        {
            return PolicyClaims.SequenceEqual(other.PolicyClaims) && string.Equals(Version, other.Version);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((AccessPolicyDefinition)obj);
        }

        public override int GetHashCode()
        {
            return (PolicyClaims != null ? PolicyClaims.GetHashCode() : 0) * 397 ^ (Version != null ? Version.GetHashCode() : 0);
        }
    }
}
