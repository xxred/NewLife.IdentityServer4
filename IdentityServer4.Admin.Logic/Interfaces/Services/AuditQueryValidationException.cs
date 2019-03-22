





using System;
using System.Runtime.Serialization;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
  [Serializable]
  public class AuditQueryValidationException : Exception
  {
    public AuditQueryValidationException()
    {
    }

    public AuditQueryValidationException(string message)
      : base(message)
    {
    }

    public AuditQueryValidationException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected AuditQueryValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
