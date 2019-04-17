using AspNetCore.Identity.XCode;
using Easy.Admin.Authentication;
using Easy.Admin.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewLife.IdentityServer4.Models;

namespace NewLife.IdentityServer4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).ConfigJsonOptions();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginReturnUrlParameter = "redirect";//返回url的参数名

                    options.UserInteraction.LoginUrl = "/login";

                    options.Authentication.CookieAuthenticationScheme = "Jwt-Cookie";
                })
                .AddXCodeConfigurationStore()
                .AddXCodeOperationalStore(options =>
                {
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
                })
                .AddDeveloperSigningCredential()
                //.AddJwtBearerClientAuthentication()
                ;

            services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddXCodeStores()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            // 身份验证
            services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = JwtBearerAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerAuthenticationDefaults.AuthenticationScheme;
                })
                // SignManager内部使用IdentityConstants.ApplicationScheme作为登陆方案名称
                .AddJwtBearerSignIn(IdentityConstants.ApplicationScheme)
                // IdentityServer内部Cookie登陆方案名称，避免跟正常使用的JwtBearerSignIn方案名称一致，
                // TODO 一样的话将会验证多一些声明，具体未详细记录
                .AddJwtBearerSignIn("Jwt-Cookie");

            services.AddCors();

            services.AddLogging(options =>
            {
                options.AddConsole();
                options.AddDebug();
            });

            // 添加EasyAdmin
            services.AddEasyAdmin();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseApiExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseExceptionHandler("/Home/Error");

            app.UseHttpsRedirection();

            app.UseCors(options => { options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials(); });

            app.UseIdentityServer();

            app.UseMvc();

            app.UseEasyAdmin();
        }
    }
}
