using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Identity
{
  public interface IIdentityUnitOfWork : IDisposable
  {
    IUserManager UserManager { get; }

    IRoleManager RoleManager { get; }

    //IRepository<IdentityClaimType, string> ClaimTypeRepository { get; }

    Task<IdentityResult> Commit();
  }
}
