namespace Vanei.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Authorization;

    public class ShopController : BaseController
    {
        //TODO: BUY System

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            return this.View(id);
        }
    }
}