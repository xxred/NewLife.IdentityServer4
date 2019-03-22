





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rsk.tools.licensevalidator.License;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Licensing.Community
{
  public class CommunityLicenseValidationService : ILicenseValidationService
  {
    private readonly LicensingOptions licensingOptions;
    private readonly IIdentityUnitOfWorkFactory uowFactory;
    private readonly IIdentityServerUnitOfWorkFactory idsUowFactory;
    private readonly ILogger<LicenseValidationService> logger;

    public CommunityLicenseValidationService(IOptions<LicensingOptions> optionsAccessor, IIdentityUnitOfWorkFactory uowFactory, IIdentityServerUnitOfWorkFactory idsUowFactory, ILogger<LicenseValidationService> logger)
    {
      this.licensingOptions = optionsAccessor.Value;
      IIdentityUnitOfWorkFactory unitOfWorkFactory1 = uowFactory;
      if (unitOfWorkFactory1 == null)
        throw new ArgumentNullException(nameof (uowFactory));
      this.uowFactory = unitOfWorkFactory1;
      IIdentityServerUnitOfWorkFactory unitOfWorkFactory2 = idsUowFactory;
      if (unitOfWorkFactory2 == null)
        throw new ArgumentNullException(nameof (idsUowFactory));
      this.idsUowFactory = unitOfWorkFactory2;
      ILogger<LicenseValidationService> logger1 = logger;
      if (logger1 == null)
        throw new ArgumentNullException(nameof (logger));
      this.logger = logger1;
    }

    public LicenseData GetLicenseData()
    {
      return new LicenseData()
      {
        AuthorityURL = "http://localhost:5003",
        ExpiryDate = new DateTime?(DateTime.MaxValue),
        LicenseType = new LicenseType?(LicenseType.Demo),
        MaxClients = new int?(2),
        MaxUsers = new int?(10),
        Audience = LicenseAudience.AdminUI
      };
    }

    public async Task<ValidateLicenseResult> IsValidLicense()
    {
      ValidateLicenseResult validateLicenseResult = await this.ValidateLicense(this.GetLicenseData());
      return validateLicenseResult;
    }

    private async Task<ValidateLicenseResult> ValidateLicense(LicenseData license)
    {
      this.logger.LogInformation("Validating License", Array.Empty<object>());
      if (license.AuthorityURL != null)
      {
        Uri licenseAuthUrl = new Uri(license.AuthorityURL);
        string licenseAuthUrlWithoutScheme = licenseAuthUrl.Host + licenseAuthUrl.PathAndQuery + licenseAuthUrl.Fragment;
        Uri expectedAuthUrl = new Uri(this.licensingOptions.AuthorityURL);
        string expectedAuthUrlWithoutScheme = expectedAuthUrl.Host + expectedAuthUrl.PathAndQuery + expectedAuthUrl.Fragment;
        if (licenseAuthUrlWithoutScheme != expectedAuthUrlWithoutScheme)
          return ValidateLicenseResult.Invalid(" Authority Url: " + this.licensingOptions.AuthorityURL + " is invalid for this license. Only the authority url is " + license.AuthorityURL + " is allowed for a Community Edition license.  Please contact sales@identityserver.com");
        licenseAuthUrl = (Uri) null;
        licenseAuthUrlWithoutScheme = (string) null;
        expectedAuthUrl = (Uri) null;
        expectedAuthUrlWithoutScheme = (string) null;
      }
      bool flag1 = await this.IsWithinClientQuota(license.MaxClients, false);
      if (!flag1)
        return ValidateLicenseResult.Invalid(string.Format(" Invalid number of clients for Coummunity Edition license. Maximum number of clients allowed: {0}. Please contact sales@identityserver.com", (object) license.MaxClients));
      bool flag2 = await this.IsWithinUserQuota(false);
      return flag2 ? ValidateLicenseResult.Valid() : ValidateLicenseResult.Invalid(string.Format(" Invalid number of users for Community Edition license. Maximum number of users allowed: {0}. Please contact sales@identityserver.com", (object) license.MaxUsers));
    }

    public Task<bool> IsWithinUserQuota(bool isAddingUser)
    {
      return this.IsWithinUserQuota(new int?(10), isAddingUser);
    }

    private Task<bool> IsWithinUserQuota(int? maxUsers, bool isAddingUser)
    {
      if (!maxUsers.HasValue)
        return Task.FromResult<bool>(true);
      int num = isAddingUser ? 1 : 0;
      using (IIdentityUnitOfWork identityUnitOfWork = this.uowFactory.Create())
        num += identityUnitOfWork.UserManager.Users.Count<IdentityExpressUser>();
      return Task.FromResult<bool>(num <= maxUsers.Value);
    }

    public Task<bool> IsWithinClientQuota(bool isAddingClient)
    {
      return this.IsWithinClientQuota(new int?(2), isAddingClient);
    }

    private async Task<bool> IsWithinClientQuota(int? maxClients, bool isAddingClient)
    {
      if (!maxClients.HasValue)
        return true;
      int clientCount = isAddingClient ? 1 : 0;
      using (IIdentityServerUnitOfWork uow = this.idsUowFactory.Create())
      {
        int num1 = clientCount;
        int num2 = await uow.ClientRepository.Count();
        clientCount = num1 + num2;
      }
      int num = clientCount;
      int? nullable = maxClients;
      int valueOrDefault = nullable.GetValueOrDefault();
      return num <= valueOrDefault & nullable.HasValue;
    }

    public void RemoveFromCache()
    {
    }

    private bool IsWithinLicensePeriod(DateTime dateIssued, DateTime buildTime)
    {
      return true;
    }
  }
}
