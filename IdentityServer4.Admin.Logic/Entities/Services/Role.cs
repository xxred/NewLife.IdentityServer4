using System;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class Role
    {
        public Role()
        {
        }

        public Role(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name;
        }

        public Role(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(description));
            Name = name;
            Description = description;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Reserved { get; set; }
    }
}
