using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;
using System.Net;

namespace APIDemo02
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
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
            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //这个集合模拟用户权限表,可从数据库中查询出来
            var permission = new List<Permission> {
                              new Permission {  Url="products", Name="admin"},
                              new Permission {  Url="product/{id}", Name="admin"},
                              new Permission {  Url="addproduct", Name="admin"},
                              new Permission {  Url="modifyproduct", Name="admin"},
                              new Permission {  Url="removeproduct/{id}", Name="admin"},
                              new Permission {  Url="products", Name="system"},
                              new Permission {  Url="product/{id}", Name="system"}
                          };
            //如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(
                "/api/denied", permission,
                ClaimTypes.Role,
                audienceConfig["Issuer"],
                audienceConfig["Audience"],
                signingCredentials,
                expiration: TimeSpan.FromSeconds(1000000)//设置Token过期时间
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy => policy.AddRequirements(permissionRequirement));
            }).
            AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
             {

                 //不使用https
                 o.RequireHttpsMetadata = false;
                 o.TokenValidationParameters = tokenValidationParameters;

                 o.Events = new JwtBearerEvents
                 {
                     OnTokenValidated = context =>
                     {
                         if (context.Request.Path.Value.ToString() == "/api/logout")
                         {
                             var token = ((context as TokenValidatedContext).SecurityToken as JwtSecurityToken).RawData;
                         }
                         return Task.CompletedTask;
                     }
                 };
             });
            //注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);


            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "APIDemo02", Version = "v1", Contact = new OpenApiContact { Email = "", Name = "APIDemo02" }, Description = "APIDemo02 Details" });
                var xmlPath = Path.Combine(basePath, "APIDemo02.xml");
                options.IncludeXmlComments(xmlPath, true);            

                var schemeName = "Bearer";
                //如果用Token验证，会在Swagger界面上有验证              
                options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "请输入带有Bearer的Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = schemeName.ToLowerInvariant(),
                    BearerFormat = "JWT"
                }); 
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = schemeName
                            }
                        },
                        new string[0]
                    }
                });
            });

            services.AddControllers()
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
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "APIDemo02";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIDemo02");
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












