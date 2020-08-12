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
                .ConfigureAppConfiguration((context, configBinder) =>
                {
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var fileInfo = new FileInfo(baseDirectory + "Config/appsettings.json");
                    if (!fileInfo.Exists)
                    {
                        if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }
                        File.Copy(baseDirectory + "appsettings.json", fileInfo.FullName);
                    }

                    configBinder.AddJsonFile(fileInfo.FullName);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
