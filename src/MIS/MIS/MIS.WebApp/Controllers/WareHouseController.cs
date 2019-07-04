using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.WareHouse;
    using ViewModels.View.Product;
    using ViewModels.View.WareHouse;

    [Authorize]
    public class WareHouseController : AuthenticationController
    {
        private readonly IWareHouseService wareHouseService;
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;

        public WareHouseController(IWareHouseService wareHouseService, 
            UserManager<MISUser> userManager,
            ICompanyService companyService)
        {
            this.wareHouseService = wareHouseService;
            this.userManager = userManager;
            this.companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var company =  await  this.companyService.GetByUserAsync(currentUser);

            var wareHouse = company.WareHouses.FirstOrDefault(x => x.IsFavorite);

            //TODO : Logic
            if (wareHouse == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var products = wareHouse.Products.Select(x => x.MapTo<WareHouseIndexProductViewModel>()).ToList();

            var result = new IndexWarehouseViewModel
            {
                Products = products,
                WareHouseName = wareHouse.Name
            };

            return this.View(result);
        }

        public IActionResult Create()
        {
            return this.View(new CreateWareHouseInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWareHouseInputModel wareHouseInput)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(Create));
            }

            var userId = this.userManager.GetUserId(this.User);
            

            var serviceModel = await this.wareHouseService.CreateAsync(wareHouseInput.Name,  userId);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}