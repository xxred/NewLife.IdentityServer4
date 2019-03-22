





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityExpressRoleManagerFactory : IRoleManagerFactory
  {
    private readonly IEnumerable<IRoleValidator<IdentityExpressRole>> roleValidators;
    private readonly ILookupNormalizer lookupNormalizer;
    private readonly IdentityErrorDescriber describer;
    private readonly ILogger<IdentityExpressRoleManager> logger;

    public IdentityExpressRoleManagerFactory(IEnumerable<IRoleValidator<IdentityExpressRole>> roleValidators, ILookupNormalizer lookupNormalizer, IdentityErrorDescriber describer, ILogger<IdentityExpressRoleManager> logger)
    {
      this.roleValidators = roleValidators;
      this.lookupNormalizer = lookupNormalizer;
      this.describer = describer;
      this.logger = logger;
    }

    public IRoleManager Create(IdentityExpressDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      IdentityExpressRoleStore expressRoleStore = new IdentityExpressRoleStore(context, (IdentityErrorDescriber) null);
      expressRoleStore.AutoSaveChanges = false;
      return (IRoleManager) new IdentityExpressRoleManager((IRoleStore<IdentityExpressRole>) expressRoleStore, this.roleValidators, this.lookupNormalizer, this.describer, this.logger);
    }
  }
}
