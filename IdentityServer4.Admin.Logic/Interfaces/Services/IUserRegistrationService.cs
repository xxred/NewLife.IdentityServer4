





using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
  public interface IUserRegistrationService
  {
    Task<IdentityResult> RegistrationConfirmation(IdentityExpressUser user);
  }
}
