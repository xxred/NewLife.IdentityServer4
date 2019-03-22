using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Licensing
{
  public interface ILicenseValidationService
  {
    Task<ValidateLicenseResult> IsValidLicense();

    Task<bool> IsWithinUserQuota(bool isAddingUser);

    Task<bool> IsWithinClientQuota(bool isAddingClient);

    void RemoveFromCache();
  }
}
