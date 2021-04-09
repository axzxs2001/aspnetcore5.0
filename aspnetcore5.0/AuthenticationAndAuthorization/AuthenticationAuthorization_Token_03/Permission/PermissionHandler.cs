
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace AuthenticationAuthorization_Token_03
{
    /// <summary>
    /// 权限授权Handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 用户权限
        /// </summary>
        private readonly List<Permission> _userPermissions;

        public PermissionHandler(List<Permission> permissions)
        {
            _userPermissions = permissions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.Resource is DefaultHttpContext)
            {
                var httpContext = context.Resource as DefaultHttpContext;
                var questPath = httpContext?.Request?.Path;
                var method = httpContext?.Request?.Method;
                //是否经过验证
                var isAuthenticated = context?.User?.Identity?.IsAuthenticated;
                if (isAuthenticated.HasValue && isAuthenticated.Value)
                {
                    //用户名
                    var role = context.User.Claims.SingleOrDefault(s => s.Type == requirement.ClaimType).Value;
                    if (_userPermissions.Where(w => w.Credentials == role && w.Method.ToUpper() == method.ToUpper() && w.Url.ToLower() == questPath).Count() > 0)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}