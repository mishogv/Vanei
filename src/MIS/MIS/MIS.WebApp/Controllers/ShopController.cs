namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Authorization;

    using Services;

    public class ShopController : BaseController
    {
        private readonly ISystemProductsService productsService;

        public ShopController(ISystemProductsService productsService)
        {
            this.productsService = productsService;
        }

        //TODO: BUY System

        public async Task<IActionResult> Index()
        {
            await this.productsService.GetAllSystemProductsAsync();
            return this.View();
        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            return this.View();
        }
    }
}