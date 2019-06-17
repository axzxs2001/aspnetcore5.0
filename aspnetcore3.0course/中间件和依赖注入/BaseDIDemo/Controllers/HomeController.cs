using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BaseDIDemo.Models;

namespace BaseDIDemo.Controllers
{
    public class HomeController : Controller
    {
        readonly IMyService _myService;
        public HomeController(IMyService myService)
        {
            _myService = myService;
            _myService.PrintTime("构造注入");
        }

        public IActionResult Index([FromServices]IMyService myService)
        {
            myService.PrintTime("Action注入");

            var service = (IMyService)HttpContext.RequestServices.GetService(typeof(IMyService));
            service.PrintTime("HttpContext.RequestServices 注入");
            return View();
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
