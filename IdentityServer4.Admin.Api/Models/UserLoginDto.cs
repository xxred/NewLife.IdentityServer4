using System;

namespace IdentityServer4.Admin.Api.Models
{
    public class UserLoginDto
    {
        public UserLoginDto()
        {

        }

        public UserLoginDto(string loginProvider, string providerKey, string displayName)
        {
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
            this.ProviderDisplayName = displayName;
        }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string ProviderDisplayName { get; set; }

        public string FriendlyProviderDisplayName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
