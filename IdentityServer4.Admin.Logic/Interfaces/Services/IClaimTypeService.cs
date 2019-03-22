





using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Interfaces.Services
{
  public interface IClaimTypeService
  {
    Task<IdentityResult> Create(ClaimType claimType);

    Task<IdentityResult> Delete(ClaimType claimType);

    Task<ClaimType> GetById(string id);

    Task<IList<ClaimType>> Get(string name = null, IList<ClaimTypeOrderBy> ordering = null);

    Task<IdentityResult> Update(ClaimType claimType);
  }
}
