using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalizationLocalizationDemo01.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalizationLocalizationDemo01.Controllers
{
    public class UserController : Controller
    {
        readonly List<UserViewModel> _users;
        public UserController()
        {
            _users = new List<UserViewModel> {
                new UserViewModel { UserName="guisuwei", Password="111111" },
                new UserViewModel { UserName="zhangsanfeng", Password="222222" },
            };
        }
        // GET: User
        public ActionResult Index()
        {
            return View(_users);
        }

        // GET: User/Details/5
        public ActionResult Details(string userName)
        {
            return View(_users.SingleOrDefault(s=>s.UserName== userName));
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string userName)
        {
            return View(_users.SingleOrDefault(s => s.UserName == userName));
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(string userName)
        {
            return View(_users.SingleOrDefault(s => s.UserName == userName));
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}