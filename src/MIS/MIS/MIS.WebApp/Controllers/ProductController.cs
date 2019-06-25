namespace MIS.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProductController : AuthenticationController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}