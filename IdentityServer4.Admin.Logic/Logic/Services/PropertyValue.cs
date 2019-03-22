





using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class PropertyValue
  {
    public PropertyValue(string propertyName, object value)
      : this(propertyName, value, false)
    {
    }

    public PropertyValue(string propertyName, object value, bool isCollection)
    {
      this.PropertyName = propertyName;
      this.Value = value;
      this.IsCollection = isCollection;
    }

    public string PropertyName { get; }

    public object Value { get; }

    public bool IsCollection { get; }

    public override bool Equals(object obj)
    {
      PropertyValue propertyValue;
      if ((propertyValue = obj as PropertyValue) == null)
        return false;
      if (this.Value == null && propertyValue.Value == null)
        return true;
      if (this.Value == null)
        return false;
      if (!this.IsCollection)
        return this.Value.Equals(propertyValue.Value);
      IEnumerable source = propertyValue.Value as IEnumerable;
      return this.CompareCollection(source != null ? source.Cast<object>() : (IEnumerable<object>) null);
    }

    private bool CompareCollection(IEnumerable<object> rhs)
    {
      IEnumerable source1 = this.Value as IEnumerable;
      IEnumerable<object> source2 = source1 != null ? source1.Cast<object>() : (IEnumerable<object>) null;
      if ((source2 != null ? source2.Count<object>() : 0) != (rhs != null ? rhs.Count<object>() : 0))
        return false;
      if (source2 == null && rhs == null)
        return true;
      if (source2 == null || rhs == null)
        return false;
      HashSet<object> objectSet = new HashSet<object>(rhs);
      foreach (object obj in source2)
      {
        if (!objectSet.Contains(obj))
          return false;
      }
      return true;
    }
  }
}
