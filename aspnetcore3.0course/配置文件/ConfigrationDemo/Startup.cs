using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ConfigrationDemo
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
            //机密文件          
            var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnectionString"));
            connectionStringBuilder.Password = Configuration["dbpassword"];
            Console.WriteLine(connectionStringBuilder.ToString());

            //绑定
            var appsetting = new Appsetting();
            Configuration.GetSection("Appsetting").Bind(appsetting);

            //热更新
            services.Configure<Appsetting>(Configuration.GetSection("Appsetting"));

            services.AddMvc()
                  .AddNewtonsoftJson();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting(routes =>
            {
                routes.MapControllers();
            });
            app.UseAuthorization();
        }
    }
}
