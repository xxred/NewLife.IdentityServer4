





using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Validators
{
  public class ClientValidator : IValidator<Client>
  {
    public Task<IdentityResult> Validate(Client input)
    {
      if (input == null)
        throw new ArgumentNullException(nameof (input));
      List<IdentityError> source = new List<IdentityError>();
      if (string.IsNullOrWhiteSpace(input.ClientId))
        source.Add(new IdentityError()
        {
          Description = "Client Id cannot be null"
        });
      if (string.IsNullOrWhiteSpace(input.ClientName))
        source.Add(new IdentityError()
        {
          Description = "Client Name cannot be null"
        });
      if (!string.IsNullOrWhiteSpace(input.LogoUri))
        source.AddRange(ClientValidator.ValidateSecureUrl(input.LogoUri));
      if (input.AllowedGrantTypes.Contains("implicit") || input.AllowedGrantTypes.Contains("hybrid") || input.AllowedGrantTypes.Contains("authorization_code"))
        source.AddRange(ClientValidator.ValidateRedirectUris(input));
      return source.Any<IdentityError>() ? Task.FromResult<IdentityResult>(IdentityResult.Failed(source.ToArray())) : Task.FromResult<IdentityResult>(IdentityResult.Success);
    }

    private static IEnumerable<IdentityError> ValidateSecureUrl(string url)
    {
      List<IdentityError> identityErrorList = new List<IdentityError>();
      Uri result;
      if (Uri.TryCreate(url, UriKind.Absolute, out result) && result.Scheme == "https")
        return (IEnumerable<IdentityError>) identityErrorList;
      identityErrorList.Add(new IdentityError()
      {
        Description = url + " is not a valid https url"
      });
      return (IEnumerable<IdentityError>) identityErrorList;
    }

    private static IEnumerable<IdentityError> ValidateRedirectUris(Client client)
    {
      List<IdentityError> identityErrorList = new List<IdentityError>();
      if (client.RedirectUris == null || !client.RedirectUris.Any<string>())
        identityErrorList.Add(new IdentityError()
        {
          Description = "Client Redirect Uris cannot be empty"
        });
      return (IEnumerable<IdentityError>) identityErrorList;
    }
  }
}
