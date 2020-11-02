using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine($"-------------------- Exception---------------------");
            Console.WriteLine($"***** {context.Exception.Message}  MyExceptionFilter.OnException");
            Console.WriteLine($"-----------------------------------------------");
            if (context.ActionDescriptor.Parameters.Count > 0 && context.ActionDescriptor.Parameters[0].Name.ToLower() == "id" && context.RouteData.Values["id"].ToString() == "2")
            {
                context.Result = new ContentResult()
                {
                    Content = "这里是Exception的取消"
                };
            }
        }
    }

    public class MyAsyncExceptionFilter : IAsyncExceptionFilter
    {

        public Task OnExceptionAsync(ExceptionContext context)
        {
            Console.WriteLine($"--------------------Exception---------------------");
            Console.WriteLine($"***** {context.Exception.Message}  MyAsyncExceptionFilter.OnExceptionAsync");
            Console.WriteLine($"-----------------------------------------------");
            return Task.CompletedTask;
        }
    }
}
