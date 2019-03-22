





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
    public class UserClaimsService : IUserClaimsService
  {
    private readonly IIdentityUnitOfWorkFactory factory;

    public UserClaimsService(IIdentityUnitOfWorkFactory factory)
    {
      IIdentityUnitOfWorkFactory unitOfWorkFactory = factory;
      if (unitOfWorkFactory == null)
        throw new ArgumentNullException(nameof (factory));
      this.factory = unitOfWorkFactory;
    }

    public async Task<UserClaim> GetUserEditableClaims(User user)
    {
      UserClaim userClaim1;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IEnumerable<IdentityExpressClaimType> expressClaimTypes = await uow.ClaimTypeRepository.Find((Expression<Func<IdentityExpressClaimType, bool>>) (x => x.UserEditable));
        IEnumerable<IdentityExpressClaimType> editableClaims = expressClaimTypes;
        expressClaimTypes = (IEnumerable<IdentityExpressClaimType>) null;
        List<IdentityExpressClaimType> identityExpressClaimTypes = editableClaims.ToList<IdentityExpressClaimType>();
        IEnumerable<string> editableClaimIds = identityExpressClaimTypes.Select<IdentityExpressClaimType, string>((Func<IdentityExpressClaimType, string>) (x => x.Name));
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(user.Subject);
        IdentityExpressUser dbUser = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        List<IdentityExpressClaim> currentUserClaims = dbUser.Claims.Where<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => editableClaimIds.Contains<string>(x.ClaimType))).ToList<IdentityExpressClaim>();
        foreach (IdentityExpressClaimType expressClaimType in identityExpressClaimTypes)
        {
          IdentityExpressClaimType editableClaim = expressClaimType;
          IdentityExpressClaim existingType = currentUserClaims.FirstOrDefault<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => x.ClaimType == editableClaim.Name));
          if (existingType == null)
          {
            List<IdentityExpressClaim> identityExpressClaimList = currentUserClaims;
            IdentityExpressClaim identityExpressClaim = new IdentityExpressClaim();
            identityExpressClaim.ClaimType = editableClaim.Name;
            identityExpressClaim.Id = 0;
            identityExpressClaim.UserId = dbUser.Id;
            identityExpressClaimList.Add(identityExpressClaim);
          }
          existingType = (IdentityExpressClaim) null;
        }
        UserClaim userClaim = new UserClaim()
        {
          Subject = dbUser.Id,
          Claims = currentUserClaims.Select<IdentityExpressClaim, ClaimDto>((Func<IdentityExpressClaim, ClaimDto>) (x => x.ToService())).ToList<ClaimDto>()
        };
        userClaim1 = userClaim;
      }
      return userClaim1;
    }

    public async Task<IdentityResult> UpdateUserEditableClaims(UserClaim userClaim)
    {
      if (userClaim.Claims.Any<ClaimDto>((Func<ClaimDto, bool>) (c => c.Value == null)))
        return IdentityResult.Failed(Array.Empty<IdentityError>());
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressUser identityExpressUser = await uow.UserManager.FindByIdAsync(userClaim.Subject);
        IdentityExpressUser user = identityExpressUser;
        identityExpressUser = (IdentityExpressUser) null;
        IEnumerable<IdentityExpressClaimType> source = await uow.ClaimTypeRepository.Find((Expression<Func<IdentityExpressClaimType, bool>>) (x => x.UserEditable));
        Dictionary<string, IdentityExpressClaimType> userEditableClaimTypes = source.ToDictionary<IdentityExpressClaimType, string, IdentityExpressClaimType>((Func<IdentityExpressClaimType, string>) (c => c.Name), (Func<IdentityExpressClaimType, IdentityExpressClaimType>) (c => c));
        source = (IEnumerable<IdentityExpressClaimType>) null;
        if (!userClaim.Claims.All<ClaimDto>((Func<ClaimDto, bool>) (c => userEditableClaimTypes.ContainsKey(c.Type))))
          return IdentityResult.Failed(Array.Empty<IdentityError>());
        List<IdentityExpressClaim> claimsToDelete = user.Claims.Where<IdentityExpressClaim>((Func<IdentityExpressClaim, bool>) (x => userEditableClaimTypes.ContainsKey(x.ClaimType))).ToList<IdentityExpressClaim>();
        claimsToDelete.ForEach((Action<IdentityExpressClaim>) (c => user.Claims.Remove(c)));
        foreach (ClaimDto claim1 in userClaim.Claims)
        {
          ClaimDto claim = claim1;
          ICollection<IdentityExpressClaim> claims = user.Claims;
          IdentityExpressClaim identityExpressClaim = new IdentityExpressClaim();
          identityExpressClaim.ClaimType = claim.Type;
          identityExpressClaim.ClaimValue = claim.Value;
          claims.Add(identityExpressClaim);
          claim = (ClaimDto) null;
        }
        IdentityResult identityResult = await uow.Commit();
        return identityResult;
      }
    }
  }
}
