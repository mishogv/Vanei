namespace MIS.WebApp.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Input.Product;

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
        public IActionResult Create(CreateProductInputModel input)
        {
            return View();
        }
    }
}