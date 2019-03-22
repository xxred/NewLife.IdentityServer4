





using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class ReflectionObjectPropertyComparator : IObjectPropertyComparator
  {
    public IEnumerable<PropertyDifference> Diff<T>(T old, T nextValue)
    {
      
      
      ReflectionObjectPropertyComparator.\u003C\u003Ec__DisplayClass0_0<T> cDisplayClass00 = new ReflectionObjectPropertyComparator.\u003C\u003Ec__DisplayClass0_0<T>();
      
      cDisplayClass00.\u003C\u003E4__this = this;
      
      cDisplayClass00.old = old;
      
      cDisplayClass00.nextValue = nextValue;
      
      if ((object) cDisplayClass00.old == null)
        throw new ArgumentNullException(nameof (old));
      
      if ((object) cDisplayClass00.nextValue == null)
        throw new ArgumentNullException(nameof (nextValue));
      
      return cDisplayClass00.\u003CDiff\u003Eg__Iterate\u007C0();
    }

    public IEnumerable<PropertyValue> GetPropertyValues<T>(T source)
    {
      
      
      ReflectionObjectPropertyComparator.\u003C\u003Ec__DisplayClass1_0<T> cDisplayClass10 = new ReflectionObjectPropertyComparator.\u003C\u003Ec__DisplayClass1_0<T>();
      
      cDisplayClass10.source = source;
      
      if ((object) cDisplayClass10.source == null)
        throw new ArgumentNullException(nameof (source));
      
      return cDisplayClass10.\u003CGetPropertyValues\u003Eg__Iterate\u007C0();
    }
  }
}
