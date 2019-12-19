using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NewLife.IdentityServer4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(serverOptions =>
                        {
                            // Set properties and call methods on options
                            serverOptions.AllowSynchronousIO = true; // 允许同步IO

                            // 上述设置还可用以下方式替代，在需要读取Body的地方设置即可
                            //var ft = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpBodyControlFeature>();
                            //if (ft != null) ft.AllowSynchronousIO = true;
                        })
                        //.UseIIS()
                        //.UseIISIntegration()
                        .UseStartup<Startup>();
                });
    }
}
