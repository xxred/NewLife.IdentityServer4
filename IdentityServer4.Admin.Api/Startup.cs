using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Admin.Logic.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityServer4.Admin.Api
{
    public class Startup : IStartup
    {
        private readonly IConfigurationRoot configuration;
        internal Startup()
        {
            this.configuration = new ConfigurationBuilder().AddInMemoryCollection(
                (IEnumerable<KeyValuePair<string, string>>)new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("DbProvider", "Sqlite"),
                    new KeyValuePair<string, string>("IdentityConnectionString", "Data Source=AspIdUsers.db;"),
                    new KeyValuePair<string, string>("IdentityServerConnectionString", "Data Source=IdentityServer.db;"),
                    new KeyValuePair<string, string>("AuditRecordsConnectionString", "Data Source=IdentityServer.db;"),
                    new KeyValuePair<string, string>("AuthorityUrl", "http://localhost:5000"),
                    new KeyValuePair<string, string>("UiUrl", "http://localhost:5000"),
                    new KeyValuePair<string, string>("RequireHttpsMetadata", "false"),
                    new KeyValuePair<string, string>("RunIdentityServerMigrations", "true"),
                    new KeyValuePair<string, string>("RunIdentityMigrations", "true"),
                    new KeyValuePair<string, string>("LicenseKey", ""),
                    new KeyValuePair<string, string>("CER_FULL_PATH", "./Data/gateway.cer"),
                    new KeyValuePair<string, string>("UseLegacyPaging", "false"),
                    new KeyValuePair<string, string>("RegistrationConfirmationEndpoint", ""),
                    new KeyValuePair<string, string>("ClientId", ""),
                    new KeyValuePair<string, string>("ClientSecret", ""),
                    new KeyValuePair<string, string>("PasswordResetEndpoint", "")
                }).Build();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Microsoft.Extensions.Internal.ISystemClock, Microsoft.Extensions.Internal.SystemClock>();
            services.AddScoped<Microsoft.AspNetCore.Authentication.ISystemClock, Microsoft.AspNetCore.Authentication.SystemClock>();
            services.AddMemoryCache();
            services.AddOptions();

            services.Configure<WebhookOptions>((IConfiguration)this.configuration);

            services.AddSingleton<IConfiguration>((IConfiguration)this.configuration);

            //bool useLegacyPaging = this.configuration.GetValue<bool>("UseLegacyPaging", false);
            //services.AddIdentityExpressAdmin(str1, useLegacyPaging);
            //services.AddIdentityCore<IdentityExpressUser>((Action<IdentityOptions>)(options =>
            //{
            //    options.Password.RequireDigit = this.configuration.GetValue<bool>("PasswordPolicy:RequireDigit", true);
            //    options.Password.RequireLowercase = this.configuration.GetValue<bool>("PasswordPolicy:RequireLowercase", true);
            //    options.Password.RequireNonAlphanumeric = this.configuration.GetValue<bool>("PasswordPolicy:RequireNonAlphanumeric", true);
            //    options.Password.RequireUppercase = this.configuration.GetValue<bool>("PasswordPolicy:RequireUppercase", true);
            //    options.Password.RequiredLength = this.configuration.GetValue<int>("PasswordPolicy:RequiredLength", 6);
            //    options.Password.RequiredUniqueChars = this.configuration.GetValue<int>("PasswordPolicy:RequiredUniqueChars", 1);
            //    options.User.AllowedUserNameCharacters = this.configuration.GetValue<string>("UsernamePolicy:AllowedUserNameCharacters", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+");
            //    options.User.RequireUniqueEmail = this.configuration.GetValue<bool>("UsernamePolicy:RequireUniqueEmail", true);
            //})).AddDefaultTokenProviders().AddRoles<IdentityExpressRole>().AddRoleValidator<RoleValidator<IdentityExpressRole>>();
            //services.AddScoped<ISecurityStampValidator, SecurityStampValidator<IdentityExpressUser>>();
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSwaggerGen((Action<SwaggerGenOptions>)(x => x.SwaggerDoc("v1", new Info()
            //{
            //    Title = "IdentityExpress Management API",
            //    Version = "v1"
            //})));
            //services.AddMvc((Action<MvcOptions>)(options =>
            //{
            //    options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse((StringSegment)"text/csv"));
            //    options.OutputFormatters.Insert(0, (IOutputFormatter)new CsvOutputFormatter());
            //}));
            //this.ConfigureAdditionalServices(services);
            //services.AddScoped<IAdminUIConfigurationRepository, AdminUIConfigurationRepository>();
            //services.AddScoped<ISerializer<AccessPolicyDefinition>, JsonSerializer<AccessPolicyDefinition>>();
            //services.AddTransient<AccessPolicyStore>();
            //services.AddTransient<IProvideClaimsToPermissionsPolicy>((Func<IServiceProvider, IProvideClaimsToPermissionsPolicy>)(s => (IProvideClaimsToPermissionsPolicy)s.GetService<AccessPolicyStore>()));
            //services.AddTransient<IAccessPolicyStore>((Func<IServiceProvider, IAccessPolicyStore>)(s => (IAccessPolicyStore)s.GetService<AccessPolicyStore>()));
            //this.ConfiugureAudit(services);
            //services.AddSingleton<IObjectPropertyComparator, ReflectionObjectPropertyComparator>();
            //services.AddIdentityDatabase(str1, connectionString2, useLegacyPaging).AddIdentityExpressDatabases(str1, str2, useLegacyPaging).AddSupportedDatabaseProviders();
            //if (!string.IsNullOrWhiteSpace(connectionString1))
            //    services.AddBaseIdentityDatabase(str1, connectionString1, useLegacyPaging);
            //services.AddCors();
            //services.AddResponseCompression();
            //this.ConfigureAuditService(services, str2, useLegacyPaging);
            services.AddMvc();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}

