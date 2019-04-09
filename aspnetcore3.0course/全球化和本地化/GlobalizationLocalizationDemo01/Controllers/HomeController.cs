using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GlobalizationLocalizationDemo01.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Internal;


namespace GlobalizationLocalizationDemo01.Controllers
{
    public class HomeController : Controller
    {
        readonly IStringLocalizer<HomeController> _localizer;
        readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public HomeController(IStringLocalizer<HomeController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            Console.WriteLine($"Controller：{_localizer["controller_test"].Value}");
            Console.WriteLine($"Share：{_sharedLocalizer["ok"].Value}");

            ViewData["ok"] = _sharedLocalizer["ok"].Value;
            ViewData["no"] = _sharedLocalizer["no"].Value;
            ViewData["cancel"] = _sharedLocalizer["cancel"].Value;
            return View();
        }
        [HttpPost]
        public IActionResult Index(string culture, string returnUrl = "/")
        {
            Response.Cookies.Append(
                           CookieRequestCultureProvider.DefaultCookieName,
                           CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                           new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
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


        //public IActionResult OnPostCreateEssentialAsync()
        //{
        //    HttpContext.Response.Cookies.Append(Constants.EssentialSec,
        //        DateTime.Now.Second.ToString(),
        //        new CookieOptions() { IsEssential = true });

        //    ResponseCookies = Response.Headers[HeaderNames.SetCookie].ToString();

        //    return RedirectToPage("./Index");
        //}


    }
}
