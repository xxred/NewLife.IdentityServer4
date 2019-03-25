using IdentityServer4.Admin.Logic.Entities.Configuration;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Policy
{
    public class AccessPolicyStore : IAccessPolicyStore, IProvideClaimsToPermissionsPolicy
    {
        private readonly IAdminUIConfigurationRepository configurationRepository;

        private readonly IApiResourceService apiResourceService;

        private readonly ISerializer<AccessPolicyDefinition> serializer;

        private readonly ISystemClock systemClock;

        private Lazy<Task<AccessPolicyDefinition>> accessPolicy;

        private static string PolicyKey = "policy";

        public PolicyClaim DefaultPolicy
        {
            get
            {
                //IL_0000: Unknown result type (might be due to invalid IL or missing references)
                //IL_0005: Unknown result type (might be due to invalid IL or missing references)
                //IL_0011: Unknown result type (might be due to invalid IL or missing references)
                //IL_001d: Unknown result type (might be due to invalid IL or missing references)
                //IL_0026: Expected O, but got Unknown
                PolicyClaim val = new PolicyClaim();
                val.Type = ("role");
                val.Value = ("AdminUI Administrator");
                val.Permission = Permission.All;
                return val;
            }
        }

        public AccessPolicyStore(IAdminUIConfigurationRepository configurationRepository, ISerializer<AccessPolicyDefinition> serializer, ISystemClock systemClock, IApiResourceService apiResourceService)
        {
            if (configurationRepository == null)
            {
                throw new ArgumentNullException("configurationRepository");
            }
            this.configurationRepository = configurationRepository;
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }
            this.serializer = serializer;
            if (systemClock == null)
            {
                throw new ArgumentNullException("systemClock");
            }
            this.systemClock = systemClock;
            if (apiResourceService == null)
            {
                throw new ArgumentNullException("apiResourceService");
            }
            this.apiResourceService = apiResourceService;
            accessPolicy = new Lazy<Task<AccessPolicyDefinition>>(LoadAccessPolicyConfiguration);
        }

        private async Task<AccessPolicyDefinition> LoadAccessPolicyConfiguration()
        {
            string accessPolicyString = await configurationRepository.TryGetValue(PolicyKey, null);
            if (accessPolicyString == null)
            {
                throw new AccessPolicyStoreException("No access policy configuration found");
            }
            AccessPolicyDefinition currentAccessPolicy = serializer.Deserialize(accessPolicyString);
            currentAccessPolicy.PolicyClaims.Add(DefaultPolicy);
            return currentAccessPolicy;
        }

        public async Task<AccessPolicyDefinition> GetAccessPolicyDefinition()
        {
            return await accessPolicy.Value;
        }

        public async Task<AccessPolicyDefinition> UpdateAccessPolicy(AccessPolicyDefinition updatedAccessPolicy)
        {
            if (updatedAccessPolicy == null)
            {
                throw new ArgumentNullException("updatedAccessPolicy");
            }
            updatedAccessPolicy.PolicyClaims.RemoveAll((PolicyClaim x) => x.Type == DefaultPolicy.Type && x.Value == DefaultPolicy.Value && x.Permission == DefaultPolicy.Permission);
            AccessPolicyDefinition currentAccessPolicy = await GetAccessPolicyDefinition();
            if (updatedAccessPolicy.Version != currentAccessPolicy.Version)
            {
                throw new PolicyConcurrencyException();
            }
            updatedAccessPolicy.Version=(systemClock.UtcNow.ToString());
            string serializedAccessPolicy = serializer.Serialize(updatedAccessPolicy);
            await configurationRepository.SetValue(PolicyKey, serializedAccessPolicy);
            await UpdateAdminUIScopeClaims((from x in updatedAccessPolicy.PolicyClaims
                                            select x.Type).Distinct());
            updatedAccessPolicy.PolicyClaims.Add(DefaultPolicy);
            accessPolicy = new Lazy<Task<AccessPolicyDefinition>>(() => Task.FromResult<AccessPolicyDefinition>(updatedAccessPolicy));
            return updatedAccessPolicy;
        }

        private async Task UpdateAdminUIScopeClaims(IEnumerable<string> claimTypes)
        {
            string adminUIApiName = "admin_api";
            List<string> claimTypesList2 = claimTypes.ToList();
            claimTypesList2.Add("role");
            claimTypesList2 = claimTypesList2.Distinct().ToList();
            ApiResource adminUIApiResource = await apiResourceService.GetByApiResourceName(adminUIApiName);
            if (adminUIApiResource == null)
            {
                throw new AccessPolicyStoreException(adminUIApiName + " api resource not found");
            }
            Scope adminUIApiScope = adminUIApiResource.Scopes.FirstOrDefault((Scope x) => x.Name == adminUIApiName);
            if (adminUIApiScope == null)
            {
                throw new AccessPolicyStoreException(adminUIApiName + " scope not found");
            }
            adminUIApiScope.UserClaims=((ICollection<string>)claimTypesList2);
            await apiResourceService.UpdateScope(adminUIApiResource.Id, adminUIApiScope);
        }
    }
}
