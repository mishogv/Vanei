using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using BindingModels.WareHouse;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;

    using ViewModels.Product;

    [Authorize]
    public class WareHouseController : AuthenticationController
    {
        private readonly IWareHouseService wareHouseService;
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;
        private readonly IMapper mapper;

        public WareHouseController(IWareHouseService wareHouseService, 
            UserManager<MISUser> userManager,
            ICompanyService companyService,
            IMapper mapper)
        {
            this.wareHouseService = wareHouseService;
            this.userManager = userManager;
            this.companyService = companyService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var company =  await  this.companyService.GetByUserAsync(currentUser);
            //var products = this.wareHouseService.GetAllProductsByWarehouseId();
            //var result = this.mapper.Map<WareHouseIndexProductViewModel[]>(products).ToList();

            return this.View(new List<WareHouseIndexProductViewModel>());
        }

        public IActionResult Create()
        {
            return this.View(new CreateInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(Create));
            }

            var userId = this.userManager.GetUserId(this.User);
            

            var serviceModel = await this.wareHouseService.CreateAsync(input.Name,  userId);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}