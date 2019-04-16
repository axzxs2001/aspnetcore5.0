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

namespace LogDemo01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

            })
            .ConfigureLogging((hostBuilderContext, logging) =>
             {
                 logging.ClearProviders();
                 logging.AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"));
                 logging.AddConsole(options => options.IncludeScopes = true);
                 //logging.AddDebug();  
                 
                 logging.AddEventSourceLogger();
             });
    }
}
