using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine($"-----------------Authorization-----------------");
            Console.WriteLine($"*****  MyAuthorizationFilter.OnAuthorization");
            Console.WriteLine($"-----------------------------------------------");
        }
    }

    public class MyAsyncAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine($"-----------------Authorization-----------------");
            Console.WriteLine($"*****  MyAsyncAuthorizationFilter.OnAuthorizationAsync");
            Console.WriteLine($"-----------------------------------------------");
            return Task.CompletedTask;
        }
    }
}
