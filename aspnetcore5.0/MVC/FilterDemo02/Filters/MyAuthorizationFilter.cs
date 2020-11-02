using Microsoft.AspNetCore.Mvc;
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
            if (context.ActionDescriptor.Parameters.Count > 0 && context.ActionDescriptor.Parameters[0].Name.ToLower() == "id" && context.RouteData.Values["id"].ToString() == "3")
            {
                context.Result = new ContentResult()
                {
                    Content = "这里是Authorization的取消"
                };
            }

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
