





using IdentityExpress.Identity;
using IdentityServer4.Admin.Logic.Entities.Exceptions;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Identity;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Logic.Extensions;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class ClaimTypeService : IClaimTypeService
  {
    private readonly IIdentityUnitOfWorkFactory factory;
    private readonly ILookupNormalizer normalizer;

    public ClaimTypeService(IIdentityUnitOfWorkFactory factory, ILookupNormalizer normalizer)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      if (normalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.factory = factory;
      this.normalizer = normalizer;
    }

    public async Task<IdentityResult> Create(ClaimType claimType)
    {
      if (claimType == null)
        throw new ArgumentNullException(nameof (claimType));
      if (string.IsNullOrWhiteSpace(claimType.Name))
        return IdentityResult.Failed(new IdentityError()
        {
          Description = "Claim type name cannot be null"
        });
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IEnumerable<IdentityExpressClaimType> expressClaimTypes = await uow.ClaimTypeRepository.Find((Expression<Func<IdentityExpressClaimType, bool>>) (x => x.NormalizedName == this.normalizer.Normalize(claimType.Name)));
        IEnumerable<IdentityExpressClaimType> foundClaimTypes = expressClaimTypes;
        expressClaimTypes = (IEnumerable<IdentityExpressClaimType>) null;
        if (foundClaimTypes.Any<IdentityExpressClaimType>())
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Claim type with name '" + claimType.Name + "' already exists"
          });
        IdentityResult identityResult1 = await uow.ClaimTypeRepository.Add(claimType.ToDomain(this.normalizer));
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        return result;
      }
    }

    public async Task<IdentityResult> Delete(ClaimType claimType)
    {
      if (claimType == null)
        throw new ArgumentNullException(nameof (claimType));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressClaimType expressClaimType = await uow.ClaimTypeRepository.GetByKey(claimType.Id);
        IdentityExpressClaimType foundClaimType = expressClaimType;
        expressClaimType = (IdentityExpressClaimType) null;
        if (foundClaimType != null)
        {
          if (foundClaimType.Reserved)
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Cannot update reserved claim type"
            });
          IdentityResult identityResult1 = await uow.ClaimTypeRepository.Delete(foundClaimType);
          IdentityResult identityResult = await uow.Commit();
          return identityResult;
        }
        foundClaimType = (IdentityExpressClaimType) null;
      }
      return IdentityResult.Success;
    }

    public async Task<ClaimType> GetById(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentNullException(nameof (id));
      ClaimType service;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressClaimType expressClaimType = await uow.ClaimTypeRepository.GetByKey(id);
        IdentityExpressClaimType foundClaimType = expressClaimType;
        expressClaimType = (IdentityExpressClaimType) null;
        service = foundClaimType.ToService();
      }
      return service;
    }

    public async Task<IList<ClaimType>> Get(string name = null, IList<ClaimTypeOrderBy> ordering = null)
    {
      List<IdentityExpressClaimType> claimTypes;
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        if (!string.IsNullOrWhiteSpace(name))
        {
          IEnumerable<IdentityExpressClaimType> expressClaimTypes = await uow.ClaimTypeRepository.Find((Expression<Func<IdentityExpressClaimType, bool>>) (x => x.NormalizedName.Contains(this.normalizer.Normalize(name))));
          IEnumerable<IdentityExpressClaimType> foundClaims = expressClaimTypes;
          expressClaimTypes = (IEnumerable<IdentityExpressClaimType>) null;
          claimTypes = foundClaims.ToList<IdentityExpressClaimType>();
          foundClaims = (IEnumerable<IdentityExpressClaimType>) null;
        }
        else
        {
          IEnumerable<IdentityExpressClaimType> expressClaimTypes = await uow.ClaimTypeRepository.GetAll();
          IEnumerable<IdentityExpressClaimType> foundClaims = expressClaimTypes;
          expressClaimTypes = (IEnumerable<IdentityExpressClaimType>) null;
          claimTypes = foundClaims.ToList<IdentityExpressClaimType>();
          foundClaims = (IEnumerable<IdentityExpressClaimType>) null;
        }
      }
      IList<ClaimTypeOrderBy> source = ordering;
      if (source != null && source.Any<ClaimTypeOrderBy>())
      {
        if (!ordering.Validate())
          throw new ValidationException("Invalid Ordering Received");
        IOrderedEnumerable<IdentityExpressClaimType> orderedUsers = claimTypes.ApplyOrdering(ordering.First<ClaimTypeOrderBy>());
        foreach (ClaimTypeOrderBy orderBy in ordering.Skip<ClaimTypeOrderBy>(1))
          orderedUsers = QueriableExtensions.ApplyOrdering(orderedUsers, orderBy);
        claimTypes = orderedUsers.ToList<IdentityExpressClaimType>();
        orderedUsers = (IOrderedEnumerable<IdentityExpressClaimType>) null;
      }
      return (IList<ClaimType>) claimTypes.Select<IdentityExpressClaimType, ClaimType>((Func<IdentityExpressClaimType, ClaimType>) (x => x.ToService())).ToList<ClaimType>();
    }

    public async Task<IdentityResult> Update(ClaimType claimType)
    {
      if (claimType == null)
        throw new ArgumentNullException(nameof (claimType));
      using (IIdentityUnitOfWork uow = this.factory.Create())
      {
        IdentityExpressClaimType expressClaimType = await uow.ClaimTypeRepository.GetByKey(claimType.Id);
        IdentityExpressClaimType existingClaimType = expressClaimType;
        expressClaimType = (IdentityExpressClaimType) null;
        if (existingClaimType == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Claim Type with id '" + claimType.Id + "' does not exist"
          });
        IdentityExpressClaimValueType claimValueType;
        Enum.TryParse<IdentityExpressClaimValueType>(claimType.ValueType, out claimValueType);
        if (existingClaimType.Reserved && (claimValueType != existingClaimType.ValueType || claimType.Id != existingClaimType.Id || (claimType.Rule != existingClaimType.Rule || claimType.RuleValidationFailureDescription != existingClaimType.RuleValidationFailureDescription) || claimType.Name != existingClaimType.Name || claimType.Description != existingClaimType.Description))
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Cannot update reserved claim type reserved fields"
          });
        if (claimType.Name != existingClaimType.Name)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Cannot update claim type name"
          });
        IdentityExpressClaimType mappedClaimType = claimType.ToDomain(this.normalizer);
        existingClaimType.Name = mappedClaimType.Name;
        existingClaimType.NormalizedName = mappedClaimType.NormalizedName;
        existingClaimType.Description = mappedClaimType.Description;
        existingClaimType.Rule = mappedClaimType.Rule;
        existingClaimType.RuleValidationFailureDescription = mappedClaimType.RuleValidationFailureDescription;
        existingClaimType.Required = mappedClaimType.Required;
        existingClaimType.ValueType = mappedClaimType.ValueType;
        existingClaimType.UserEditable = mappedClaimType.UserEditable;
        IdentityResult identityResult1 = await uow.ClaimTypeRepository.Update(existingClaimType);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.Commit();
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
        }
        return result;
      }
    }
  }
}
