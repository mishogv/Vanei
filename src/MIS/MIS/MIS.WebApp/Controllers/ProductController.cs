namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Product;

    public class ProductController : AuthenticationController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductController(IProductService productService,
            ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Create(string id)
        {
            var categories = await this.categoryService.GetAllCategoriesAsync(id);

            var result = new CreateProductInputModel()
            {
                Categories = categories,
                WarehouseId = id
            };

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputModel input)
        {
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
            var product = await this.productService.GetProductAsync(id);

            if (product == null)
            {
                return this.RedirectToAction(nameof(Create));
            }

            var result = product.MapTo<EditProductInputModel>();
            result.Categories = await this.categoryService.GetAllCategoriesAsync(product.WareHouseId);

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel input)
        {
            await this.productService.UpdateAsync(input.Id, input.Name, input.Price, input.Quantity,
                input.BarCode, input.CategoryId);

            return this.RedirectToAction("Index", "WareHouse");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.productService.DeleteAsync(id);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}