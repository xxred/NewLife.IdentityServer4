





using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Options;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class WebhookService : IWebhookService
  {
    private readonly WebhookOptions options;
    private readonly ILogger<WebhookService> logger;

    public WebhookService(IOptions<WebhookOptions> options, ILogger<WebhookService> logger)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      this.options = options.Value;
      ILogger<WebhookService> logger1 = logger;
      if (logger1 == null)
        throw new ArgumentNullException(nameof (logger));
      this.logger = logger1;
    }

    private async Task<string> GetAccessToken()
    {
      DiscoveryClient discoClient = new DiscoveryClient(this.options.AuthorityUrl + "/.well-known/openid-configuration", (HttpMessageHandler) null);
      DiscoveryResponse discoveryResponse = await discoClient.GetAsync(new CancellationToken());
      DiscoveryResponse discoDocument = discoveryResponse;
      discoveryResponse = (DiscoveryResponse) null;
      if (discoDocument.IsError)
      {
        this.logger.LogError(string.Format("Error return from discovery document. Error Type: {0}. Error: {1}", (object) discoDocument.ErrorType, (object) discoDocument.Error), Array.Empty<object>());
        return (string) null;
      }
      TokenClient tokenClient = new TokenClient(discoDocument.TokenEndpoint, this.options.ClientId, this.options.ClientSecret, (HttpMessageHandler) null, AuthenticationStyle.BasicAuthentication);
      TokenResponse tokenResponse1 = await tokenClient.RequestClientCredentialsAsync((string) null, (object) null, new CancellationToken());
      TokenResponse tokenResponse2 = tokenResponse1;
      tokenResponse1 = (TokenResponse) null;
      if (!tokenResponse2.IsError)
        return tokenResponse2.AccessToken;
      this.logger.LogError(string.Format("Error during webhook token request. Error Type: {0}. Error: {1}", (object) tokenResponse2.ErrorType, (object) tokenResponse2.Error), Array.Empty<object>());
      return (string) null;
    }

    public async Task<bool> PostToEndpoint(string endpointUrl, string email)
    {
      string str = await this.GetAccessToken();
      string token = str;
      str = (string) null;
      if (token == null)
        return false;
      HttpClient httpClient = new HttpClient();
      httpClient.SetBearerToken(token);
      HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(endpointUrl, (HttpContent) new StringContent("{ \"Email\":\"" + email + "\"}", Encoding.UTF8, "application/json"));
      HttpResponseMessage responseMessage = httpResponseMessage;
      httpResponseMessage = (HttpResponseMessage) null;
      if (responseMessage.IsSuccessStatusCode)
        return true;
      this.logger.LogError(string.Format("Error during webhook call. Status Code: {0}", (object) responseMessage.StatusCode), Array.Empty<object>());
      return false;
    }
  }
}
