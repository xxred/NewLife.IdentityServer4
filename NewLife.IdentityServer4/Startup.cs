using System;
using Easy.Admin.Configuration;
using Easy.Admin.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Easy.Admin.SpaServices;
using NewLife.IdentityServer4.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Extensions;
using NewLife.IdentityServer4.Services;

namespace NewLife.IdentityServer4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // 清空所有ClaimType映射，不进行任何转换
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加EasyAdmin
            services.AddEasyAdmin();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginReturnUrlParameter = "returnUrl";//返回url的参数名

                    options.UserInteraction.LoginUrl = "/login";

                    options.Authentication.CookieAuthenticationScheme =
                        IdentityConstants.ApplicationScheme;
                    //"Jwt-Cookie";
                })
                .AddXCodeConfigurationStore()
                .AddXCodeOperationalStore(options =>
                {
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
                })
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential() // 生成一个密钥对，并存储到tempkey.rsa
                                                 //.AddJwtBearerClientAuthentication()
                ;

            //// 身份验证
            //services.AddAuthentication()
            ////    // IdentityServer内部Cookie登陆方案名称，避免跟正常使用的JwtBearerSignIn方案名称一致，
            ////    // TODO 一样的话将会验证多一些声明，具体未详细记录
            //    .AddJwtBearerSignIn("Jwt-Cookie", options =>
            //{
            //    // 获得跟EasyAdmin同样的配置
            //    AuthenticationConfig.ConfigureJwtBearerOptions(options, Configuration);
            //});

            services.AddLogging(options =>
            {
                options.AddConsole();
                options.AddDebug();
            });

            // 扫描控制器
            services.ScanController(options =>
            {
                var type = typeof(ClientsController);
                options.Description = "授权中心";
                options.DisplayName = "授权中心";
                options.Namespace = type.Namespace;
                options.RootUrl = "api";
                options.ScanAssembly = type.Assembly;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 不使用EasyAdmin管道，为了插入 IdentityServer
            //app.UseAdminBase();

            app.Use(async (ctx, next) =>
            {
                // 如果是代理，设置重新设置Scheme
                if (ctx.Request.Headers.ContainsKey("X-Forwarded-Proto"))
                {
                    ctx.Request.Scheme = ctx.Request.Headers["X-Forwarded-Proto"];
                }

                await next();
            });

            app.UseApiExceptionHandler();
            //IdentityServer4.Validation.AuthorizeRequestValidator
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["ApiTitle"] ?? "IdentityServer");

                var oAuthConfiguration = app.ApplicationServices.GetRequiredService<OAuthConfiguration>();
                if (!oAuthConfiguration.Authority.IsNullOrEmpty())
                {
                    c.OAuthClientId(oAuthConfiguration.ClientId);
                    c.OAuthClientSecret(oAuthConfiguration.ClientSecret);
                    //c.OAuthRealm("test-realm");
                    c.OAuthAppName(Configuration["ApiTitle"]);
                    //c.OAuthScopeSeparator(" ");
                    //c.OAuthAdditionalQueryStringParams(new { foo = "bar" });
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //跨域
            app.UseCors("default");

            //身份认证
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(options => { options.MapControllers(); });


            // 如果是开发环境，并且配置了前端源码路径，则启用前端开发中间件
            // 否则设置打包后的静态文件
            // ClientAppSourcePath配置优先，生产部署无需配置，因此，改配置放在appsettings.Development.json即可

            var clientAppSourcePath = Configuration["ClientAppSourcePath"];

            if (env.IsDevelopment() && !clientAppSourcePath.IsNullOrWhiteSpace())
            {
                app.UseSpa(options =>
                {
                    // 开发时，当前目录就是项目目录，而不是bin目录
                    options.Options.SourcePath = clientAppSourcePath;
                    //options.Options.StartupTimeout = TimeSpan.FromSeconds(20);
                    options.UseVueDevelopmentServer("yarn", "start");
                });
            }
            else if (env.WebRootPath != null)
            {
                // 如果dist文件夹不存在，说明没有部署前端文件
                var dist = Path.Combine(env.WebRootPath, "dist");
                if (!Directory.Exists(dist))
                {
                    return;
                }

                app.UseSpa(options =>
                {
                    var staticFileOptions = new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(dist)
                    };

                    app.UseSpaStaticFiles(staticFileOptions);
                    options.Options.DefaultPageStaticFileOptions = staticFileOptions;
                });
            }
        }
    }
}
