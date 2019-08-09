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

    using ViewModels.Input.WareHouse;
    using ViewModels.View.Product;
    using ViewModels.View.WareHouse;

    [Authorize]
    public class WareHouseController : AuthenticationController
    {
        private const string RedirectCreate = "Create";
        private const string RedirectCompany = "Company";
        private const string RedirectWareHouse = "WareHouse";

        private readonly IWareHouseService wareHouseService;
        private readonly UserManager<MISUser> userManager;

        public WareHouseController(IWareHouseService wareHouseService,
            UserManager<MISUser> userManager)
        {
            this.wareHouseService = wareHouseService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user?.CompanyId == null)
            {
                return this.RedirectToAction(RedirectCreate, RedirectCompany);
            }

            var warehouses = await this.wareHouseService.GetWarehousesByCompanyIdAsync(user.CompanyId);

            var currentWarehouse = id == null
                                         ? warehouses.FirstOrDefault(x => x.IsFavorite)
                                         : warehouses.FirstOrDefault(x => x.Id == id);

            if (currentWarehouse == null)
            {
                return this.RedirectToAction(RedirectCreate, RedirectWareHouse);
            }

            var products = currentWarehouse.Products.MapTo<ProductWareHouseIndexViewModel[]>();
            var result = currentWarehouse.MapTo<WarehouseIndexViewModel>();
            result.Products = products;
            result.WarehouseDropdown = warehouses.MapTo<WarehouseIndexDropdownViewModel[]>();
          
            return this.View(result);
        }

        public IActionResult Create()
        {
            return this.View(new WareHouseCreateInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(WareHouseCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.wareHouseService.CreateAsync(input.Name, user?.CompanyId);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Favorite(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.wareHouseService.MakeFavoriteAsync(id, user.CompanyId);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var warehouse = await this.wareHouseService.GetWareHouseAsync(id);

            if (warehouse == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(warehouse.MapTo<WarehouseEditInputModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WarehouseEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.wareHouseService.EditAsync(input.Id, input.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var warehouse = await this.wareHouseService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}