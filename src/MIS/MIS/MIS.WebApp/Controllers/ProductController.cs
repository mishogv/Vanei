namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Category;
    using ViewModels.Input.Product;

    public class ProductController : AuthenticationController
    {
        private const string RedirectIndex = "Index";
        private const string RedirectWareHouse = "WareHouse";


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

            var result = new ProductCreateInputModel()
            {
                Categories = categories.MapTo<CategoryCreateWareHouseInputModel[]>(),
                WarehouseId = id
            };

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return await this.Create(input.WarehouseId);
            }


            await this.productService
                      .CreateAsync(input.Name, input.Price, input.Quantity,
                          input.BarCode, input.CategoryId, input.WarehouseId);

            return this.RedirectToAction(RedirectIndex, RedirectWareHouse);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var product = await this.productService.GetProductAsync(id);

            if (product == null)
            {
                return this.RedirectToAction(nameof(Create));
            }

            var result = product.MapTo<EditProductInputModel>();
            var categories = await this.categoryService.GetAllCategoriesAsync(product.WareHouseId);
            result.Categories = categories.MapTo<CategoryCreateWareHouseInputModel[]>();
            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel input)
        {
            await this.productService.UpdateAsync(input.Id, input.Name, input.Price, input.Quantity,
                input.BarCode, input.CategoryId);

            return this.RedirectToAction(RedirectIndex, RedirectWareHouse);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.productService.DeleteAsync(id);

            return this.RedirectToAction(RedirectIndex, RedirectWareHouse);
        }
    }
}