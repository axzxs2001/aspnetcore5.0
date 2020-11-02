using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyResourceFilter : IResourceFilter
    {    
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"--------------------Resource---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyResourceFilter.OnResourceExecuted");
            Console.WriteLine($"-----------------------------------------------");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"--------------------Resource---------------------");
            Console.WriteLine($"*****  {context.ActionDescriptor.DisplayName}  MyResourceFilter.OnResourceExecuting");
            Console.WriteLine($"-----------------------------------------------");
            if (context.ActionDescriptor.Parameters.Count > 0 && context.ActionDescriptor.Parameters[0].Name.ToLower() == "id" && context.RouteData.Values["id"].ToString() == "4")
            {
                context.Result = new ContentResult()
                {
                    Content = "这里是Resource的取消"
                };
            }
        }
    }

    public class MyAsyncResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine($"--------------------Resource---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyAsyncActionFilter.OnActionExecutionAsync");
            Console.WriteLine($"-----------------------------------------------");
            var resultContext = await next();
        }
    }
}
