





using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Validators
{
    public class ApiResourceValidator : IValidator<ApiResource>
  {
    private readonly IValidator<IList<string>> claimValidator;

    public ApiResourceValidator(IValidator<IList<string>> claimValidator)
    {
      if (claimValidator == null)
        throw new ArgumentNullException(nameof (claimValidator));
      this.claimValidator = claimValidator;
    }

    public async Task<IdentityResult> Validate(ApiResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      List<IdentityError> errors = new List<IdentityError>();
      if (string.IsNullOrWhiteSpace(resource.Name))
        errors.Add(new IdentityError()
        {
          Description = "Resource Name cannot be null"
        });
      if (string.IsNullOrWhiteSpace(resource.DisplayName))
        errors.Add(new IdentityError()
        {
          Description = "Resource Diplay Name cannot be null"
        });
      if (resource.ApiSecrets != null && resource.ApiSecrets.Any<Secret>())
      {
        foreach (Secret apiSecret in (IEnumerable<Secret>) resource.ApiSecrets)
        {
          Secret secret = apiSecret;
          if (string.IsNullOrWhiteSpace(secret.Type))
            errors.Add(new IdentityError()
            {
              Description = "Secret Type cannot be null"
            });
          if (string.IsNullOrWhiteSpace(secret.Value))
            errors.Add(new IdentityError()
            {
              Description = "Secret Value cannot be null"
            });
          secret = (Secret) null;
        }
      }
      if (resource.Scopes != null && resource.Scopes.Any<Scope>())
      {
        foreach (Scope scope1 in (IEnumerable<Scope>) resource.Scopes)
        {
          Scope scope = scope1;
          if (string.IsNullOrWhiteSpace(scope.Name))
            errors.Add(new IdentityError()
            {
              Description = "Scope Name cannot be null"
            });
          if (string.IsNullOrWhiteSpace(scope.DisplayName))
            errors.Add(new IdentityError()
            {
              Description = "Scope Display Name cannot be null"
            });
          IdentityResult identityResult = await this.claimValidator.Validate((IList<string>) scope.UserClaims.ToList<string>());
          IdentityResult claimValidation = identityResult;
          identityResult = (IdentityResult) null;
          if (!claimValidation.Succeeded)
            errors.AddRange(claimValidation.Errors);
          claimValidation = (IdentityResult) null;
          scope = (Scope) null;
        }
      }
      if (resource.UserClaims != null && resource.UserClaims.Any<string>())
      {
        IdentityResult identityResult = await this.claimValidator.Validate((IList<string>) resource.UserClaims.ToList<string>());
        IdentityResult claimValidation = identityResult;
        identityResult = (IdentityResult) null;
        if (!claimValidation.Succeeded)
          errors.AddRange(claimValidation.Errors);
        claimValidation = (IdentityResult) null;
      }
      return errors.Any<IdentityError>() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }
  }
}
