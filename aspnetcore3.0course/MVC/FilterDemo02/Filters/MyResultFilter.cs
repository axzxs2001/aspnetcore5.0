using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyResultFilter : IResultFilter
    {   
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"-------------------- Result---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyResultFilter.OnResultExecuted");
            Console.WriteLine($"-----------------------------------------------");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"-------------------- Result---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyResultFilter.OnResultExecuting");
            Console.WriteLine($"-----------------------------------------------");
            if (context.ActionDescriptor.Parameters.Count > 0 && context.ActionDescriptor.Parameters[0].Name.ToLower() == "id" && context.RouteData.Values["id"].ToString() == "7")
            {
                context.Result = new ContentResult()
                {
                    Content = "这里是Result的取消"
                };
            }
        }
    }

    public class MyAsyncResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine($"--------------------Result---------------------");
            Console.WriteLine($"***** {context.ActionDescriptor.DisplayName}  MyAsyncResultFilter.OnResultExecutionAsync");
            Console.WriteLine($"-----------------------------------------------");
            var resultContext = await next();
        }
    }
}
