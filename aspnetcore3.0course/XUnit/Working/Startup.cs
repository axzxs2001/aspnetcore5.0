using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Working.Models.DataModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Working.Models.Repository;
using System.Data;
using Microsoft.Extensions.Hosting;

namespace Working
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
            AddRepository(services);
            services.AddControllersWithViews()
              .AddNewtonsoftJson();
            services.AddRazorPages(options =>
            {
               // options.Conventions.AllowAnonymousToPage("/userindex");
            });
            //验证注放
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
               {
                   options.LoginPath = new PathString("/login");
                   options.AccessDeniedPath = new PathString("/denied");
               });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        /// <summary>
        /// 注放仓储
        /// </summary>
        /// <param name="services">服务容器</param>
        void AddRepository(IServiceCollection services)
        {
            //注入连接字符串
            //var connectionString = string.Format(Configuration.GetConnectionString("DefaultConnection"), System.IO.Directory.GetCurrentDirectory());
            //集成测试修改
            var connectionString = string.Format("Data Source={0}/working_db.sqlite", System.IO.Directory.GetCurrentDirectory());

            services.AddSingleton(connectionString);

            //sqlieconnection注放

            services.AddScoped<IDbConnection, SqliteConnection>();
            //注放数据库
            services.AddScoped<IWorkingDB, WorkingDB>();
            //注入用户仓储
            services.AddScoped<IUserRepository, UserRepository>();
            //注入部门仓储
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //注入工作仓储
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            //注放角色仓储
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
