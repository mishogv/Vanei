namespace Vanei.WebApp.Controllers
{
    using Data;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using System.Diagnostics;
    using System.Threading.Tasks;

    using Data.Data;

    using Data.Models.Models;

    public class HomeController : BaseController
    {
        private readonly VaneiDbContext dbContext;

        public HomeController(VaneiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            return this.View();
        }

        public async Task<IActionResult> Shop()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
