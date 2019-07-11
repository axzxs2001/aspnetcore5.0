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

namespace ConfigrationDemo_Custom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //创建Host
            CreateHostBuilder(args).Build().Run();
            //创建WebHost
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>//webBuilder是主机(host)配置构建对象
                {
                    webBuilder.ConfigureKestrel(option =>
                    {
                        option.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(20);
                    });
                    webBuilder.ConfigureAppConfiguration((context, config) => //config是应用(application)配置构建对象
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddYamlFile("appsettings.yaml");
                    });
                    webBuilder.UseStartup<Startup>();
                });

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddYamlFile("appsettings.yaml");
            })
            .UseStartup<Startup>();
    }
}
