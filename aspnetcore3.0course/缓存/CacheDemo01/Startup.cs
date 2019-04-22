using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DistributedPostgreCache;
namespace CacheDemo01
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
            services.AddMemoryCache();
            services.AddDistributedPostgreCache(options =>
            {
                options.ConnectionString =
                    Configuration.GetConnectionString("DefaultConnectionString");
                options.TableName = "cachetable";
            });
            services.AddControllers()
                .AddNewtonsoftJson();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDistributedCache cache)
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
