





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityUnitOfWorkFactory : IIdentityUnitOfWorkFactory
  {
    private readonly IUserManagerFactory userManagerFactory;
    private readonly IRoleManagerFactory roleManagerFactory;
    private readonly DbContextOptions<IdentityExpressDbContext> dbContextOptions;
    private readonly ILookupNormalizer normalizer;

    public IdentityUnitOfWorkFactory(IUserManagerFactory userManagerFactory, IRoleManagerFactory roleManagerFactory, DbContextOptions<IdentityExpressDbContext> dbContextOptions, ILookupNormalizer normalizer)
    {
      IUserManagerFactory userManagerFactory1 = userManagerFactory;
      if (userManagerFactory1 == null)
        throw new ArgumentNullException(nameof (userManagerFactory));
      this.userManagerFactory = userManagerFactory1;
      IRoleManagerFactory roleManagerFactory1 = roleManagerFactory;
      if (roleManagerFactory1 == null)
        throw new ArgumentNullException(nameof (roleManagerFactory));
      this.roleManagerFactory = roleManagerFactory1;
      DbContextOptions<IdentityExpressDbContext> dbContextOptions1 = dbContextOptions;
      if (dbContextOptions1 == null)
        throw new ArgumentNullException(nameof (dbContextOptions));
      this.dbContextOptions = dbContextOptions1;
      ILookupNormalizer lookupNormalizer = normalizer;
      if (lookupNormalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.normalizer = lookupNormalizer;
    }

    public IIdentityUnitOfWork Create()
    {
      return (IIdentityUnitOfWork) new IdentityUnitOfWork(this.userManagerFactory, this.roleManagerFactory, this.dbContextOptions, this.normalizer, (IdentityErrorDescriber) null);
    }
  }
}
