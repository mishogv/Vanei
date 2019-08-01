namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Product;

    public class ProductController : AuthenticationController
    {
        private readonly IWareHouseService wareHouseService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductController(IWareHouseService wareHouseService, 
            IProductService productService, 
            ICategoryService categoryService)
        {
            this.wareHouseService = wareHouseService;
            this.productService = productService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Create(string id)
        {
            var categories = await this.categoryService.GetAllCategories(id);

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
            
            if (!this.ModelState.IsValid)
            {
                return await this.Create(input.WarehouseId);
            }


            await this.productService
                      .CreateAsync(input.Name, input.Price, input.Quantity,
                          input.BarCode, input.CategoryId, input.WarehouseId);

            return this.RedirectToAction("Index", "WareHouse");
        }

        public async Task<IActionResult> Edit(string id)
        {
            //TODO : Security parameter tampering

            var product = await this.productService.GetProductAsync(id);

            var result = product.MapTo<EditProductInputModel>();
            result.Categories = await this.categoryService.GetAllCategories(product.WareHouseId);

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel input)
        {
            //TODO : Security parameter tampering

            await this.productService.UpdateAsync(input.Id, input.Name, input.Price, input.Quantity,
                input.BarCode, input.CategoryId);

            return this.RedirectToAction("Index", "WareHouse");
        }

        public async Task<IActionResult> Delete(string id)
        {
            //TODO : Security parameter tampering
            await this.productService.DeleteAsync(id);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}