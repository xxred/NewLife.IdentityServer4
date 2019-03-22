namespace IdentityServer4.Admin.Api.Models
{
    public class RoleDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Reserved { get; set; }

        public override string ToString()
        {
            return Name;
        }

        protected bool Equals(RoleDto other)
        {
            return string.Equals(Id, other.Id) && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) && Reserved == other.Reserved;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((RoleDto)obj);
        }

        public override int GetHashCode()
        {
            return (((Id != null ? Id.GetHashCode() : 0) * 397 ^ (Name != null ? Name.GetHashCode() : 0)) * 397 ^ (Description != null ? Description.GetHashCode() : 0)) * 397 ^ Reserved.GetHashCode();
        }
    }
}
