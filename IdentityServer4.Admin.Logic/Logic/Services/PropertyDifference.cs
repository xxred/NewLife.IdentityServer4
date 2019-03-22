





namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class PropertyDifference
  {
    public PropertyDifference(string propertyName, object previousValue, object newValue)
    {
      this.PropertyName = propertyName;
      this.Previous = previousValue;
      this.New = newValue;
    }

    public string PropertyName { get; private set; }

    public object Previous { get; private set; }

    public object New { get; private set; }
  }
}
