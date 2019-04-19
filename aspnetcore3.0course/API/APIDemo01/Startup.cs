using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace APIDemo01
{/// <summary>
/// 
/// </summary>
    public class Startup
    {/// <summary>
     /// 
     /// </summary>
     /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {


            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "APIDemo01", Version = "v1", Contact = new OpenApiContact { Email = "", Name = "APIDemo01" }, Description = "APIDemo01 Details" });
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "APIDemo01.xml");
                options.IncludeXmlComments(xmlPath, true);
                options.DocInclusionPredicate((docName, description) => true);

                //如果用Token验证，会在Swagger界面上有验证
                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "请输入带有Bearer的Token", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement() { });

            });

            services.AddMvc()
             .AddNewtonsoftJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "APIDemo01";         
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIDemo01");
            });    

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
