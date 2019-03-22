





using IdentityServer4.Admin.Logic.Entities.IdentityServer;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.IdentityServer;
using IdentityServer4.Admin.Logic.Interfaces.Mappers;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using IdentityServer4.Admin.Logic.Logic.Licensing;
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
    public class ClientService : IClientService
  {
    private readonly IIdentityServerUnitOfWorkFactory factory;
    private readonly IValidator<IdentityServer4.Admin.Logic.Entities.Services.Client> clientValidator;
    private readonly IMapper<GenericClient, IdentityServer4.Admin.Logic.Entities.Services.Client> genericClientMapper;
    private readonly ILookupNormalizer normalizer;
    private readonly ILicenseValidationService clientLicensingService;

    public ClientService(IIdentityServerUnitOfWorkFactory factory, IValidator<IdentityServer4.Admin.Logic.Entities.Services.Client> clientValidator, IMapper<GenericClient, IdentityServer4.Admin.Logic.Entities.Services.Client> genericClientMapper, ILookupNormalizer normalizer, ILicenseValidationService licensingService)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      if (clientValidator == null)
        throw new ArgumentNullException(nameof (clientValidator));
      if (genericClientMapper == null)
        throw new ArgumentNullException(nameof (genericClientMapper));
      if (normalizer == null)
        throw new ArgumentNullException(nameof (normalizer));
      this.factory = factory;
      this.clientValidator = clientValidator;
      this.genericClientMapper = genericClientMapper;
      this.normalizer = normalizer;
      this.clientLicensingService = licensingService;
    }

    public async Task<IdentityResult> Create(GenericClient client)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      bool flag = await this.clientLicensingService.IsWithinClientQuota(true);
      if (!flag)
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "420",
          Description = "Client quota reached"
        });
      IdentityServer4.Admin.Logic.Entities.Services.Client mappedClient = this.genericClientMapper.Map(client);
      IdentityResult identityResult1 = await this.clientValidator.Validate(mappedClient);
      IdentityResult validationResult = identityResult1;
      identityResult1 = (IdentityResult) null;
      if (!validationResult.Succeeded)
        return validationResult;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityServer4.EntityFramework.Entities.Client client1 = await uow.ClientRepository.GetByKey(mappedClient.ClientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client1;
        client1 = (IdentityServer4.EntityFramework.Entities.Client) null;
        if (foundClient != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + mappedClient.ClientId + " already exists"
          });
        ExtendedClient extendedClient = await uow.ExtendedClientRepository.GetByNormalizedClientName(this.normalizer.Normalize(client.ClientName));
        ExtendedClient foundExtendedClient = extendedClient;
        extendedClient = (ExtendedClient) null;
        if (foundExtendedClient != null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with client name " + mappedClient.ClientName + " already exists"
          });
        IdentityResult identityResult2 = await uow.ClientRepository.Add(mappedClient.ToDomain());
        IdentityResult result = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult3 = await uow.ExtendedClientRepository.Add(mappedClient.ToExtendedClient(this.normalizer));
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

    public async Task<IdentityResult> Delete(IdentityServer4.Admin.Logic.Entities.Services.Client client)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      IdentityResult result = IdentityResult.Success;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedClient extendedClient = await uow.ExtendedClientRepository.GetByClientId(client.ClientId);
        ExtendedClient foundExtendedClient = extendedClient;
        extendedClient = (ExtendedClient) null;
        if (foundExtendedClient != null)
        {
          if (client.NonEditable)
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Cannot delete reserved client"
            });
          IdentityResult identityResult = await uow.ExtendedClientRepository.Delete(foundExtendedClient);
          result = identityResult;
          identityResult = (IdentityResult) null;
        }
        if (!result.Succeeded)
          return result;
        IdentityServer4.EntityFramework.Entities.Client client1 = await uow.ClientRepository.GetByKey(client.ClientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client1;
        client1 = (IdentityServer4.EntityFramework.Entities.Client) null;
        if (foundClient != null)
        {
          IdentityResult identityResult = await uow.ClientRepository.Delete(foundClient);
          result = identityResult;
          identityResult = (IdentityResult) null;
        }
        if (!result.Succeeded)
          return result;
        IdentityResult identityResult1 = await uow.Commit();
        result = identityResult1;
        identityResult1 = (IdentityResult) null;
        foundExtendedClient = (ExtendedClient) null;
        foundClient = (IdentityServer4.EntityFramework.Entities.Client) null;
      }
      return result;
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.Client> GetById(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentNullException(nameof (id), "Value cannot be null or whitespace.");
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedClient extendedClient = await uow.ExtendedClientRepository.GetByKey(id);
        ExtendedClient foundExtendedClient = extendedClient;
        extendedClient = (ExtendedClient) null;
        if (foundExtendedClient == null)
          return (IdentityServer4.Admin.Logic.Entities.Services.Client) null;
        IdentityServer4.EntityFramework.Entities.Client client = await uow.ClientRepository.GetByKey(foundExtendedClient.ClientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client;
        client = (IdentityServer4.EntityFramework.Entities.Client) null;
        return foundClient.ToService(foundExtendedClient);
      }
    }

    public async Task<IdentityServer4.Admin.Logic.Entities.Services.Client> GetByClientId(string clientId)
    {
      if (string.IsNullOrWhiteSpace(clientId))
        throw new ArgumentNullException(nameof (clientId), "Value cannot be null or whitespace.");
      IdentityServer4.Admin.Logic.Entities.Services.Client service;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityServer4.EntityFramework.Entities.Client client = await uow.ClientRepository.GetByKey(clientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client;
        client = (IdentityServer4.EntityFramework.Entities.Client) null;
        ExtendedClient extendedClient = await uow.ExtendedClientRepository.GetByClientId(foundClient.ClientId);
        ExtendedClient foundExtendedClient = extendedClient;
        extendedClient = (ExtendedClient) null;
        service = foundClient.ToService(foundExtendedClient);
      }
      return service;
    }

    public async Task<IList<IdentityServer4.Admin.Logic.Entities.Services.Client>> Get(string name = null)
    {
      
      
      ClientService.\u003C\u003Ec__DisplayClass10_0 cDisplayClass100 = new ClientService.\u003C\u003Ec__DisplayClass10_0();
      
      cDisplayClass100.\u003C\u003E4__this = this;
      
      cDisplayClass100.name = name;
      IList<IdentityServer4.Admin.Logic.Entities.Services.Client> list;
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        
        
        ClientService.\u003C\u003Ec__DisplayClass10_1 cDisplayClass101 = new ClientService.\u003C\u003Ec__DisplayClass10_1();
        IEnumerable<IdentityServer4.EntityFramework.Entities.Client> resources;
        
        if (!string.IsNullOrWhiteSpace(cDisplayClass100.name))
        {
          
          IEnumerable<ExtendedClient> extendedClients = await uow.ExtendedClientRepository.Find((Expression<Func<ExtendedClient, bool>>) (x => x.NormalizedClientName.Contains(this.normalizer.Normalize(cDisplayClass100.name))));
          
          cDisplayClass101.extendedClients = extendedClients;
          extendedClients = (IEnumerable<ExtendedClient>) null;
          ParameterExpression parameterExpression1;
          ParameterExpression parameterExpression2;
          
          
          
          IEnumerable<IdentityServer4.EntityFramework.Entities.Client> clients = await uow.ClientRepository.Find(Expression.Lambda<Func<IdentityServer4.EntityFramework.Entities.Client, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new Expression[2]
          {
            cDisplayClass101.extendedClients,
            (Expression) Expression.Lambda<Func<ExtendedClient, bool>>((Expression) Expression.Equal(y.ClientId, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (IdentityServer4.EntityFramework.Entities.Client.get_ClientId)))), new ParameterExpression[1]
            {
              parameterExpression2
            })
          }), new ParameterExpression[1]
          {
            parameterExpression1
          }));
          resources = clients;
          clients = (IEnumerable<IdentityServer4.EntityFramework.Entities.Client>) null;
        }
        else
        {
          IEnumerable<ExtendedClient> extendedClients = await uow.ExtendedClientRepository.GetAll();
          
          cDisplayClass101.extendedClients = extendedClients;
          extendedClients = (IEnumerable<ExtendedClient>) null;
          IEnumerable<IdentityServer4.EntityFramework.Entities.Client> clients = await uow.ClientRepository.GetAll();
          resources = clients;
          clients = (IEnumerable<IdentityServer4.EntityFramework.Entities.Client>) null;
        }
        
        resources = resources.Where<IdentityServer4.EntityFramework.Entities.Client>(new Func<IdentityServer4.EntityFramework.Entities.Client, bool>(cDisplayClass101.\u003CGet\u003Eb__2));
        
        list = (IList<IdentityServer4.Admin.Logic.Entities.Services.Client>) resources.Select<IdentityServer4.EntityFramework.Entities.Client, IdentityServer4.Admin.Logic.Entities.Services.Client>(new Func<IdentityServer4.EntityFramework.Entities.Client, IdentityServer4.Admin.Logic.Entities.Services.Client>(cDisplayClass101.\u003CGet\u003Eb__3)).ToList<IdentityServer4.Admin.Logic.Entities.Services.Client>();
      }
      return list;
    }

    public async Task<IdentityResult> Update(IdentityServer4.Admin.Logic.Entities.Services.Client client)
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        IdentityServer4.EntityFramework.Entities.Client client1 = await uow.ClientRepository.GetByKey(client.ClientId);
        IdentityServer4.EntityFramework.Entities.Client existingClient = client1;
        client1 = (IdentityServer4.EntityFramework.Entities.Client) null;
        ExtendedClient extendedClient1 = await uow.ExtendedClientRepository.GetByClientId(client.ClientId);
        ExtendedClient existingExtendedClient = extendedClient1;
        extendedClient1 = (ExtendedClient) null;
        if (existingClient == null || existingExtendedClient == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + client.ClientId + " does not exist"
          });
        if (client.NonEditable)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Cannot update reserved client"
          });
        IdentityResult identityResult1 = await this.clientValidator.Validate(client);
        IdentityResult validationResult = identityResult1;
        identityResult1 = (IdentityResult) null;
        if (!validationResult.Succeeded)
          return validationResult;
        if (this.normalizer.Normalize(client.ClientName) != this.normalizer.Normalize(existingClient.ClientName))
        {
          ExtendedClient extendedClient2 = await uow.ExtendedClientRepository.GetByNormalizedClientName(this.normalizer.Normalize(client.ClientName));
          ExtendedClient foundExtendedClient = extendedClient2;
          extendedClient2 = (ExtendedClient) null;
          if (foundExtendedClient != null)
            return IdentityResult.Failed(new IdentityError()
            {
              Description = "Cannot update a client with client name " + client.ClientName + " already exists"
            });
          foundExtendedClient = (ExtendedClient) null;
        }
        IdentityServer4.EntityFramework.Entities.Client mappedClient = client.ToDomain();
        existingClient.ClientName = mappedClient.ClientName;
        existingExtendedClient.NormalizedClientName = this.normalizer.Normalize(mappedClient.ClientName);
        existingExtendedClient.Description = client.Description;
        existingClient.AlwaysIncludeUserClaimsInIdToken = mappedClient.AlwaysIncludeUserClaimsInIdToken;
        existingClient.AbsoluteRefreshTokenLifetime = mappedClient.AbsoluteRefreshTokenLifetime;
        existingClient.AccessTokenLifetime = mappedClient.AccessTokenLifetime;
        existingClient.AccessTokenType = mappedClient.AccessTokenType;
        existingClient.AllowAccessTokensViaBrowser = mappedClient.AllowAccessTokensViaBrowser;
        existingClient.AllowOfflineAccess = mappedClient.AllowOfflineAccess;
        existingClient.AllowPlainTextPkce = mappedClient.AllowPlainTextPkce;
        existingClient.AllowRememberConsent = mappedClient.AllowRememberConsent;
        existingClient.AlwaysSendClientClaims = mappedClient.AlwaysSendClientClaims;
        existingClient.AuthorizationCodeLifetime = mappedClient.AuthorizationCodeLifetime;
        existingClient.ClientClaimsPrefix = mappedClient.ClientClaimsPrefix;
        existingClient.ClientUri = mappedClient.ClientUri;
        existingClient.EnableLocalLogin = mappedClient.EnableLocalLogin;
        existingClient.Enabled = mappedClient.Enabled;
        existingClient.FrontChannelLogoutSessionRequired = mappedClient.FrontChannelLogoutSessionRequired;
        existingClient.FrontChannelLogoutUri = mappedClient.FrontChannelLogoutUri;
        existingClient.IdentityTokenLifetime = mappedClient.IdentityTokenLifetime;
        existingClient.IncludeJwtId = mappedClient.IncludeJwtId;
        existingClient.LogoUri = mappedClient.LogoUri;
        existingClient.ProtocolType = mappedClient.ProtocolType;
        existingClient.RefreshTokenExpiration = mappedClient.RefreshTokenExpiration;
        existingClient.RefreshTokenUsage = mappedClient.RefreshTokenUsage;
        existingClient.RequireClientSecret = mappedClient.RequireClientSecret;
        existingClient.RequireConsent = mappedClient.RequireConsent;
        existingClient.RequirePkce = mappedClient.RequirePkce;
        existingClient.SlidingRefreshTokenLifetime = mappedClient.SlidingRefreshTokenLifetime;
        existingClient.UpdateAccessTokenClaimsOnRefresh = mappedClient.UpdateAccessTokenClaimsOnRefresh;
        existingClient.BackChannelLogoutUri = mappedClient.BackChannelLogoutUri;
        existingClient.BackChannelLogoutSessionRequired = mappedClient.BackChannelLogoutSessionRequired;
        existingClient.Description = mappedClient.Description;
        existingClient.PairWiseSubjectSalt = mappedClient.PairWiseSubjectSalt;
        existingClient.ConsentLifetime = mappedClient.ConsentLifetime;
        existingClient.UserSsoLifetime = mappedClient.UserSsoLifetime;
        existingClient.Updated = mappedClient.Updated;
        existingClient.LastAccessed = mappedClient.LastAccessed;
        existingClient.DeviceCodeLifetime = mappedClient.DeviceCodeLifetime;
        existingClient.UserCodeType = mappedClient.UserCodeType;
        foreach (ClientGrantType clientGrantType in mappedClient.AllowedGrantTypes.Where<ClientGrantType>((Func<ClientGrantType, bool>) (x => existingClient.AllowedGrantTypes.All<ClientGrantType>((Func<ClientGrantType, bool>) (y => x.GrantType != y.GrantType)))))
        {
          ClientGrantType grantToAdd = clientGrantType;
          existingClient.AllowedGrantTypes.Add(new ClientGrantType()
          {
            GrantType = grantToAdd.GrantType
          });
          grantToAdd = (ClientGrantType) null;
        }
        foreach (ClientGrantType clientGrantType in existingClient.AllowedGrantTypes.Where<ClientGrantType>((Func<ClientGrantType, bool>) (x => mappedClient.AllowedGrantTypes.All<ClientGrantType>((Func<ClientGrantType, bool>) (y => x.GrantType != y.GrantType)))).ToList<ClientGrantType>())
        {
          ClientGrantType grantToRemove = clientGrantType;
          existingClient.AllowedGrantTypes.Remove(grantToRemove);
          grantToRemove = (ClientGrantType) null;
        }
        foreach (ClientRedirectUri clientRedirectUri in mappedClient.RedirectUris.Where<ClientRedirectUri>((Func<ClientRedirectUri, bool>) (x => existingClient.RedirectUris.All<ClientRedirectUri>((Func<ClientRedirectUri, bool>) (y => x.RedirectUri != y.RedirectUri)))))
        {
          ClientRedirectUri uriToAdd = clientRedirectUri;
          existingClient.RedirectUris.Add(new ClientRedirectUri()
          {
            RedirectUri = uriToAdd.RedirectUri
          });
          uriToAdd = (ClientRedirectUri) null;
        }
        foreach (ClientRedirectUri clientRedirectUri in existingClient.RedirectUris.Where<ClientRedirectUri>((Func<ClientRedirectUri, bool>) (x => mappedClient.RedirectUris.All<ClientRedirectUri>((Func<ClientRedirectUri, bool>) (y => x.RedirectUri != y.RedirectUri)))).ToList<ClientRedirectUri>())
        {
          ClientRedirectUri uriToRemove = clientRedirectUri;
          existingClient.RedirectUris.Remove(uriToRemove);
          uriToRemove = (ClientRedirectUri) null;
        }
        foreach (ClientPostLogoutRedirectUri logoutRedirectUri in mappedClient.PostLogoutRedirectUris.Where<ClientPostLogoutRedirectUri>((Func<ClientPostLogoutRedirectUri, bool>) (x => existingClient.PostLogoutRedirectUris.All<ClientPostLogoutRedirectUri>((Func<ClientPostLogoutRedirectUri, bool>) (y => x.PostLogoutRedirectUri != y.PostLogoutRedirectUri)))))
        {
          ClientPostLogoutRedirectUri uriToAdd = logoutRedirectUri;
          existingClient.PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri()
          {
            PostLogoutRedirectUri = uriToAdd.PostLogoutRedirectUri
          });
          uriToAdd = (ClientPostLogoutRedirectUri) null;
        }
        foreach (ClientPostLogoutRedirectUri logoutRedirectUri in existingClient.PostLogoutRedirectUris.Where<ClientPostLogoutRedirectUri>((Func<ClientPostLogoutRedirectUri, bool>) (x => mappedClient.PostLogoutRedirectUris.All<ClientPostLogoutRedirectUri>((Func<ClientPostLogoutRedirectUri, bool>) (y => x.PostLogoutRedirectUri != y.PostLogoutRedirectUri)))).ToList<ClientPostLogoutRedirectUri>())
        {
          ClientPostLogoutRedirectUri uriToRemove = logoutRedirectUri;
          existingClient.PostLogoutRedirectUris.Remove(uriToRemove);
          uriToRemove = (ClientPostLogoutRedirectUri) null;
        }
        foreach (ClientScope clientScope in mappedClient.AllowedScopes.Where<ClientScope>((Func<ClientScope, bool>) (x => existingClient.AllowedScopes.All<ClientScope>((Func<ClientScope, bool>) (y => x.Scope != y.Scope)))))
        {
          ClientScope scopeToAdd = clientScope;
          existingClient.AllowedScopes.Add(new ClientScope()
          {
            Scope = scopeToAdd.Scope
          });
          scopeToAdd = (ClientScope) null;
        }
        foreach (ClientScope clientScope in existingClient.AllowedScopes.Where<ClientScope>((Func<ClientScope, bool>) (x => mappedClient.AllowedScopes.All<ClientScope>((Func<ClientScope, bool>) (y => x.Scope != y.Scope)))).ToList<ClientScope>())
        {
          ClientScope scopeToRemove = clientScope;
          existingClient.AllowedScopes.Remove(scopeToRemove);
          scopeToRemove = (ClientScope) null;
        }
        foreach (ClientCorsOrigin clientCorsOrigin in mappedClient.AllowedCorsOrigins.Where<ClientCorsOrigin>((Func<ClientCorsOrigin, bool>) (x => existingClient.AllowedCorsOrigins.All<ClientCorsOrigin>((Func<ClientCorsOrigin, bool>) (y => x.Origin != y.Origin)))))
        {
          ClientCorsOrigin originToAdd = clientCorsOrigin;
          existingClient.AllowedCorsOrigins.Add(new ClientCorsOrigin()
          {
            Origin = originToAdd.Origin
          });
          originToAdd = (ClientCorsOrigin) null;
        }
        foreach (ClientCorsOrigin clientCorsOrigin in existingClient.AllowedCorsOrigins.Where<ClientCorsOrigin>((Func<ClientCorsOrigin, bool>) (x => mappedClient.AllowedCorsOrigins.All<ClientCorsOrigin>((Func<ClientCorsOrigin, bool>) (y => x.Origin != y.Origin)))).ToList<ClientCorsOrigin>())
        {
          ClientCorsOrigin originToRemove = clientCorsOrigin;
          existingClient.AllowedCorsOrigins.Remove(originToRemove);
          originToRemove = (ClientCorsOrigin) null;
        }
        foreach (ClientIdPRestriction clientIdPrestriction in mappedClient.IdentityProviderRestrictions.Where<ClientIdPRestriction>((Func<ClientIdPRestriction, bool>) (x => existingClient.IdentityProviderRestrictions.All<ClientIdPRestriction>((Func<ClientIdPRestriction, bool>) (y => x.Provider != y.Provider)))))
        {
          ClientIdPRestriction providerToAdd = clientIdPrestriction;
          existingClient.IdentityProviderRestrictions.Add(new ClientIdPRestriction()
          {
            Provider = providerToAdd.Provider
          });
          providerToAdd = (ClientIdPRestriction) null;
        }
        foreach (ClientIdPRestriction clientIdPrestriction in existingClient.IdentityProviderRestrictions.Where<ClientIdPRestriction>((Func<ClientIdPRestriction, bool>) (x => mappedClient.IdentityProviderRestrictions.All<ClientIdPRestriction>((Func<ClientIdPRestriction, bool>) (y => x.Provider != y.Provider)))).ToList<ClientIdPRestriction>())
        {
          ClientIdPRestriction providerToRemove = clientIdPrestriction;
          existingClient.IdentityProviderRestrictions.Remove(providerToRemove);
          providerToRemove = (ClientIdPRestriction) null;
        }
        foreach (ClientClaim clientClaim in mappedClient.Claims.Where<ClientClaim>((Func<ClientClaim, bool>) (x => existingClient.Claims.All<ClientClaim>((Func<ClientClaim, bool>) (y => new
        {
          Type = x.Type,
          Value = x.Value
        }.GetHashCode() != new
        {
          Type = y.Type,
          Value = y.Value
        }.GetHashCode())))))
        {
          ClientClaim claimToAdd = clientClaim;
          existingClient.Claims.Add(new ClientClaim()
          {
            Type = claimToAdd.Type,
            Value = claimToAdd.Value
          });
          claimToAdd = (ClientClaim) null;
        }
        foreach (ClientClaim clientClaim in existingClient.Claims.Where<ClientClaim>((Func<ClientClaim, bool>) (x => mappedClient.Claims.All<ClientClaim>((Func<ClientClaim, bool>) (y => new
        {
          Type = x.Type,
          Value = x.Value
        }.GetHashCode() != new
        {
          Type = y.Type,
          Value = y.Value
        }.GetHashCode())))).ToList<ClientClaim>())
        {
          ClientClaim claimToRemove = clientClaim;
          existingClient.Claims.Remove(claimToRemove);
          claimToRemove = (ClientClaim) null;
        }
        foreach (ClientProperty clientProperty1 in mappedClient.Properties.Where<ClientProperty>((Func<ClientProperty, bool>) (x => existingClient.Properties.All<ClientProperty>((Func<ClientProperty, bool>) (y => x.Key != y.Key)))))
        {
          ClientProperty propertyToAdd = clientProperty1;
          List<ClientProperty> properties = existingClient.Properties;
          ClientProperty clientProperty2 = new ClientProperty();
          clientProperty2.Key = propertyToAdd.Key;
          clientProperty2.Value = propertyToAdd.Value;
          properties.Add(clientProperty2);
          propertyToAdd = (ClientProperty) null;
        }
        foreach (ClientProperty clientProperty in existingClient.Properties.Where<ClientProperty>((Func<ClientProperty, bool>) (x => mappedClient.Properties.All<ClientProperty>((Func<ClientProperty, bool>) (y => x.Key != y.Key)))).ToList<ClientProperty>())
        {
          ClientProperty propertyToRemove = clientProperty;
          existingClient.Properties.Remove(propertyToRemove);
          propertyToRemove = (ClientProperty) null;
        }
        IdentityResult identityResult2 = await uow.ClientRepository.Update(existingClient);
        IdentityResult result = identityResult2;
        identityResult2 = (IdentityResult) null;
        if (result.Succeeded)
        {
          IdentityResult identityResult3 = await uow.ExtendedClientRepository.Update(existingExtendedClient);
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

    public async Task<IdentityResult> AddSecret(string id, PlainTextSecret secret)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof (id));
      if (secret == null)
        throw new ArgumentNullException(nameof (secret));
      using (IIdentityServerUnitOfWork uow = this.factory.Create())
      {
        ExtendedClient extendedClient1 = await uow.ExtendedClientRepository.GetByKey(id);
        ExtendedClient extendedClient2 = extendedClient1;
        extendedClient1 = (ExtendedClient) null;
        if (extendedClient2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.Client client = await uow.ClientRepository.GetByKey(extendedClient2.ClientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client;
        client = (IdentityServer4.EntityFramework.Entities.Client) null;
        if (foundClient == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        foundClient.ClientSecrets.Add(secret.ToClientSecret());
        IdentityResult identityResult1 = await uow.ClientRepository.Update(foundClient);
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
        ExtendedClient extendedClient1 = await uow.ExtendedClientRepository.GetByKey(id);
        ExtendedClient extendedClient2 = extendedClient1;
        extendedClient1 = (ExtendedClient) null;
        if (extendedClient2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.Client client = await uow.ClientRepository.GetByKey(extendedClient2.ClientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client;
        client = (IdentityServer4.EntityFramework.Entities.Client) null;
        if (foundClient == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        ClientSecret foundSecret = foundClient.ClientSecrets.FirstOrDefault<ClientSecret>((Func<ClientSecret, bool>) (x => x.Id == secret.Id));
        if (foundSecret == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = string.Format("Secret with id {0} does not exist", (object) secret.Id)
          });
        ClientSecret mappedSecret = secret.ToClientSecret();
        foundSecret.Type = mappedSecret.Type;
        foundSecret.Description = mappedSecret.Description;
        foundSecret.Expiration = mappedSecret.Expiration;
        if ((uint) string.Compare(mappedSecret.Type, "SharedSecret", StringComparison.CurrentCultureIgnoreCase) > 0U)
          foundSecret.Value = mappedSecret.Value;
        IdentityResult identityResult1 = await uow.ClientRepository.Update(foundClient);
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
        ExtendedClient extendedClient1 = await uow.ExtendedClientRepository.GetByKey(id);
        ExtendedClient extendedClient2 = extendedClient1;
        extendedClient1 = (ExtendedClient) null;
        if (extendedClient2 == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        IdentityServer4.EntityFramework.Entities.Client client = await uow.ClientRepository.GetByKey(extendedClient2.ClientId);
        IdentityServer4.EntityFramework.Entities.Client foundClient = client;
        client = (IdentityServer4.EntityFramework.Entities.Client) null;
        if (foundClient == null)
          return IdentityResult.Failed(new IdentityError()
          {
            Description = "Client with id " + id + " does not exist"
          });
        ClientSecret foundSecret = foundClient.ClientSecrets.FirstOrDefault<ClientSecret>((Func<ClientSecret, bool>) (x => x.Id == secret.Id));
        if (foundSecret == null)
          return IdentityResult.Success;
        foundClient.ClientSecrets.Remove(foundSecret);
        IdentityResult identityResult1 = await uow.ClientRepository.Update(foundClient);
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
