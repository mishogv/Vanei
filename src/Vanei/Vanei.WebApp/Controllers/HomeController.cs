using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Vanei.WebApp.Models;

namespace Vanei.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    public class HomeController : Controller
    {
        private readonly VaneiDbContext dbContext;

        public HomeController(VaneiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            return this.View(user);
        }

        public async Task<IActionResult> Shop()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            return this.View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
