using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyFilterAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        public MyFilterAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"-----------------ResultFilterAttribute-----------------");
            Console.WriteLine($"*****  name={_name},value={_value}  MyFilterAttribute.OnResultExecuting");
            Console.WriteLine($"-----------------------------------------------");
            if (_name == "cancel")
            {
                context.Result = new ContentResult()
                {
                    Content = "这里是Resource的取消"
                };
            }
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"-----------------ResultFilterAttribute-----------------");
            Console.WriteLine($"*****  name={_name},value={_value}    MyFilterAttribute.OnResultExecuted");
            Console.WriteLine($"-----------------------------------------------");
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine($"-----------------ResultFilterAttribute-----------------");
            Console.WriteLine($"*****  name={_name},value={_value}    MyFilterAttribute.OnResultExecutionAsync");
            Console.WriteLine($"-----------------------------------------------");
            return base.OnResultExecutionAsync(context, next);
        }
    }
}
