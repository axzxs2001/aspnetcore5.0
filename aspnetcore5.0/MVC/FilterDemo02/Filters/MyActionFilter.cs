using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"-------------------- Action---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyActionFilter.OnActionExecuted");
            Console.WriteLine($"-----------------------------------------------");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"--------------------Action---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyActionFilter.OnActionExecuting");
            Console.WriteLine($"-----------------------------------------------");
            if (context.ActionDescriptor.Parameters.Count > 0 && context.ActionDescriptor.Parameters[0].Name.ToLower() == "id" && context.RouteData.Values["id"].ToString() == "5")
            {
                context.Result = new ContentResult()
                {
                    Content = "这里是Action的取消"
                };
            }
        }
    }

    public class MyAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"--------------------Action---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyAsyncActionFilter.OnActionExecutionAsync");
            Console.WriteLine($"-----------------------------------------------");
            var resultContext = await next();
        }
    }
}
