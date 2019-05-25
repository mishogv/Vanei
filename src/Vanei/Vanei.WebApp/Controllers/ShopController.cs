using Microsoft.AspNetCore.Mvc;

namespace Vanei.WebApp.Controllers
{
    public class ShopController : Controller
    {
        //TODO: BUY System

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Buy(int id)
        {
            return this.View(id);
        }
    }
}