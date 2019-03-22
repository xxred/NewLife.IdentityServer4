





using System;

namespace IdentityServer4.Admin.Logic.Entities.Exceptions
{
  public class ValidationException : Exception
  {
    public ValidationException()
    {
    }

    public ValidationException(string message)
      : base(message)
    {
    }
  }
}
