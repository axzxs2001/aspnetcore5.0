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
        #region list
        public IActionResult Index()
        {
            return View(new List<UserViewModel>() { new UserViewModel { UserName = "gsw", Password = "123456" } });
        }
        #endregion

        #region create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                return Redirect("/users");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region  delete
        public IActionResult Delete()
        {
            return View(new UserViewModel { UserName = "gsw", Password = "123456" });
        }
        [HttpPost]
        public IActionResult Delete(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                return Redirect("/users");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region edit
        public IActionResult Edit()
        {
            return View(new UserViewModel { UserName = "gsw", Password = "123456" });
        }
        [HttpPost]
        public IActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                return Redirect("/users");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region details
        public IActionResult Details()
        {
            return View(new UserViewModel { UserName = "gsw", Password = "123456" });
        }
        #endregion
    }
}