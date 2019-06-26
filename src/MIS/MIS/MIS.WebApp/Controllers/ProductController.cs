namespace MIS.WebApp.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    public class ProductController : AuthenticationController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string ad = "asd")
        {
            return View();
        }
    }
}