using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCDemo01.Models;
using MVCDemo01.Services;

namespace MVCDemo01.Controllers
{
    public class UsersController : Controller
    {
        readonly IUsersService _usersService;
        public UsersController(IUsersService userService)
        {
            _usersService = userService;
        }

        #region list
        public IActionResult Index()
        {
            return View(new List<UserViewModel>() { new UserViewModel { ID = 1, UserName = "gsw", Password = "123456" } });
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
        public IActionResult Delete(int id)
        {
            return View(new UserViewModel { ID = id, UserName = "gsw", Password = "123456" });
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
        public IActionResult Edit(int id)
        {
            return View(new UserViewModel { ID = id, UserName = "gsw", Password = "123456" });
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
        public IActionResult Details(int id)
        {
            return View(new UserViewModel { ID = id, UserName = "gsw", Password = "123456" });
        }
        #endregion
    }
}