





using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Licensing
{
  public class LicensingMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ILogger<LicensingMiddleware> middlewareLogger;

    public LicensingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
      this.next = next;
      this.middlewareLogger = loggerFactory.CreateLogger<LicensingMiddleware>();
    }

    public async Task Invoke(HttpContext context, ILicenseValidationService licenseValidation)
    {
      await this.GetLicense(context, licenseValidation);
    }

    private async Task GetLicense(HttpContext context, ILicenseValidationService licenseValidation)
    {
      ValidateLicenseResult validateLicenseResult = await licenseValidation.IsValidLicense();
      ValidateLicenseResult licenseResult = validateLicenseResult;
      validateLicenseResult = (ValidateLicenseResult) null;
      if (licenseResult.IsValid)
      {
        await this.next(context);
      }
      else
      {
        string errorLog = "Invalid License Key: " + licenseResult.ErrorMessage;
        this.middlewareLogger.LogWarning(errorLog, Array.Empty<object>());
        context.Response.StatusCode = 403;
        licenseValidation.RemoveFromCache();
        await context.Response.WriteAsync(errorLog, new CancellationToken());
        errorLog = (string) null;
      }
    }
  }
}
