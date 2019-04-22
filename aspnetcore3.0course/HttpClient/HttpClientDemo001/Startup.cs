using System;
using System.Collections.Generic;
using System.Linq;
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
