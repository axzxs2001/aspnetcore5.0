using AuthenticationAuthorization_Cookie_02.Permission;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PolicyPrivilegeManagement.Models
{
    /// <summary>
    /// 权限授权Handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 用户权限
        /// </summary>
        public List<UserPermission> UserPermissions { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var route = (context.Resource as Microsoft.AspNetCore.Routing.RouteEndpoint);
            var questUrl = "";

            if (route.RoutePattern.Parameters.Count > 0)
            {
                questUrl = $"{route.RoutePattern.Defaults["controller"].ToString().ToLower() }/{route.RoutePattern.Defaults["action"].ToString().ToLower()}";
            }
            else
            {
                questUrl = route.RoutePattern.RawText;
            }
            //赋值用户权限
            UserPermissions = requirement.UserPermissions;
            //是否经过验证
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                if (UserPermissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == questUrl).Count() > 0)
                {
                    //用户名
                    var userName = context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;
                    if (UserPermissions.Where(w => w.UserName == userName && w.Url.ToLower() == questUrl).Count() > 0)
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