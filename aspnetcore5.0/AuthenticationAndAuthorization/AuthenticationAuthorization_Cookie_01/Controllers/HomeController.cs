﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthenticationAuthorization_Cookie_01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace AuthenticationAuthorization_Cookie_01.Controllers
{
    [Authorize(Roles = "admin,system")]
    public class HomeController : Controller
    {

        public HomeController(IMemoryCache aaa)
        {

        }


        public IActionResult Index()
        {
            var v = HttpContext.RequestServices;
            Console.WriteLine("##################" + HttpContext.TraceIdentifier);
            return View();
        }
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            Console.WriteLine("##################===============-------" + HttpContext.Request.Cookies[".AspNetCore.Cookies"]);
            Console.WriteLine("##################" + HttpContext.TraceIdentifier);
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string userName, string password, string returnUrl = null)
        {
            Console.WriteLine("##################" + HttpContext.TraceIdentifier);
            var list = new List<dynamic> {
                 new { UserName = "gsw", Password = "111111", Role = "admin",Name="桂素伟" },
                 new { UserName = "aaa", Password = "222222", Role = "system" ,Name="路人甲"}
             };
            var user = list.SingleOrDefault(s => s.UserName == userName && s.Password == password);
            if (user != null)
            {
                //用户标识
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Sid, userName));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                if (returnUrl == null)
                {
                    returnUrl = TempData["returnUrl"]?.ToString();
                }
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return Redirect("/");
                }
            }
            else
            {
                return BadRequest("用户名或密码错误！");
            }
        }

        public async Task<IActionResult> Logout()
        {

            Console.WriteLine("##################" + HttpContext.TraceIdentifier);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }
      
        [Authorize(Roles = "admin")]
        public IActionResult AdminPage()
        {
            Console.WriteLine("##################===============-------" + HttpContext.Request.Cookies[".AspNetCore.Cookies"]);
            Console.WriteLine("##################" + HttpContext.TraceIdentifier);
            return View();
        }
        [Authorize(Roles = "system")]
        public IActionResult SystemPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
