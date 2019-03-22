
using System.Collections.Generic;

namespace IdentityServer4.Admin.Api.Mappers
{
  public class AuditQueryMapper
  {
    private List<string> errors = new List<string>();

    public bool HasErrors
    {
      get
      {
        return this.errors.Count > 0;
      }
    }
  }
}
