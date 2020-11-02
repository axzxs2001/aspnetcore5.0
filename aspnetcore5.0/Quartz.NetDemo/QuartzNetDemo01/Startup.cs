using System;
using System.Collections.Generic;
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
using Quartz;
using QuartzNetDemo01.Model;
using QuartzNetDemo01.Model.DataModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace QuartzNetDemo01
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
            services.Configure<List<CronMethod>>(Configuration.GetSection("CronJob"));
            services.AddTransient<IBackgroundRepository, BackgroundRepository>();


            services.AddQuartz(typeof(BackgroundJob));

            services.AddControllers()
                .AddNewtonsoftJson();

          
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IScheduler scheduler, IOptionsSnapshot<List<CronMethod>> cronJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            foreach (var cronJob in cronJobs.Value)
            {
                QuartzServicesUtilities.StartJob<BackgroundJob>(scheduler, cronJob.CronExpression, cronJob.MethodName);
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
