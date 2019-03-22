using System;

namespace IdentityServer4.Admin.Api.Models
{
    public class CreateSecretDto
    {
        public CreateSecretDto()
        {
        }

        public CreateSecretDto(string type, string value, string description, DateTime? expiration)
        {
            this.Type = type;
            this.Value = value;
            this.Description = description;
            this.Expiration = expiration;
        }

        public string Type { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
