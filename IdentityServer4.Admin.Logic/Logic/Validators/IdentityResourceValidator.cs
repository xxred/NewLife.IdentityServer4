





using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Validators
{
  public class IdentityResourceValidator : IValidator<IdentityResource>
  {
    private readonly IValidator<IList<string>> claimValidator;

    public IdentityResourceValidator(IValidator<IList<string>> claimValidator)
    {
      if (claimValidator == null)
        throw new ArgumentNullException(nameof (claimValidator));
      this.claimValidator = claimValidator;
    }

    public async Task<IdentityResult> Validate(IdentityResource resource)
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
