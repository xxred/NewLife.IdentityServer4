





using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Validators
{
  public interface IValidator<in T>
  {
    Task<IdentityResult> Validate(T input);
  }
}
