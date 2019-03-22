





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class UserRegistrationService : IUserRegistrationService
  {
    private readonly WebhookOptions options;
    private readonly IWebhookService webhookTokenService;

    public UserRegistrationService(IOptions<WebhookOptions> options, IWebhookService webhookTokenService)
    {
      this.options = options.Value;
      this.webhookTokenService = webhookTokenService;
    }

    public async Task<IdentityResult> RegistrationConfirmation(IdentityExpressUser user)
    {
      bool endpoint = await this.webhookTokenService.PostToEndpoint(this.options.RegistrationConfirmationEndpoint, user.Email);
      bool isSuccess = endpoint;
      return isSuccess ? IdentityResult.Success : IdentityResult.Failed(Array.Empty<IdentityError>());
    }
  }
}
