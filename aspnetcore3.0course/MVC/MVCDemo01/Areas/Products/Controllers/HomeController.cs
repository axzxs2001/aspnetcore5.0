using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCDemo01.Areas.Products.Models;
using MVCDemo01.Areas.Products.Services;

namespace MVCDemo01.Areas.Products.Controllers
{
    [Area("Products")]
    public class HomeController : Controller
    {
        readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View(_productService.GetProducts());
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int id)
        {
            return View(_productService.GetProduct(id));
        }
        public IActionResult Delete(int id)
        {
            return View(_productService.GetProduct(id));
        }
        public IActionResult Details(int id)
        {
            return View(_productService.GetProduct(id));
        }
    }
}