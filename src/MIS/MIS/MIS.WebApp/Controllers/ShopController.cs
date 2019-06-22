namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;

    using ViewModels;
    using ViewModels.SystemProductModels;

    public class ShopController : BaseController
    {
        private readonly ISystemProductService productService;
        private readonly IMapper mapper;
        private readonly UserManager<MISUser> userManager;

        public ShopController(ISystemProductService productService
            , IMapper mapper
            , UserManager<MISUser> userManager)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        //TODO: BUY System

        public IActionResult Index()
        {
            var products = this.productService.GetAllSystemProducts();

            var result = this.mapper.Map<SystemProductShowViewModel[]>(products);

            return this.View(result);
        }

        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var product = await this.productService.GetSystemProductByIdAsync(id);
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