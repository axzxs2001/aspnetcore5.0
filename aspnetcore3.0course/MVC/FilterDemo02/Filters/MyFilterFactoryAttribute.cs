using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDemo02.Filters
{
    public class MyFilterFactoryAttribute : Attribute, IFilterFactory
    {

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new MyResultFilter();
        }
        private class MyResultFilter : IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext context)
            {
                Console.WriteLine($"--------------------IFilterFactory---------------------");
                Console.WriteLine($"*****  MyFilterFactoryAttribute.MyResultFilter.OnActionExecuting");
                Console.WriteLine($"-------------------------------------------------------");
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
                Console.WriteLine($"--------------------IFilterFactory---------------------");
                Console.WriteLine($"*****  MyFilterFactoryAttribute.MyResultFilter.OnResultExecuted");
                Console.WriteLine($"-------------------------------------------------------");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
