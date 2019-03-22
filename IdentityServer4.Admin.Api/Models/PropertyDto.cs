namespace IdentityServer4.Admin.Api.Models
{
    public class PropertyDto
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return this.Key + " = " + this.Value;
        }

        protected bool Equals(PropertyDto other)
        {
            return string.Equals(this.Key, other.Key) && string.Equals(this.Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((PropertyDto)obj);
        }

        public override int GetHashCode()
        {
            return (this.Key != null ? this.Key.GetHashCode() : 0) * 397 ^ (this.Value != null ? this.Value.GetHashCode() : 0);
        }
    }
}
