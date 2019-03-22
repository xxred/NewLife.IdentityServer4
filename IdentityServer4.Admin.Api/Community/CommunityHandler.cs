
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Community
{
  public class CommunityHandler : AuthenticationHandler<CommunityOptions>
  {
    public CommunityHandler(IOptionsMonitor<CommunityOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
      : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      ClaimsPrincipal principal1 = new ClaimsPrincipal((IIdentity) new ClaimsIdentity((IEnumerable<Claim>) new List<Claim>() { new Claim("sub", "Community"), new Claim("name", "Admin"), new Claim("role", "AdminUI Administrator") }, this.Scheme.Name));
      SignInContext signInContext = new SignInContext(this.Scheme.Name, principal1, (IDictionary<string, string>) new Dictionary<string, string>());
      ClaimsPrincipal principal2 = principal1;
      AuthenticationProperties properties = new AuthenticationProperties(signInContext.Properties);
      properties.RedirectUri = this.BuildRedirectUri((string) this.Request.Path);
      string name = this.Scheme.Name;
      return Task.FromResult<AuthenticateResult>((AuthenticateResult) HandleRequestResult.Success(new AuthenticationTicket(principal2, properties, name)));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
      return Task.CompletedTask;
    }
  }
}
