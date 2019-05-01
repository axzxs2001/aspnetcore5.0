using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCDemo01.Models;

namespace MVCDemo01.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<UserViewModel>() { new UserViewModel { UserName="gsw",Password="123456" } });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserViewModel userViewModel)
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View( new UserViewModel { UserName = "gsw", Password = "123456" });
        }
        [HttpPost]
        public IActionResult Delete(UserViewModel userViewModel)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit(UserViewModel userViewModel)
        {
            return View();
        }
        public IActionResult Edit()
        {
             return View(new UserViewModel { UserName = "gsw", Password = "123456" });
        }
        public IActionResult Details()
        {
            return View(new UserViewModel { UserName = "gsw", Password = "123456" });
        }
    }
}