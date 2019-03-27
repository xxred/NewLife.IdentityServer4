namespace IdentityServer4.Admin.Logic.Interfaces.Identity
{
  public interface IIdentityUnitOfWorkFactory
  {
    IIdentityUnitOfWork Create();
  }
}
