using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}