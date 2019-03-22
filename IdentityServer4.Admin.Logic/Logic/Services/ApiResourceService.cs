





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using IdentityServer4.Admin.Logic.Logic.Mappers;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Logic.Logic.Services
{
    public class ApiResourceService : IApiResourceService
  {
    private readonly IIdentityServerUnitOfWorkFactory factory;
    private readonly IValidator<IdentityServer4.Admin.Logic.Entities.Services.ApiResource> resourceValidator;
    private readonly ILookupNormalizer normalizer;

    public ApiResourceService(IIdentityServerUnitOfWorkFactory factory, IValidator<IdentityServer4.Admin.Logic.Entities.Services.ApiResource> resourceValidator, ILookupNormalizer normalizer)
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

    public async Task<IdentityResult> Create(IdentityServer4.Admin.Logic.Entities.Services.ApiResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      IdentityResult identityResult1 = await this.resourceValidator.Validate(resource);
      IdentityResult validationResult = identityResult1;
      identityResult1 = (IdentityResult) null;
      if (!validationResult.Succeeded)
        return validationResult;
      IIdentityServerUnitOfWork uow = this.factory.Create();
      try
      {
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Name " + resource.Name + " already exists"
          });
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource = await uow.IdentityResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.IdentityResource foundIdentityResource = identityResource;
        identityResource = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        if (foundIdentityResource != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Name " + resource.Name + " already exists"
          });
        resource.Scopes.Add(new Scope()
        {
          Name = resource.Name ?? "",
          DisplayName = resource.DisplayName ?? "",
          Description = resource.DisplayName + " - Full Access"
        });
        IdentityResult identityResult2 = await uow.ApiResourceRepository.Add(resource.ToDomain());
        IdentityResult result = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult3 = await uow.ExtendedApiResourceRepository.Add(resource.ToExtendedApiResource(this.normalizer));
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
      finally
      {
        if ((object) uow != null)
          uow.Dispose();
      }
    }

    public async Task<IdentityResult> Delete(IdentityServer4.Admin.Logic.Entities.Services.ApiResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      IdentityResult result = IdentityResult.Success;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(resource.Id);
        ExtendedApiResource foundExtendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (foundExtendedResource != null)
        {
          if (resource.NonEditable)
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Cannot delete reserved resource"
            });
          IdentityResult identityResult = await uow.ExtendedApiResourceRepository.Delete(foundExtendedResource);
          result = identityResult;
          identityResult = (IdentityResult) null;
        }
        if (!result.Succeeded)
          return result;
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource != null)
        {
          IdentityResult identityResult = await uow.ApiResourceRepository.Delete(foundResource);
          result = identityResult;
          identityResult = (IdentityResult) null;
        }
        if (!result.Succeeded)
          return result;
        IdentityResult identityResult1 = await uow.Commit();
        result = identityResult1;
        identityResult1 = (IdentityResult) null;
        foundExtendedResource = (ExtendedApiResource) null;
        foundResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
      }
      return result;
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.ApiResource> GetById(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource foundExtendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (foundExtendedResource == null)
          return (IdentityServer4.Admin.Logic.Entities.Services.ApiResource) null;
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(foundExtendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        return foundResource.ToService(foundExtendedResource);
      }
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.ApiResource> GetByApiResourceName(string apiResourceName)
    {
      if (string.IsNullOrWhiteSpace(apiResourceName))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (apiResourceName));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(apiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return (IdentityServer4.Admin.Logic.Entities.Services.ApiResource) null;
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByApiResourceName(apiResourceName);
        ExtendedApiResource foundExtendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        return foundResource.ToService(foundExtendedResource);
      }
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.ApiResource> GetApiResourceByScopeName(string scopeName)
    {
      if (string.IsNullOrWhiteSpace(scopeName))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (scopeName));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource> source = await uow.ApiResourceRepository.Find((Expression<Func<IdentityServer4.EntityFramework.Entities.ApiResource, bool>>) (x => x.Scopes.Any<ApiScope>((Func<ApiScope, bool>) (y => y.Name == scopeName))));
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = source.SingleOrDefault<IdentityServer4.EntityFramework.Entities.ApiResource>();
        source = (IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource>) null;
        if (foundResource == null)
          return (IdentityServer4.Admin.Logic.Entities.Services.ApiResource) null;
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByApiResourceName(foundResource.Name);
        ExtendedApiResource foundExtendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        return foundResource.ToService(foundExtendedResource);
      }
    }

    public async Task<IList<IdentityServer4.Admin.Logic.Entities.Services.ApiResource>> Get(string name = null)
    {
      
      
      ApiResourceService.\u003C\u003Ec__DisplayClass9_0 cDisplayClass90 = new ApiResourceService.\u003C\u003Ec__DisplayClass9_0();
      
      cDisplayClass90.\u003C\u003E4__this = this;
      
      cDisplayClass90.name = name;
      IList<IdentityServer4.Admin.Logic.Entities.Services.ApiResource> list;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        
        
        ApiResourceService.\u003C\u003Ec__DisplayClass9_1 cDisplayClass91 = new ApiResourceService.\u003C\u003Ec__DisplayClass9_1();
        IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource> resources;
        
        if (!string.IsNullOrWhiteSpace(cDisplayClass90.name))
        {
          
          IEnumerable<ExtendedApiResource> extendedApiResources = await uow.ExtendedApiResourceRepository.Find((Expression<Func<ExtendedApiResource, bool>>) (x => x.NormalizedName.Contains(this.normalizer.Normalize(cDisplayClass90.name))));
          
          cDisplayClass91.extendedResources = extendedApiResources;
          extendedApiResources = (IEnumerable<ExtendedApiResource>) null;
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          
          
          
          IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource> apiResources = await uow.ApiResourceRepository.Find(Expression.Lambda<Func<IdentityServer4.EntityFramework.Entities.ApiResource, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
          {
            cDisplayClass91.extendedResources,
            (Expression) Expression.Lambda<Func<ExtendedApiResource, bool>>((Expression) Expression.Equal(y.ApiResourceName, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityServer4.EntityFramework.Entities.ApiResource.get_Name)))), new ParameterExpression[1]
            {
              parameterExpression2
            })
          }), new ParameterExpression[1]
          {
            parameterExpression1
          }));
          resources = apiResources;
          apiResources = (IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource>) null;
        }
        else
        {
          IEnumerable<ExtendedApiResource> extendedApiResources = await uow.ExtendedApiResourceRepository.GetAll();
          
          cDisplayClass91.extendedResources = extendedApiResources;
          extendedApiResources = (IEnumerable<ExtendedApiResource>) null;
          IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource> apiResources = await uow.ApiResourceRepository.GetAll();
          resources = apiResources;
          apiResources = (IEnumerable<IdentityServer4.EntityFramework.Entities.ApiResource>) null;
        }
        
        list = (IList<IdentityServer4.Admin.Logic.Entities.Services.ApiResource>) resources.Select<IdentityServer4.EntityFramework.Entities.ApiResource, IdentityServer4.Admin.Logic.Entities.Services.ApiResource>(new Func<IdentityServer4.EntityFramework.Entities.ApiResource, IdentityServer4.Admin.Logic.Entities.Services.ApiResource>(cDisplayClass91.\u003CGet\u003Eb__2)).ToList<IdentityServer4.Admin.Logic.Entities.Services.ApiResource>();
      }
      return list;
    }

    public async Task<IdentityResult> Update(IdentityServer4.Admin.Logic.Entities.Services.ApiResource resource)
    {
      if (resource == null)
        throw new ArgumentNullException(nameof (resource));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(resource.Id);
        ExtendedApiResource existingExtendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (existingExtendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource with Id " + resource.Id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(existingExtendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource existingResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
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
          IEnumerable<ExtendedApiResource> extendedApiResources = await uow.ExtendedApiResourceRepository.Find((Expression<Func<ExtendedApiResource, bool>>) (x => this.normalizer.Normalize(resource.Name) == x.NormalizedName));
          IEnumerable<ExtendedApiResource> duplicateResource = extendedApiResources;
          extendedApiResources = (IEnumerable<ExtendedApiResource>) null;
          if (duplicateResource != null && duplicateResource.Any<ExtendedApiResource>())
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Resource already exists with name of " + resource.Name
            });
          existingResource.Name = resource.Name;
          existingExtendedResource.ApiResourceName = resource.Name;
          existingExtendedResource.NormalizedName = this.normalizer.Normalize(resource.Name);
          duplicateResource = (IEnumerable<ExtendedApiResource>) null;
        }
        IdentityServer4.EntityFramework.Entities.IdentityResource identityResource = await uow.IdentityResourceRepository.GetByKey(resource.Name);
        IdentityServer4.EntityFramework.Entities.IdentityResource existingIdentityResource = identityResource;
        identityResource = (IdentityServer4.EntityFramework.Entities.IdentityResource) null;
        if (existingIdentityResource != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Resource already exists with name of " + resource.Name
          });
        existingResource.DisplayName = resource.DisplayName;
        existingResource.Description = resource.Description;
        existingResource.Enabled = resource.Enabled;
        foreach (string str in resource.UserClaims.Where<string>((Func<string, bool>) (x => existingResource.UserClaims.All<ApiResourceClaim>((Func<ApiResourceClaim, bool>) (y => x != y.Type)))))
        {
          string claimToAdd = str;
          List<ApiResourceClaim> userClaims = existingResource.UserClaims;
          ApiResourceClaim apiResourceClaim = new ApiResourceClaim();
          apiResourceClaim.Type = claimToAdd;
          userClaims.Add(apiResourceClaim);
          claimToAdd = (string) null;
        }
        foreach (ApiResourceClaim apiResourceClaim in existingResource.UserClaims.Where<ApiResourceClaim>((Func<ApiResourceClaim, bool>) (x => resource.UserClaims.All<string>((Func<string, bool>) (y => x.Type != y)))).ToList<ApiResourceClaim>())
        {
          ApiResourceClaim claimToRemove = apiResourceClaim;
          existingResource.UserClaims.Remove(claimToRemove);
          claimToRemove = (ApiResourceClaim) null;
        }
        IdentityResult identityResult1 = await uow.ExtendedApiResourceRepository.Update(existingExtendedResource);
        IdentityResult result = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult2 = await uow.ApiResourceRepository.Update(existingResource);
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

    public async Task<IdentityResult> AddSecret(string id, PlainTextSecret secret)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (secret == null)
        throw new ArgumentNullException(nameof (secret));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource extendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (extendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(extendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        foundResource.Secrets.Add(secret.ToApiSecret());
        IdentityResult identityResult1 = await uow.ApiResourceRepository.Update(foundResource);
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

    public async Task<IdentityResult> UpdateSecret(string id, PlainTextSecret secret)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (secret == null)
        throw new ArgumentNullException(nameof (secret));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource extendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (extendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(extendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        ApiSecret foundSecret = foundResource.Secrets.FirstOrDefault<ApiSecret>((Func<ApiSecret, bool>) (x => x.Id == secret.Id));
        if (foundSecret == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = string.Format("Secret with id {0} does not exist", (object) secret.Id)
          });
        ApiSecret mappedSecret = secret.ToApiSecret();
        foundSecret.Type = mappedSecret.Type;
        foundSecret.Description = mappedSecret.Description;
        foundSecret.Expiration = mappedSecret.Expiration;
        if ((uint) string.Compare(mappedSecret.Type, "SharedSecret", StringComparison.CurrentCultureIgnoreCase) > 0U)
          foundSecret.Value = mappedSecret.Value;
        IdentityResult identityResult1 = await uow.ApiResourceRepository.Update(foundResource);
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

    public async Task<IdentityResult> DeleteSecret(string id, Secret secret)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (secret == null)
        throw new ArgumentNullException(nameof (secret));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource extendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (extendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(extendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        ApiSecret foundSecret = foundResource.Secrets.FirstOrDefault<ApiSecret>((Func<ApiSecret, bool>) (x => x.Id == secret.Id));
        if (foundSecret == null)
          return IdentityResult.Success;
        foundResource.Secrets.Remove(foundSecret);
        IdentityResult identityResult1 = await uow.ApiResourceRepository.Update(foundResource);
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

    public async Task<IdentityResult> AddScope(string id, Scope scope)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (scope == null)
        throw new ArgumentNullException(nameof (scope));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource extendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (extendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(extendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        if (foundResource.Scopes.FirstOrDefault<ApiScope>((Func<ApiScope, bool>) (x => x.Name == scope.Name)) != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Scope with name " + scope.Name + " already exists"
          });
        foundResource.Scopes.Add(new ApiScope()
        {
          Name = scope.Name,
          DisplayName = scope.DisplayName,
          Required = scope.Required,
          Description = scope.Description,
          Emphasize = scope.Emphasize,
          ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument,
          UserClaims = scope.UserClaims != null ? scope.UserClaims.Select<string, ApiScopeClaim>((Func<string, ApiScopeClaim>) (x =>
          {
            return new ApiScopeClaim() { Type = x };
          })).ToList<ApiScopeClaim>() : new List<ApiScopeClaim>()
        });
        IdentityResult identityResult1 = await uow.ApiResourceRepository.Update(foundResource);
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

    public async Task<IdentityResult> UpdateScope(string id, Scope scope)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (scope == null)
        throw new ArgumentNullException(nameof (scope));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource extendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (extendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(extendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        ApiScope foundScope = foundResource.Scopes.FirstOrDefault<ApiScope>((Func<ApiScope, bool>) (x => x.Id == scope.Id));
        if (foundScope == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = string.Format("Scope with id {0} does not exist", (object) scope.Id)
          });
        foundScope.Name = scope.Name;
        foundScope.DisplayName = scope.DisplayName;
        foundScope.Description = scope.Description;
        foundScope.Required = scope.Required;
        foundScope.Emphasize = scope.Emphasize;
        foundScope.ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument;
        foreach (string str in scope.UserClaims.Where<string>((Func<string, bool>) (x => foundScope.UserClaims.All<ApiScopeClaim>((Func<ApiScopeClaim, bool>) (y => x != y.Type)))))
        {
          string scopeToAdd = str;
          List<ApiScopeClaim> userClaims = foundScope.UserClaims;
          ApiScopeClaim apiScopeClaim = new ApiScopeClaim();
          apiScopeClaim.Type = scopeToAdd;
          userClaims.Add(apiScopeClaim);
          scopeToAdd = (string) null;
        }
        foreach (ApiScopeClaim apiScopeClaim in foundScope.UserClaims.Where<ApiScopeClaim>((Func<ApiScopeClaim, bool>) (x => scope.UserClaims.All<string>((Func<string, bool>) (y => x.Type != y)))).ToList<ApiScopeClaim>())
        {
          ApiScopeClaim scopeToRemove = apiScopeClaim;
          foundScope.UserClaims.Remove(scopeToRemove);
          scopeToRemove = (ApiScopeClaim) null;
        }
        IdentityResult identityResult1 = await uow.ApiResourceRepository.Update(foundResource);
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

    public async Task<IdentityResult> DeleteScope(string id, Scope scope)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (scope == null)
        throw new ArgumentNullException(nameof (scope));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedApiResource extendedApiResource = await uow.ExtendedApiResourceRepository.GetByKey(id);
        ExtendedApiResource extendedResource = extendedApiResource;
        extendedApiResource = (ExtendedApiResource) null;
        if (extendedResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.ApiResource apiResource = await uow.ApiResourceRepository.GetByKey(extendedResource.ApiResourceName);
        IdentityServer4.EntityFramework.Entities.ApiResource foundResource = apiResource;
        apiResource = (IdentityServer4.EntityFramework.Entities.ApiResource) null;
        if (foundResource == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        ApiScope foundScope = foundResource.Scopes.FirstOrDefault<ApiScope>((Func<ApiScope, bool>) (x => x.Id == scope.Id));
        if (foundScope == null)
          return IdentityResult.Success;
        foundResource.Scopes.Remove(foundScope);
        IdentityResult identityResult1 = await uow.ApiResourceRepository.Update(foundResource);
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
