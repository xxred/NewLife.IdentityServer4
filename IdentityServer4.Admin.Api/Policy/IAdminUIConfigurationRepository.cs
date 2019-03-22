using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
	public interface IAdminUIConfigurationRepository
	{
		Task<string> TryGetValue(string key, string defaultValue);

		Task SetValue(string key, string value);
	}
}
