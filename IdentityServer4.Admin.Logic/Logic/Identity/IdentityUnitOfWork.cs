





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Identity
{
  public class IdentityUnitOfWork : IIdentityUnitOfWork, IDisposable
  {
    private readonly IdentityErrorDescriber describer;
    private readonly IdentityExpressDbContext dbContext;
    private bool disposed;

    public IdentityUnitOfWork(IUserManagerFactory userManager, IRoleManagerFactory roleManager, DbContextOptions<IdentityExpressDbContext> dbContextOptions, ILookupNormalizer normalizer, IdentityErrorDescriber describer = null)
    {
      if (userManager == null)
        throw new ArgumentNullException(nameof (userManager));
      if (roleManager == null)
        throw new ArgumentNullException(nameof (roleManager));
      if (dbContextOptions == null)
        throw new ArgumentNullException(nameof (dbContextOptions));
      if (normalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.dbContext = new IdentityExpressDbContext(dbContextOptions);
      this.RoleManager = roleManager.Create(this.dbContext);
      this.UserManager = userManager.Create(this.dbContext);
      this.ClaimTypeRepository = (IRepository<IdentityExpressClaimType, string>) new IdentityExpressClaimTypeRepository(this.dbContext);
      this.describer = describer ?? new IdentityErrorDescriber();
    }

    public IUserManager UserManager { get; }

    public IRoleManager RoleManager { get; }

    public IRepository<IdentityExpressClaimType, string> ClaimTypeRepository { get; }

    public async Task<IdentityResult> Commit()
    {
      try
      {
        int num = await this.dbContext.SaveChangesAsync(new CancellationToken());
      }
      catch
      {
        return IdentityResult.Failed(this.describer.ConcurrencyFailure());
      }
      return IdentityResult.Success;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
        this.dbContext?.Dispose();
      this.disposed = true;
    }
  }
}
