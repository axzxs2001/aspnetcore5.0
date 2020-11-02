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
using APIDemo02.Models;

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
            //��ȡ�����ļ�
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//������
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//������
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //�������ģ���û�Ȩ�ޱ�,�ɴ����ݿ��в�ѯ����
            var permission = new List<Permission> {
                              new Permission {  Url="/products", Name="admin"},
                              new Permission {  Url="/product/{id}", Name="admin"},
                              new Permission {  Url="/addproduct", Name="admin"},
                              new Permission {  Url="/modifyproduct", Name="admin"},
                              new Permission {  Url="/removeproduct/{id}", Name="admin"},
                              new Permission {  Url="/products", Name="system"},
                              new Permission {  Url="/product/{id}", Name="system"}
                          };
            //�����������������ClaimTypes.Role�����漯�ϵ�ÿ��Ԫ�ص�NameΪ��ɫ���ƣ����ClaimTypes.Name�������漯�ϵ�ÿ��Ԫ�ص�NameΪ�û���
            var permissionRequirement = new PermissionRequirement(
                "/api/denied", permission,
                ClaimTypes.Role,
                audienceConfig["Issuer"],
                audienceConfig["Audience"],
                signingCredentials,
                expiration: TimeSpan.FromSeconds(1000000)//����Token����ʱ��
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

                 //��ʹ��https
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
            //ע����ȨHandler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);


            var basePath = Environment.CurrentDirectory;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "APIDemo02", Version = "v1", Contact = new OpenApiContact { Email = "", Name = "APIDemo02" }, Description = "APIDemo02 Details" });
                var xmlPath = Path.Combine(basePath, "APIDemo02.xml");
                options.IncludeXmlComments(xmlPath, true);

                var schemeName = "Bearer";
                //�����Token��֤������Swagger����������֤              
                options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "�����벻����Bearer��Token",
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












