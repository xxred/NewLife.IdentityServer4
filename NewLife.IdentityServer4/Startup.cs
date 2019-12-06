using System;
using Easy.Admin.Areas.Admin.Controllers;
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
using NewLife.IdentityServer4.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

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

            //app.UseHttpsRedirection();

            app.UseExceptionHandler("/Home/Error");

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

            var fileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "dist"));
            var staticFileOptions = new StaticFileOptions()
            {
                FileProvider = fileProvider
            };

            //app.UseDefaultFiles(new DefaultFilesOptions()
            //{
            //    FileProvider = fileProvider
            //});

            app.UseStaticFiles(staticFileOptions);

            app.UseRouting();

            //跨域
            app.UseCors("default");

            //身份认证
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(options => { options.MapControllers(); });

            //以下为SPA准备，这里是
            if (env.WebRootPath != null)
            {
                app.UseSpa(options =>
                {
                    // options.Options.SourcePath = "ClientApp"; 前端项目在ClientApp文件夹

                    options.Options.DefaultPageStaticFileOptions =
                        staticFileOptions;
                    // options.UseProxyToSpaDevelopmentServer("http://127.0.0.1:1337/"); // 转发请求到前端项目
                });
            }
        }
    }
}
