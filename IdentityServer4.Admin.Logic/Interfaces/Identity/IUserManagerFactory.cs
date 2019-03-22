





using IdentityExpress.Identity;

namespace IdentityServer4.Admin.Logic.Interfaces.Identity
{
  public interface IUserManagerFactory
  {
    IUserManager Create(IdentityExpressDbContext context);
  }
}
