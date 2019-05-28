namespace Vanei.WebApp.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using WebApp.Controllers;

    [Area("Administrator")]
    [Authorize(Roles = "Admin")]
    public class AdministratorController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}