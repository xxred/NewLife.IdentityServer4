





using IdentityServer4.Admin.Logic.Entities.Configuration;

namespace IdentityServer4.Admin.Logic.Entities.Configuration
{
    public class PolicyClaim
  {
    public string Type { get; set; }

    public string Value { get; set; }

    public Permission Permission { get; set; }

    protected bool Equals(PolicyClaim other)
    {
      return string.Equals(this.Type, other.Type) && string.Equals(this.Value, other.Value) && this.Permission == other.Permission;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (obj.GetType() != this.GetType())
        return false;
      return this.Equals((PolicyClaim) obj);
    }

    public override int GetHashCode()
    {
      return (int) ((Permission) (((this.Type != null ? this.Type.GetHashCode() : 0) * 397 ^ (this.Value != null ? this.Value.GetHashCode() : 0)) * 397) ^ this.Permission);
    }

    public override string ToString()
    {
      return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", (object) "Type", (object) this.Type, (object) "Value", (object) this.Value, (object) "Permission", (object) this.Permission);
    }
  }
}
