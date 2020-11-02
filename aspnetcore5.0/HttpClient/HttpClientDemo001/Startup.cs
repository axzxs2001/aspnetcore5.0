using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HttpClientDemo001.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace HttpClientDemo001
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            //基本用法
            services.AddHttpClient();
            //命名客户端
            services.AddHttpClient("nameclient5000", c =>
            {
                c.BaseAddress = new Uri("http://localhost:5000/");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientDemo001");
            });
            //类型化客户端      
            services.AddTransient<MyHttpClientHandler>();
            services
                .AddHttpClient<ITypeClientRepository, TypeClientRepository>("typeclient5000")
                .AddHttpMessageHandler<MyHttpClientHandler>();//添加请求中间件

            //重试
            services.AddHttpClient("pollyclient5000")
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(1)))
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(15, TimeSpan.FromSeconds(30)));
          
            //证书Client
            services.AddHttpClient("CertificateClient").ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls | SslProtocols.None | SslProtocols.Tls11;
                try
                {
                    var crt = new X509Certificate2(Directory.GetCurrentDirectory() + "/client.pfx", "cccccc");
                    handler.ClientCertificates.Add(crt);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //验证服务器证书是否正规
                handler.ServerCertificateCustomValidationCallback = (message, cer, chain, errors) =>
                {
                    //var verify = false;
                    //foreach (X509ChainElement element in chain.ChainElements)
                    //{
                    //    Console.WriteLine("SerialNumber:{0}", element.Certificate.SerialNumber);
                    //    Console.WriteLine("Element subject name: {0}", element.Certificate.Subject);
                    //    Console.WriteLine("Element issuer name: {0}", element.Certificate.Issuer);
                    //    Console.WriteLine("Element certificate valid until: {0}", element.Certificate.NotAfter);
                    //    Console.WriteLine("Element certificate is valid: {0}", element.Certificate.Verify());
                    //    Console.WriteLine("Element error status length: {0}", element.ChainElementStatus.Length);
                    //    Console.WriteLine("Element information: {0}", element.Information);
                    //    Console.WriteLine("Number of element extensions: {0}{1}", element.Certificate.Extensions.Count, Environment.NewLine);
                    //    if (element.Certificate.Issuer == cer.Issuer)
                    //    {
                    //        verify = element.Certificate.Verify();
                    //    }
                    //}
                    //Console.WriteLine($"*********   X509Certificate2.Verify={cer.Verify()}");
                    //Console.WriteLine($"*********   Element certificate is valid: {verify}");
                    var res = chain.Build(cer);
                    Console.WriteLine($"*********   X509Chain.Build={res}");
                    return res;
                };
                return handler;
            });


            services.AddControllers()
                .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
