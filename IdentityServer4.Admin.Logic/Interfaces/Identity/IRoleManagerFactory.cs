





using IdentityExpress.Identity;

namespace IdentityServer4.Admin.Logic.Interfaces.Identity
{
  public interface IRoleManagerFactory
  {
    IRoleManager Create(IdentityExpressDbContext context);
  }
}
