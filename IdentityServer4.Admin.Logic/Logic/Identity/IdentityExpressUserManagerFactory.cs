





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityExpressUserManagerFactory : IUserManagerFactory
  {
    private readonly IOptions<IdentityOptions> options;
    private readonly IPasswordHasher<IdentityExpressUser> passwordHasher;
    private readonly IEnumerable<IUserValidator<IdentityExpressUser>> userValidators;
    private readonly IEnumerable<IPasswordValidator<IdentityExpressUser>> passwordValidators;
    private readonly ILookupNormalizer keyNormalizer;
    private readonly IdentityErrorDescriber errors;
    private readonly IServiceProvider services;
    private readonly ILogger<IdentityExpressUserManager> logger;

    public IdentityExpressUserManagerFactory(IOptions<IdentityOptions> options, IPasswordHasher<IdentityExpressUser> passwordHasher, IEnumerable<IUserValidator<IdentityExpressUser>> userValidators, IEnumerable<IPasswordValidator<IdentityExpressUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<IdentityExpressUserManager> logger)
    {
      this.options = options;
      this.passwordHasher = passwordHasher;
      this.userValidators = userValidators;
      this.passwordValidators = passwordValidators;
      this.keyNormalizer = keyNormalizer;
      this.errors = errors;
      this.services = services;
      this.logger = logger;
    }

    public IUserManager Create(IdentityExpressDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      IdentityExpressUserStore expressUserStore = new IdentityExpressUserStore(context, (IdentityErrorDescriber) null);
      expressUserStore.IncludeDeletedUsers = true;
      expressUserStore.AutoSaveChanges = false;
      return (IUserManager) new IdentityExpressUserManager((IUserStore<IdentityExpressUser>) expressUserStore, this.options, this.passwordHasher, this.userValidators, this.passwordValidators, this.keyNormalizer, this.errors, this.services, (ILogger<UserManager<IdentityExpressUser>>) this.logger);
    }
  }
}
