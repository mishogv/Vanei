using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;
    using Services.Mapping;

    using ServicesModels;

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

        public async Task<IActionResult> Index(int? id)
        {
            //TODO : Security
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CompanyId == null)
            {
                return this.RedirectToAction("Create", "Company");
            }

            var warehouses = this.wareHouseService.GetWarehousesByCompanyId(user.CompanyId);

            WareHouseServiceModel currentWarehouse = null;
            var wareHouseServiceModels = warehouses as WareHouseServiceModel[] ?? warehouses.ToArray();

            if (id == null)
            {
                currentWarehouse = wareHouseServiceModels.FirstOrDefault(x => x.IsFavorite);
            }
            else
            {
                currentWarehouse = wareHouseServiceModels.FirstOrDefault(x => x.Id == id);
            }


            if (currentWarehouse == null)
            {
                return this.RedirectToAction("Create", "WareHouse");
            }

            var products = currentWarehouse.Products.Select(x => x.MapTo<WareHouseIndexProductViewModel>()).ToList();

            //TODO : auto mapper 
            var result = new IndexWarehouseViewModel
            {
                Products = products,
                WareHouseName = currentWarehouse.Name,
                WarehouseDropdown = wareHouseServiceModels.Select(x => x.MapTo<IndexWarehouseDropdownViewModel>())
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
            //TODO : SECURITY

            if (!this.ModelState.IsValid)
            {
                return this.View(wareHouseInput);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var serviceModel = await this.wareHouseService.CreateAsync(wareHouseInput.Name, user.CompanyId);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}