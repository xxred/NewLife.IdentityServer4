





using IdentityServer4.Admin.Logic.Entities.Services;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Entities.Services
{
    public class UserClaim
  {
    public string Subject { get; set; }

    public List<ClaimDto> Claims { get; set; }
  }
}
