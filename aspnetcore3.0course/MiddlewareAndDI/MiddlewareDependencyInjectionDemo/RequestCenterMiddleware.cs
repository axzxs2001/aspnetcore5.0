using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDependencyInjectionDemo
{
    /// <summary>
    /// 请求记录中间件
    /// </summary>
    public class RequestCenterMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCenterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRequeryCountRepository requeryCountRepository)
        {
            requeryCountRepository.RequestCount.Add(context.TraceIdentifier, true);
            await _next(context);
            requeryCountRepository.RequestCount[context.TraceIdentifier] = false;
        }
    }
}
