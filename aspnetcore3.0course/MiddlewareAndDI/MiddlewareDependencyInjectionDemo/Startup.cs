using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MiddlewareDependencyInjectionDemo
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
            //每个构造函数或方法中，使用的地方都要实例化一个RequeryCountRepository
            //services.AddTransient<IRequeryCountRepository, RequeryCountRepository>();
            //在一次请求中，实例化一个RequeryCountRepository，其他地方共用这个实例
            //services.AddScoped<IRequeryCountRepository, RequeryCountRepository>();
            //所有的请求，实例化一个RequeryCountRepository，所有请求共用这个实例
            services.AddSingleton<IRequeryCountRepository, RequeryCountRepository>();


            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = new RequestDelegate((async context =>
               {
                   Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                   var exception = context.Features.Get<IExceptionHandlerFeature>();
                   var requeryCountRepository = context.RequestServices.GetService<IRequeryCountRepository>();
                   requeryCountRepository.RequestCount[context.TraceIdentifier] = false;
                   var content = @"
                                        ,.
                                      ,_> `.   ,';
                                  ,-`'      `'   '`'._
                               ,,-) ---._   |   .---''`-),.
                             ,'      `.  \  ;  /   _,'     `,
                          ,--' ____       \   '  ,'    ___  `-,
                         _>   /--. `-.              .-'.--\   \__
                        '-,  (    `.  `.,`~ \~'-. ,' ,'    )    _\
                        _<    \     \ ,'  ') )   `. /     /    <,.
                     ,-'   _,  \    ,'    ( /      `.    /        `-,
                     `-.,-'     `.,'       `         `.,'  `\    ,-'
                      ,'       _  /   ,,,      ,,,     \     `-. `-._
                     /-,     ,'  ;   ' _ \    / _ `     ; `.     `(`-\
                      /-,        ;    (o)      (o)      ;          `'`,
                    ,~-'  ,-'    \     '        `      /     \      <_
                    /-. ,'        \                   /       \     ,-'
                      '`,     ,'   `-/             \-' `.      `-. <
                       /_    /      /   (_     _)   \    \          `,
                         `-._;  ,' |  .::.`-.-' :..  |       `-.    _\
                           _/       \  `:: ,^. :.:' / `.        \,-'
                         '`.   ,-'  /`-..-'-.-`-..-'\            `-.
                           >_ /     ;  (\/( ' )\/)  ;     `-.    _<
                           ,-'      `.  \`-^^^-'/  ,'        \ _<
                            `-,  ,'   `. `""""""""""' ,'   `-.   <`'
                              ')        `._.,,_.'        \ ,-'
                               '._        '`'`'   \       <
                                  >   ,'       ,   `-.   <`'
                                   `,/          \      ,-`
                                    `,   ,' |   /     /
                                     '; /   ;        (
                                      _) |   `       (
                                      `')         .-'
                                        < _   \   / 
                                          \   /\(
                                           `;/  `
";
                   context.Response.ContentType = "text/plain; charset=utf-8";
                   await context.Response.WriteAsync($"Exception Message:{exception?.Error?.Message}\r\nthis is a lion! \r\n{content}");
               }))
            });
            app.UseRequestCenter();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
