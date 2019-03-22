using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IdentityServer4.Admin.Logic.Entities.Configuration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Permission
    {
        None,
        UserManager,
        IdentityServerManager,
        Auditer,
        All,
    }
}
