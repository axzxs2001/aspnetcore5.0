using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConfigrationDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {           
            CreateHostBuilder(args).Build().Run();
        }  

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //命令行配置
                    config.AddCommandLine(args);
                    //环境变量配置
                    config.AddEnvironmentVariables("JAVA_");
                    //azure 
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));
                    config.AddAzureKeyVault(
                        $"https://azurekeyvalue.vault.azure.net",
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());
                });
                webBuilder.UseStartup<Startup>();
            });
    }
}
