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

    using Services.Models;

    using ViewModels.Input.WareHouse;
    using ViewModels.View.Product;
    using ViewModels.View.WareHouse;

    [Authorize]
    public class WareHouseController : AuthenticationController
    {
        private readonly IWareHouseService wareHouseService;
        private readonly UserManager<MISUser> userManager;

        public WareHouseController(IWareHouseService wareHouseService,
            UserManager<MISUser> userManager)
        {
            this.wareHouseService = wareHouseService;
            this.userManager = userManager;
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
                Id = currentWarehouse.Id,
                IsFavorite = currentWarehouse.IsFavorite,
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

        public async Task<IActionResult> Favorite(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            //TODO : PARAMETER TAMPERING SECURITY
            await this.wareHouseService.MakeFavoriteAsync(id, user.CompanyId);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            //TODO : Security
            var warehouse = await this.wareHouseService.GetWareHouseAsync(id);

            return this.View(warehouse.MapTo<EditWarehouseInputModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditWarehouseInputModel input)
        {
            //TODO : Security
            await this.wareHouseService.EditAsync(input.Id, input.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //TODO : Security
            var warehouse = await this.wareHouseService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}