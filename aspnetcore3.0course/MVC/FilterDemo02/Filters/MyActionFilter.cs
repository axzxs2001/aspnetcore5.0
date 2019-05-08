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
            Console.WriteLine($"--------------------Action---------------------");
            Console.WriteLine($"*****  MyActionFilter.OnActionExecuted");
            Console.WriteLine($"-----------------------------------------------");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"--------------------Action---------------------");
            Console.WriteLine($"*****  MyActionFilter.OnActionExecuting");
            Console.WriteLine($"-----------------------------------------------");
        }
    }

    public class MyAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"--------------------Action---------------------");
            Console.WriteLine($"*****  MyAsyncActionFilter.OnActionExecutionAsync");
            Console.WriteLine($"-----------------------------------------------");
            var resultContext = await next();
        }
    }
}
