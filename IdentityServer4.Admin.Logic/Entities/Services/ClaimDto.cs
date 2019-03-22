using System;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class ClaimDto
    {
        public ClaimDto()
        {
        }

        public ClaimDto(string type, string value)
        {
            string str = type;
            if (str == null)
                throw new ArgumentNullException(nameof(type));
            Type = str;
            Value = value;
        }

        public string Type { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Type + " = " + Value;
        }

        protected bool Equals(ClaimDto other)
        {
            return string.Equals(Type, other.Type) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ClaimDto)obj);
        }

        public override int GetHashCode()
        {
            return (Type != null ? Type.GetHashCode() : 0) * 397 ^ (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
