using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Mappers;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using IdentityServer4.Admin.Logic.Interfaces.Validators;
using IdentityServer4.Stores.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityExpressExtensions
    {
        public static IServiceCollection AddIdentityAdmin(this IServiceCollection services, string dbProvider, bool useLegacyPaging)
        {
            //services.AddScoped<ICertificateStore, EmbeddedCertificateStore>();
            //services.AddScoped<ILicenseHandler, LicenseHandler>();
            //services.AddScoped<ILicenseValidationService, CommunityLicenseValidationService>();
            //services.AddScoped<IIdentityUnitOfWorkFactory, IdentityUnitOfWorkFactory>();
            //services.AddScoped<IRoleManagerFactory, IdentityExpressRoleManagerFactory>();
            //services.AddScoped<IUserManagerFactory, IdentityExpressUserManagerFactory>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IClaimTypeService, ClaimTypeService>();
            //services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            //services.AddScoped<IUserClaimsService, UserClaimsService>();
            //services.AddScoped<IIdentityServerUnitOfWorkFactory, IdentityServerUnitOfWorkFactory>();
            //services.AddScoped<IMapper<GenericClient, Client>, GenericClientMapper>();
            //services.AddScoped<IClientService, ClientService>();
            //services.AddScoped<IApiResourceService, ApiResourceService>();
            //services.AddScoped<IIdentityResourceService, IdentityResourceService>();
            //services.AddScoped<IGrantService, GrantService>();
            //services.AddScoped<IValidator<Client>, ClientValidator>();
            //services.AddScoped<IValidator<ApiResource>, ApiResourceValidator>();
            //services.AddScoped<IValidator<IdentityResource>, IdentityResourceValidator>();
            //services.AddScoped<IValidator<IList<string>>, ClaimsValidator>();
            //services.AddScoped<IPersistentGrantSerializer, PersistentGrantSerializer>();
            //services.AddScoped<IWebhookService, WebhookService>();
            //IUserQueryFactory implementationInstance = !useLegacyPaging ? (IUserQueryFactory)new LegacyUserQueryFactory(dbProvider) : (IUserQueryFactory)new UserQueryFactory(dbProvider);
            //services.AddSingleton<IUserQueryFactory>(implementationInstance);
            return services;
        }

        public static IServiceCollection AddIdentityExpressDatabases(this IServiceCollection services, string provider, string connectionString, bool useLegacyPaging)
        {
            //services.AddSingleton<ConfigurationStoreOptions>();
            //services.AddSingleton<OperationalStoreOptions>();
            return services;
        }
    }
}
