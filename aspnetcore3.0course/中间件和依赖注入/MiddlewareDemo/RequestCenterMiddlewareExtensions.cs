using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDemo
{
    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class RequestCenterMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCenter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCenterMiddleware>();
        }
    }
}
