





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
  public class IdentityResourceService : IIdentityResourceService
  {
    private readonly IIdentityServerUnitOfWorkFactory factory;
    private readonly IValidator<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource> resourceValidator;
    private readonly ILookupNormalizer normalizer;

    public IdentityResourceService(IIdentityServerUnitOfWorkFactory factory, IValidator<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource> resourceValidator, ILookupNormalizer normalizer)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      if (resourceValidator == null)
        throw new ArgumentNullException(nameof (resourceValidator));
      if (normalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.factory = factory;
      this.resourceValidator = resourceValidator;
      this.normalizer = normalizer;
    }

    public async Task<IdentityResult> Create(IdentityServer4.Admin.Logic.Entities.Services.IdentityResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      IdentityResult identityResult1 = await this.resourceValidator.Validate(resource);
      IdentityResult validationResult = identityResult1;
      identityResult1 = (IdentityResult) null;
      if (!validationResult.Succeeded)
        return validationResult;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource = await uow.IdentityResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.IdentityResource foundResource = identityResource;
        identityResource = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        if (foundResource != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Name " + resource.Name + " already exists"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.ApiResource foundApiResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundApiResource != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Name " + resource.Name + " already exists"
          });
        IdentityResult identityResult2 = await uow.IdentityResourceRepository.Add(resource.ToEntity());
        IdentityResult result = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult3 = await uow.ExtendedIdentityResourceRepository.Add(resource.ToExtendedIdentityResource(this.normalizer));
          result = identityResult3;
          identityResult3 = (IdentityResult) null;
          if (result.Succeeded)
          {
            IdentityResult identityResult4 = await uow.Commit();
            result = identityResult4;
            identityResult4 = (IdentityResult) null;
          }
        }
        return result;
      }
    }

    public async Task<IdentityResult> Delete(IdentityServer4.Admin.Logic.Entities.Services.IdentityResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      IdentityResult result = IdentityResult.Success;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedIdentityResource identityResource1 = await uow.ExtendedIdentityResourceRepository.GetByKey(resource.Id);
        ExtendedIdentityResource foundExtendedResource = identityResource1;
        identityResource1 = (ExtendedIdentityResource) null;
        if (foundExtendedResource != null)
        {
          if (resource.NonEditable)
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Cannot delete reserved resource"
            });
          IdentityResult identityResult = await uow.ExtendedIdentityResourceRepository.Delete(foundExtendedResource);
          result = identityResult;
          identityResult = (IdentityResult) null;
        }
        if (!result.Succeeded)
          return result;
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource2 = await uow.IdentityResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.IdentityResource foundResource = identityResource2;
        identityResource2 = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        if (foundResource != null)
        {
          IdentityResult identityResult = await uow.IdentityResourceRepository.Delete(foundResource);
          result = identityResult;
          identityResult = (IdentityResult) null;
        }
        if (!result.Succeeded)
          return result;
        IdentityResult identityResult1 = await uow.Commit();
        result = identityResult1;
        identityResult1 = (IdentityResult) null;
        foundExtendedResource = (ExtendedIdentityResource) null;
        foundResource = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
      }
      return result;
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource> GetById(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedIdentityResource identityResource1 = await uow.ExtendedIdentityResourceRepository.GetByKey(id);
        ExtendedIdentityResource foundExtendedResource = identityResource1;
        identityResource1 = (ExtendedIdentityResource) null;
        if (foundExtendedResource == null)
          return (IdentityServer4.Admin.Logic.Entities.Services.IdentityResource) null;
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource2 = await uow.IdentityResourceRepository.GetByKey(foundExtendedResource.IdentityResourceName);
        IdentityServer4.EntityFramework.Entities.IdentityResource foundResource = identityResource2;
        identityResource2 = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        return foundResource.ToService(foundExtendedResource);
      }
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource> GetByIdentityResourceName(string identityResourceName)
    {
      if (string.IsNullOrWhiteSpace(identityResourceName))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (identityResourceName));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource1 = await uow.IdentityResourceRepository.GetByKey(identityResourceName);
        IdentityServer4.EntityFramework.Entities.IdentityResource foundResource = identityResource1;
        identityResource1 = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        if (foundResource == null)
          return (IdentityServer4.Admin.Logic.Entities.Services.IdentityResource) null;
        ExtendedIdentityResource identityResource2 = await uow.ExtendedIdentityResourceRepository.GetByIdentityResourceName(identityResourceName);
        ExtendedIdentityResource foundExtendedResource = identityResource2;
        identityResource2 = (ExtendedIdentityResource) null;
        return foundResource.ToService(foundExtendedResource);
      }
    }

    public async Task<IList<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource>> Get(string name = null)
    {
      
      
      IdentityResourceService.\u003C\u003Ec__DisplayClass8_0 cDisplayClass80 = new IdentityResourceService.\u003C\u003Ec__DisplayClass8_0();
      
      cDisplayClass80.\u003C\u003E4__this = this;
      
      cDisplayClass80.name = name;
      IList<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource> list;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        
        
        IdentityResourceService.\u003C\u003Ec__DisplayClass8_1 cDisplayClass81 = new IdentityResourceService.\u003C\u003Ec__DisplayClass8_1();
        IEnumerable<IdentityServer4.EntityFramework.Entities.IdentityResource> resources;
        
        if (!string.IsNullOrWhiteSpace(cDisplayClass80.name))
        {
          
          IEnumerable<ExtendedIdentityResource> identityResources1 = await uow.ExtendedIdentityResourceRepository.Find((Expression<Func<ExtendedIdentityResource, bool>>) (x => x.NormalizedName.Contains(this.normalizer.Normalize(cDisplayClass80.name))));
          
          cDisplayClass81.extendedResources = identityResources1;
          identityResources1 = (IEnumerable<ExtendedIdentityResource>) null;
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          
          
          
          IEnumerable<IdentityServer4.EntityFramework.Entities.IdentityResource> identityResources2 = await uow.IdentityResourceRepository.Find(Expression.Lambda<Func<IdentityServer4.EntityFramework.Entities.IdentityResource, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
          {
            cDisplayClass81.extendedResources,
            (Expression) Expression.Lambda<Func<ExtendedIdentityResource, bool>>((Expression) Expression.Equal(y.IdentityResourceName, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityServer4.EntityFramework.Entities.IdentityResource.get_Name)))), new ParameterExpression[1]
            {
              parameterExpression2
            })
          }), new ParameterExpression[1]
          {
            parameterExpression1
          }));
          resources = identityResources2;
          identityResources2 = (IEnumerable<IdentityServer4.EntityFramework.Entities.IdentityResource>) null;
        }
        else
        {
          IEnumerable<ExtendedIdentityResource> identityResources1 = await uow.ExtendedIdentityResourceRepository.GetAll();
          
          cDisplayClass81.extendedResources = identityResources1;
          identityResources1 = (IEnumerable<ExtendedIdentityResource>) null;
          IEnumerable<IdentityServer4.EntityFramework.Entities.IdentityResource> identityResources2 = await uow.IdentityResourceRepository.GetAll();
          resources = identityResources2;
          identityResources2 = (IEnumerable<IdentityServer4.EntityFramework.Entities.IdentityResource>) null;
        }
        
        list = (IList<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource>) resources.Select<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityServer4.Admin.Logic.Entities.Services.IdentityResource>(new Func<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityServer4.Admin.Logic.Entities.Services.IdentityResource>(cDisplayClass81.\u003CGet\u003Eb__2)).ToList<IdentityServer4.Admin.Logic.Entities.Services.IdentityResource>();
      }
      return list;
    }

    public async Task<IdentityResult> Update(IdentityServer4.Admin.Logic.Entities.Services.IdentityResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedIdentityResource identityResource1 = await uow.ExtendedIdentityResourceRepository.GetByKey(resource.Id);
        ExtendedIdentityResource existingExtendedResource = identityResource1;
        identityResource1 = (ExtendedIdentityResource) null;
        if (existingExtendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Id " + resource.Id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource2 = await uow.IdentityResourceRepository.GetByKey(existingExtendedResource.IdentityResourceName);
        IdentityServer4.EntityFramework.Entities.IdentityResource existingResource = identityResource2;
        identityResource2 = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        if (existingResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Id " + resource.Id + " does not exist"
          });
        if (resource.NonEditable)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Cannot update reserved resource"
          });
        if (existingResource.Name != resource.Name)
        {
          IEnumerable<ExtendedIdentityResource> identityResources = await uow.ExtendedIdentityResourceRepository.Find((Expression<Func<ExtendedIdentityResource, bool>>) (x => this.normalizer.Normalize(resource.Name) == x.NormalizedName));
          IEnumerable<ExtendedIdentityResource> duplicateResource = identityResources;
          identityResources = (IEnumerable<ExtendedIdentityResource>) null;
          if (duplicateResource != null && duplicateResource.Any<ExtendedIdentityResource>())
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Resource already exists with name of " + resource.Name
            });
          existingResource.Name = resource.Name;
          existingExtendedResource.IdentityResourceName = resource.Name;
          existingExtendedResource.NormalizedName = this.normalizer.Normalize(resource.Name);
          duplicateResource = (IEnumerable<ExtendedIdentityResource>) null;
        }
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.ApiResource existingApiResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (existingApiResource != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource already exists with name of " + resource.Name
          });
        existingResource.DisplayName = resource.DisplayName;
        existingResource.Description = resource.Description;
        existingResource.Enabled = resource.Enabled;
        existingResource.Emphasize = resource.Emphasize;
        existingResource.Required = resource.Required;
        existingResource.ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument;
        foreach (string str in resource.UserClaims.Where<string>((Func<string, bool>) (x => existingResource.UserClaims.All<IdentityClaim>((Func<IdentityClaim, bool>) (y => x != y.Type)))))
        {
          string claimToAdd = str;
          List<IdentityClaim> userClaims = existingResource.UserClaims;
          IdentityClaim identityClaim = new IdentityClaim();
          identityClaim.Type = claimToAdd;
          userClaims.Add(identityClaim);
          claimToAdd = (string) null;
        }
        foreach (IdentityClaim identityClaim in existingResource.UserClaims.Where<IdentityClaim>((Func<IdentityClaim, bool>) (x => resource.UserClaims.All<string>((Func<string, bool>) (y => x.Type != y)))).ToList<IdentityClaim>())
        {
          IdentityClaim claimToRemove = identityClaim;
          existingResource.UserClaims.Remove(claimToRemove);
          claimToRemove = (IdentityClaim) null;
        }
        IdentityResult identityResult1 = await uow.ExtendedIdentityResourceRepository.Update(existingExtendedResource);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.IdentityResourceRepository.Update(existingResource);
          result = identityResult2;
          identityResult2 = (IdentityResult) null;
          if (result.Succeeded)
          {
            IdentityResult identityResult3 = await uow.Commit();
            result = identityResult3;
            identityResult3 = (IdentityResult) null;
          }
        }
        return result;
      }
    }
  }
}
