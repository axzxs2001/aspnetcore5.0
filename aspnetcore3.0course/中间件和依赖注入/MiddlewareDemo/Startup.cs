using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MiddlewareDemo
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
            services.AddSingleton<IRequeryCountRepository, RequeryCountRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            // if (env.IsDevelopment())
            //{
            //   app.UseDeveloperExceptionPage();
            //}
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = new RequestDelegate((context =>
                {
                    var requeryCountRepository = serviceProvider.GetService<IRequeryCountRepository>();
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
                    return context.Response.WriteAsync($"this is a lion! \r\n{content}");
                }))
            });
            app.UseRequestCenter();
            app.UseMvc();
        }

    }
}
