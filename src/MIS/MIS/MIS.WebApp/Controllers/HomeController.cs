namespace MIS.WebApp.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Models;

    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => this.View());
        }

        public async Task<IActionResult> Privacy()
        {
            return await Task.Run(() => this.View());
        }

        public async Task<IActionResult> Contact()
        {
            return await Task.Run(() => this.View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
