using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthenticationAuthorization_Cookie_01
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

            services.AddDataProtection().PersistKeysToPostgres(Configuration.GetConnectionString("Postgre"));


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddControllersWithViews()
                .AddNewtonsoftJson();
            services.AddRazorPages();

            //添加认证Cookie信息
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
            app.UseCookiePolicy();
        }
    }


    public static class DataProtectionKeyExtensions
    {
        public static IDataProtectionBuilder PersistKeysToPostgres(this IDataProtectionBuilder builder, string connectionString)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException("connectionString is empty");
            }
            builder.Services.Configure<KeyManagementOptions>(options => options.XmlRepository = new DataProtectionKeyRepository(connectionString));
            return builder;
        }
    }

    public class DataProtectionKeyRepository : IXmlRepository
    {
        readonly string _connectionString;
        public DataProtectionKeyRepository(string connectionString)
        {
            _connectionString = connectionString;

        }
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return new ReadOnlyCollection<XElement>(GetXElements());
        }
        public void StoreElement(XElement element, string friendlyName)
        {
            var entity = GetElement(friendlyName);
            if (null != entity)
            {
                entity.XmlData = element.ToString();
                Update(entity);
            }
            else
            {
                Add(new DataProtectionKey
                {
                    FriendlyName = friendlyName,
                    XmlData = element.ToString()
                });
            }
        }
        /// <summary>
        /// create DataProtectionKeys
        /// </summary>
        void CreateTable()
        {
            using (var con = new Npgsql.NpgsqlConnection(_connectionString))
            {
                var sql = @"CREATE TABLE if not exists public.""DataProtectionKeys""
 (
     ""FriendlyName"" character varying(256) COLLATE pg_catalog.""default"" NOT NULL,
     ""XmlData"" text COLLATE pg_catalog.""default"",
     CONSTRAINT ""DataProtectionKeys_pkey"" PRIMARY KEY(""FriendlyName"")
 )";
                using (var cmd = new Npgsql.NpgsqlCommand(sql, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        /// <summary>
        /// add dataProtectionKey
        /// </summary>
        /// <param name="dataProtectionKey">Data Protection Key</param>
        /// <returns></returns>
        public bool Add(DataProtectionKey dataProtectionKey)
        {
            using (var con = new Npgsql.NpgsqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO public.""DataProtectionKeys""(""FriendlyName"", ""XmlData"")  VALUES(@FriendlyName, @XmlData);";
                using (var cmd = new Npgsql.NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@FriendlyName", dataProtectionKey.FriendlyName));
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@XmlData", dataProtectionKey.XmlData));
                    con.Open();
                    var result = cmd.ExecuteNonQuery() > 0;
                    con.Close();
                    return result;
                }
            }
        }
        /// <summary>
        /// get DataProtectionKey by FriendlyName
        /// </summary>
        /// <param name="friendlyName">Friendly Name</param>
        /// <returns></returns>
        public DataProtectionKey GetElement(string friendlyName)
        {
            using (var con = new Npgsql.NpgsqlConnection(_connectionString))
            {
                var sql = @"select ""FriendlyName"",""XmlData"" from public.""DataProtectionKeys"" where  ""FriendlyName""=@FriendlyName;";
                using (var cmd = new Npgsql.NpgsqlCommand(sql, con))
                {
                    DataProtectionKey dataProtectionKey = null;
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@FriendlyName", friendlyName));
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataProtectionKey = new DataProtectionKey();
                            dataProtectionKey.FriendlyName = reader.GetString(0);
                            dataProtectionKey.XmlData = reader.GetString(1);
                        }
                        reader.Close();
                    }
                    con.Close();
                    return dataProtectionKey;
                }
            }
        }
        /// <summary>
        /// get XmlData list
        /// </summary>
        /// <returns></returns>
        public IList<XElement> GetXElements()
        {
            CreateTable();
            using (var con = new Npgsql.NpgsqlConnection(_connectionString))
            {
                var sql = @"select ""XmlData"" from public.""DataProtectionKeys"" ";
                using (var cmd = new Npgsql.NpgsqlCommand(sql, con))
                {
                    var elements = new List<XElement>(); 
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            elements.Add(XElement.Parse(reader.GetString(0)));
                        }
                        reader.Close();
                    }
                    con.Close();
                    return elements;
                } 
            }
        }

        public bool Update(DataProtectionKey dataProtectionKey)
        {
            using (var con = new Npgsql.NpgsqlConnection(_connectionString))
            {
                var sql = @"update public.""DataProtectionKeys"" set ""XmlData""=@XmlData where  ""FriendlyName""=@FriendlyName;";
                using (var cmd = new Npgsql.NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@FriendlyName", dataProtectionKey.FriendlyName));
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("@XmlData", dataProtectionKey.XmlData));
                    con.Open();
                    var result = cmd.ExecuteNonQuery() > 0;
                    con.Close();
                    return result;
                }               
            }
        }
    }
    public class DataProtectionKey
    {
        public string FriendlyName { get; set; }
        public string XmlData { get; set; }
    }


}
