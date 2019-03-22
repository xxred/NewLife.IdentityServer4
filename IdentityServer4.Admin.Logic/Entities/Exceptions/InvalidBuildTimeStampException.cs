





using System;
using System.Runtime.Serialization;

namespace IdentityServer4.Admin.Logic.Entities.Exceptions
{
  public class InvalidBuildTimeStampException : Exception
  {
    public InvalidBuildTimeStampException()
    {
    }

    public InvalidBuildTimeStampException(string message)
      : base(message)
    {
    }

    public InvalidBuildTimeStampException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected InvalidBuildTimeStampException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
