namespace MIS.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;

    using ViewModels.Input.Product;

    public class ProductController : AuthenticationController
    {
        private readonly IWareHouseService wareHouseService;
        private readonly IProductService productService;

        public ProductController(IWareHouseService wareHouseService, IProductService productService)
        {
            this.wareHouseService = wareHouseService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(string wareHouseName)
        {
            var warehouseCategories = await this.wareHouseService.GetAllCategoriesNamesAsync(wareHouseName, this.User.Identity.Name);

            var result = new CreateProductInputModel()
            {
                CategoryNames = warehouseCategories,
                WareHouseName = wareHouseName
            };

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return await this.Create(input.WareHouseName);
            }


            //TODO : Security if it need more
            var wareHouses = this.wareHouseService.GetAllUserWareHousesByUserName(this.User.Identity.Name);

            if (!wareHouses.Select(x => x.Name).Contains(input.WareHouseName))
            {
                return await this.Create(input.WareHouseName);
            }

            await this.productService
                      .CreateAsync(input.Name, input.Price, input.Quantity,
                          input.BarCode, input.CategoryName, input.WareHouseName, this.User.Identity.Name);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}