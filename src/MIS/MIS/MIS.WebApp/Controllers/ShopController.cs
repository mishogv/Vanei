namespace MIS.WebApp.Controllers
{
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;

    using ViewModels;

    public class ShopController : BaseController
    {
        private readonly ISystemProductsService productsService;
        private readonly IMapper mapper;
        private readonly UserManager<MISUser> userManager;

        public ShopController(ISystemProductsService productsService
            , IMapper mapper
            , UserManager<MISUser> userManager)
        {
            this.productsService = productsService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        //TODO: BUY System

        public async Task<IActionResult> Index()
        {
            var products = await this.productsService.GetAllSystemProductsAsync();

            var result = this.mapper.Map<SystemProductShowViewModel[]>(products);

            return this.View(result);
        }

        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var product = await this.productsService.GetSystemProductByIdAsync(id);
            var currentUser = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            var showViewModel = this.mapper.Map<SystemProductShowViewModel>(product);

            var result = new SystemProductBuyViewModel()
            {
                ShowViewModel = showViewModel,
                Username = currentUser.UserName,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
            };

            return this.View(result);
        }
    }
}