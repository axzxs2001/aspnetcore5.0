using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project01.Models;

namespace Project01.Controllers
{
    //[AutoValidateAntiforgeryToken]
    [Authorize(Roles = "admin,system")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("/adduser")]
        public async Task<IActionResult> AddUser([FromBody]UserModel userModel)
        {
            try
            {
                var baseAddress = new Uri("http://192.168.252.41:5400");
                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                {
                    using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                    {
                        var content = new StringContent("{\"username\":\"" + userModel.UserName + "\",\"rolename\":\"" + userModel.RoleName + "\"}", Encoding.UTF8, "application/json");
                        foreach (var cookie in Request.Cookies)
                        {
                            if (!cookie.Key.Contains(".AspNetCore.Antiforgery."))
                            {
                                cookieContainer.Add(baseAddress, new Cookie(cookie.Key, cookie.Value));
                            }
                        }
                        var tokenResponse = await client.GetAsync("/gettoken");
                        var token = await tokenResponse.Content.ReadAsStringAsync();
                        client.DefaultRequestHeaders.Add("X-CSRF-TOKEN-GSW", token);
                        var result = client.PostAsync("/adduser", content).Result;
                        Console.WriteLine($"  adduser返回值：{ await result.Content.ReadAsStringAsync()}");
                    }
                }
                return Ok("添加用户成功");
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            return Redirect("http://localhost:5400/login");
        }
    }
    public class UserModel
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
