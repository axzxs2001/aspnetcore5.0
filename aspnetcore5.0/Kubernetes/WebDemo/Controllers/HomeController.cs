using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebDemo.Models;
using System.IO;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            try
            {
                ViewBag.connectionstring = _configuration.GetConnectionString("Postgre");
                ViewBag.path =  GetFile(Environment.CurrentDirectory);
                return View();
            }
            catch (Exception exc)
            {
                ViewBag.connectionstring = "";
                ViewBag.path = exc.Message;
                return View();
            }
        }
        string GetFile(string dir)
        {
            var path = "";
            foreach (var cdir in Directory.GetDirectories(dir))
            {
                path += GetFile(cdir);
            }
            foreach (var file in Directory.GetFiles(dir))
            {
                path += file + ",";

            }
            return path;
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
