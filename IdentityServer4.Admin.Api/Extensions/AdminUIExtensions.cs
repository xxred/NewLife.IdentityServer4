using AspNetCore.Identity.XCode;
using Extensions.Identity.Stores.XCode;
using IdentityServer4.Admin.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using IdentityUser = Extensions.Identity.Stores.XCode.IdentityUser;

namespace IdentityServer4
{
    public static class AdminUIExtensions
    {
        public static IApplicationBuilder UseAdminUI(this IApplicationBuilder appBuilder)
        {
            var configuration = appBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
            Startup startup = new Startup(configuration);
            IServiceProvider property = (IServiceProvider)appBuilder.Properties["application.Services"];
            startup.Configure(appBuilder);
            return appBuilder;
        }

        public static IServiceCollection UseAdminUI(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            new Startup(configuration).ConfigureServices(services);
            services.TryAddScoped<SignInManager<IdentityUser>>();
            services.TryAddScoped<IUserStore<IdentityUser>, UserStore>();
            return services;
        }
    }
}