





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityExpressRoleManager : RoleManager<IdentityExpressRole>, IRoleManager, IDisposable
  {
    public IdentityExpressRoleManager(IRoleStore<IdentityExpressRole> store, IEnumerable<IRoleValidator<IdentityExpressRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<IdentityExpressRoleManager> logger)
      : base(store, roleValidators, keyNormalizer, errors, (ILogger<RoleManager<IdentityExpressRole>>) logger)
    {
      if (store == null)
        throw new ArgumentNullException(nameof (store));
    }
  }
}
