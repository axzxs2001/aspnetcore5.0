using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCDemo01.Models;

namespace MVCDemo01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new MainPageModel { Title="主页", Content= "Free. Cross-platform. Open source.A framework for building web apps and services with.NET and C#.", CopyRight= "2019©" });
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
    }
}
