





using IdentityServer4.Admin.Logic.Entities.Exceptions;
using System;
using System.Globalization;

namespace IdentityServer4.Admin.Logic.Logic.Licensing
{
  public class BuildTimeStampAttribute : Attribute
  {
    public DateTime Stamp;

    public BuildTimeStampAttribute(string timeStamp)
    {
      if (timeStamp == null)
        throw new ArgumentNullException(nameof (timeStamp));
      try
      {
        this.Stamp = DateTime.ParseExact(timeStamp, "dd/MM/yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch
      {
        throw new InvalidBuildTimeStampException();
      }
    }
  }
}
