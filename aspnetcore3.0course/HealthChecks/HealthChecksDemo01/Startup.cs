using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthChecksDemo01
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

            //最简单模式
            services.AddHealthChecks();
            //自定义健康检查
            services.AddScoped<IDbConnection, Npgsql.NpgsqlConnection>();
            services.AddHealthChecks()
                .AddCheck<PostgreHealthCheck>("postgre_health_check");


            //services.AddHealthChecks()
            //    .AddCheck("postgre_health_check", () =>
            //    HealthCheckResult.Healthy("postgre_health_check is ok!"), tags: new[] { "postgre" });


            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //最简单模式
            app.UseHealthChecks("/health");

            //加端口
            app.UseHealthChecks("/health", 8800);



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
