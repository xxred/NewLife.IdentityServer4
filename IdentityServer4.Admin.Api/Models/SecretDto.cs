using System;

namespace IdentityServer4.Admin.Api.Models
{
    public class SecretDto
    {
        public SecretDto()
        {
        }

        public SecretDto(int id, string type, string value, string description, DateTime? expiration)
        {
            Id = id;
            Type = type;
            Value = value;
            Description = description;
            Expiration = expiration;
        }

        public int Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
