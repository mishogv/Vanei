namespace MIS.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

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

        public async Task<IActionResult> Create(int id)
        {
            //TODO : Security parameter tampering

            var categories = this.wareHouseService.GetAllCategories(id);

            var result = new CreateProductInputModel()
            {
                Categories = categories,
                WarehouseId = id
            };

            await Task.CompletedTask;

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputModel input)
        {
            //TODO : VALIDATION AND SECURITY PARAMETER TAMPERING
            ;
            if (!this.ModelState.IsValid)
            {
                return await this.Create(input.WarehouseId);
            }


            //if (createCategoryWareHouseModels.SelectMany(x => x.Products).Select(x => x.Name).Contains(input.Name)
            //|| createCategoryWareHouseModels.SelectMany(x => x.Products).Select(x => x.BarCode).Contains(input.BarCode))
            //{
            //    // That's should validate that product have unique name and barcode
            //    this.ModelState.AddModelError("Product", "You can't add product that is already registered in your company.");
            //    return await this.Create(input.WareHouseName);
            //}

            await this.productService
                      .CreateAsync(input.Name, input.Price, input.Quantity,
                          input.BarCode, input.CategoryId, input.WarehouseId);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}