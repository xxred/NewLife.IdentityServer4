using IdentityServer4.Admin.Logic.Entities.Configuration;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public interface IProvideClaimsToPermissionsPolicy
	{
		Task<AccessPolicyDefinition> GetAccessPolicyDefinition();
	}
}
