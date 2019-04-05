
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



namespace AuthenticationAuthorization_Token_03
{
    /// <summary>
    /// 权限授权Handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {


        public PermissionHandler(IHttpContextFactory httpContextFactory)
        {
            var a = httpContextFactory;
        }
        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            return base.HandleAsync(context);
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            context.Succeed(requirement);
            var route = (context.Resource as Microsoft.AspNetCore.Routing.RouteEndpoint);
            var questUrl = "";
            if (route.RoutePattern.RawText.Contains("{controller") && route.RoutePattern.RawText.Contains("{action"))
            {
                questUrl = $"{route.RoutePattern.Defaults["controller"].ToString().ToLower() }/{route.RoutePattern.Defaults["action"].ToString().ToLower()}";

            }
            else
            {
                questUrl = route.RoutePattern.RawText;
            }
            //赋值用户权限
           var Requirements = requirement.Permissions;



            // if (defaultAuthenticate != null)
            //{
            //var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
            ////result?.Principal不为空即登录成功
            //if (result?.Principal != null)
            //{

            //    httpContext.User = result.Principal;
            //    //权限中是否存在请求的url
            //    if (requirement.Permissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == questUrl).Count() > 0)
            //    {
            //        var name = httpContext.User.Claims.SingleOrDefault(s => s.Type == requirement.ClaimType).Value;
            //        //验证权限
            //        if (requirement.Permissions.Where(w => w.Name == name && w.Url.ToLower() == questUrl).Count() <= 0)
            //        {
            //            //无权限跳转到拒绝页面
            //            httpContext.Response.Redirect(requirement.DeniedAction);
            //        }
            //    }
            //    //判断过期时间
            //    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.Now)
            //    {
            //        context.Succeed(requirement);
            //    }
            //    else
            //    {
            //        context.Fail();
            //    }
            //    return;
            //}
            // }
            //是否经过验证
            var isAuthenticated = context.User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                if (Requirements.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == questUrl).Count() > 0)
                {
                    //用户名
                    var userName = context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;
                    if (Requirements.Where(w => w.Name == userName && w.Url.ToLower() == questUrl).Count() > 0)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        //无权限跳转到拒绝页面
                        context.Fail();
                    }
                }
                else
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}