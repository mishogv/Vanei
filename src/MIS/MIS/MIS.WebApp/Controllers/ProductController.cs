namespace MIS.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;

    using ViewModels.Input.Category;
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
            if (wareHouseName == null)
            {
                return this.RedirectToAction("Index", "WareHouse");
            }

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


            var wareHouses = this.wareHouseService.GetAllUserWareHousesByUserName(this.User.Identity.Name);

            var createCategoryWareHouseModels = wareHouses as CreateCategoryWareHouseModel[] ?? wareHouses.ToArray();

            if (!createCategoryWareHouseModels.Select(x => x.Name).Contains(input.WareHouseName))
            {
                return await this.Create(input.WareHouseName);
            }

            if (createCategoryWareHouseModels.SelectMany(x => x.Products).Select(x => x.Name).Contains(input.Name)
            || createCategoryWareHouseModels.SelectMany(x => x.Products).Select(x => x.BarCode).Contains(input.BarCode))
            {
                // Thats should validate that product have unique name and barcode
                this.ModelState.AddModelError("Product", "You can't add product that is already registered in your company.");
                return await this.Create(input.WareHouseName);
            }

            await this.productService
                      .CreateAsync(input.Name, input.Price, input.Quantity,
                          input.BarCode, input.CategoryName, input.WareHouseName, this.User.Identity.Name);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}