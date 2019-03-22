





using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public interface IObjectPropertyComparator
  {
    IEnumerable<PropertyDifference> Diff<T>(T previous, T next);

    IEnumerable<PropertyValue> GetPropertyValues<T>(T source);
  }
}
