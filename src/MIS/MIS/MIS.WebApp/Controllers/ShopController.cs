namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using BindingModels.Shop;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;

    using ViewModels.SystemProductModels;

    public class ShopController : BaseController
    {
        private readonly ISystemProductService productService;
        private readonly IMapper mapper;
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;

        public ShopController(ISystemProductService productService
            , IMapper mapper
            , UserManager<MISUser> userManager
            , ICompanyService companyService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.companyService = companyService;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy(BuyBindingModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return await this.Buy(input.ProductId);
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.companyService.CreateAsync(input.CompanyName, input.Address, userId);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}