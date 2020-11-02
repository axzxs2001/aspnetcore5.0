using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIDemo02.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize("Permission")]
    public class PermissionController : Controller

    {

        /// <summary>
        /// 自定义策略参数
        /// </summary>
        readonly PermissionRequirement _requirement;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirement"></param>
        public PermissionController(PermissionRequirement requirement)
        {
            _requirement = requirement;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/api/login")]
        public  IActionResult Login(string username, string password, string role)
        {

            var isValidated = username == "gsw" && password == "111111";
            if (!isValidated)
            {
                return new JsonResult(new
                {
                    Status = false,
                    Message = "认证失败"
                });
            }
            else
            {
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new Claim[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString())
                };           

                var token = JwtToken.BuildJwtToken(claims, _requirement);
                return new JsonResult(token);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpPost("/api/logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/api/denied")]
        public IActionResult Denied()
        {
            return new JsonResult(new
            {
                Status = false,
                Message = "你无权限访问"
            });
        }

    }
}
