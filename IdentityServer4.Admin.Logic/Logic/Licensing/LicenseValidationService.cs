





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rsk.tools.licensevalidator.License;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Licensing
{
  public class LicenseValidationService : ILicenseValidationService
  {
    private readonly LicensingOptions licensingOptions;
    private readonly ILicenseHandler licenseHandler;
    private readonly IIdentityUnitOfWorkFactory uowFactory;
    private readonly IIdentityServerUnitOfWorkFactory idsUowFactory;
    private readonly IMemoryCache cache;
    private readonly ILogger<LicenseValidationService> logger;

    public LicenseValidationService(IOptions<LicensingOptions> optionsAccessor, ILicenseHandler licenseHandler, IIdentityUnitOfWorkFactory uowFactory, IIdentityServerUnitOfWorkFactory idsUowFactory, IMemoryCache cache, ILogger<LicenseValidationService> logger)
    {
      this.licensingOptions = optionsAccessor.Value;
      ILicenseHandler licenseHandler1 = licenseHandler;
      if (licenseHandler1 == null)
        throw new ArgumentNullException(nameof (licenseHandler));
      this.licenseHandler = licenseHandler1;
      IIdentityUnitOfWorkFactory unitOfWorkFactory1 = uowFactory;
      if (unitOfWorkFactory1 == null)
        throw new ArgumentNullException(nameof (uowFactory));
      this.uowFactory = unitOfWorkFactory1;
      IIdentityServerUnitOfWorkFactory unitOfWorkFactory2 = idsUowFactory;
      if (unitOfWorkFactory2 == null)
        throw new ArgumentNullException(nameof (idsUowFactory));
      this.idsUowFactory = unitOfWorkFactory2;
      IMemoryCache memoryCache = cache;
      if (memoryCache == null)
        throw new ArgumentNullException(nameof (cache));
      this.cache = memoryCache;
      ILogger<LicenseValidationService> logger1 = logger;
      if (logger1 == null)
        throw new ArgumentNullException(nameof (logger));
      this.logger = logger1;
    }

    public LicenseData GetLicenseData()
    {
      return this.licenseHandler.Validate(this.licensingOptions.LicenseKey, LicenseAudience.AdminUI);
    }

    public async Task<ValidateLicenseResult> IsValidLicense()
    {
      ValidateLicenseResult cachedLicense = IdentityServer4.Admin.Logic.Logic.Licensing.Caching<ValidateLicenseResult>.CacheGet(this.cache, CacheKeys.LicenseKeyEntry);
      if (cachedLicense != null)
        return cachedLicense;
      string licenseKey = this.licensingOptions.LicenseKey;
      LicenseData license;
      try
      {
        license = this.licenseHandler.Validate(licenseKey, LicenseAudience.AdminUI);
      }
      catch (LicenseExpiredException ex)
      {
        return ValidateLicenseResult.Invalid(" License has expired. Please contact sales@identityserver.com");
      }
      catch (InvalidAudienceLicenseException ex)
      {
        return ValidateLicenseResult.Invalid(" License is not for this product. Please contact sales@identityserver.com");
      }
      catch (InvalidLicenseException ex)
      {
        return ValidateLicenseResult.Invalid("Invalid License");
      }
      ValidateLicenseResult validateLicenseResult = await this.ValidateLicense(license);
      ValidateLicenseResult result = validateLicenseResult;
      validateLicenseResult = (ValidateLicenseResult) null;
      IdentityServer4.Admin.Logic.Logic.Licensing.Caching<ValidateLicenseResult>.CacheSet(this.cache, CacheKeys.LicenseKeyEntry, result);
      return result;
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
          return ValidateLicenseResult.Invalid(" Authority Url: " + this.licensingOptions.AuthorityURL + " is invalid for this license. Expected authority url is " + license.AuthorityURL + ".  Please contact sales@identityserver.com");
        licenseAuthUrl = (Uri) null;
        licenseAuthUrlWithoutScheme = (string) null;
        expectedAuthUrl = (Uri) null;
        expectedAuthUrlWithoutScheme = (string) null;
      }
      bool flag1 = await this.IsWithinClientQuota(license.MaxClients, false);
      if (!flag1)
        return ValidateLicenseResult.Invalid(string.Format(" Invalid number of clients for this license. Maximum number of clients allowed: {0}. Please contact sales@identityserver.com", (object) license.MaxClients));
      bool flag2 = await this.IsWithinUserQuota(false);
      return flag2 ? (this.IsWithinLicensePeriod(license.DateIssued, this.licensingOptions.BuildDate) ? ValidateLicenseResult.Valid() : ValidateLicenseResult.Invalid(" License invalid for this version of AdminUI. Please contact sales@identityserver.com")) : ValidateLicenseResult.Invalid(string.Format(" Invalid number of users for this license. Maximum number of users allowed: {0}. Please contact sales@identityserver.com", (object) license.MaxUsers));
    }

    public Task<bool> IsWithinUserQuota(bool isAddingUser)
    {
      return this.IsWithinUserQuota(this.licenseHandler.Validate(this.licensingOptions.LicenseKey, LicenseAudience.AdminUI).MaxUsers, isAddingUser);
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
      return this.IsWithinClientQuota(this.licenseHandler.Validate(this.licensingOptions.LicenseKey, LicenseAudience.AdminUI).MaxClients, isAddingClient);
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
      IdentityServer4.Admin.Logic.Logic.Licensing.Caching<ValidateLicenseResult>.CacheRemove(this.cache, CacheKeys.LicenseKeyEntry);
    }

    private bool IsWithinLicensePeriod(DateTime dateIssued, DateTime buildTime)
    {
      return dateIssued <= buildTime.AddYears(1);
    }
  }
}
