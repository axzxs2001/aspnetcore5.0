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
            //机密文件中的密码         
            var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("DefaultConnectionString"));
            connectionStringBuilder.Password = Configuration["dbpassword"];
            Console.WriteLine(connectionStringBuilder.ToString());

            //绑定
            var appsetting = new Appsetting();
            Configuration.GetSection("Appsetting").Bind(appsetting);

            //热更新
            services.Configure<Appsetting>(Configuration.GetSection("Appsetting"));

            //azure配置文件
            Console.WriteLine($"Azure pgdb：{Configuration["a"]}");

            //环境变量 
            Console.WriteLine($"环境变量Java_中的值：{Configuration["home"]}");

            services.AddControllers()
                  .AddNewtonsoftJson();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
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
