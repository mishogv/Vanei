namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;

    using Services.Models;

    using ViewModels.Input.Category;
    using ViewModels.View.Category;

    public class CategoryController : AuthenticationController
    {
        private const string RedirectCreate = "Create";
        private const string RedirectCompany = "Company";

        private const string RedirectIndex = "Index";
        private const string RedirectWareHouse = "WareHouse";

        private readonly ICategoryService categoryService;
        private readonly UserManager<MISUser> userManager;

        public CategoryController(ICategoryService categoryService, UserManager<MISUser> userManager)
        {
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user?.CompanyId == null)
            {
                return this.RedirectToAction(RedirectCreate, RedirectCompany);
            }

            var categories = await this.categoryService.GetAllByCompanyIdAsync(user.CompanyId);

            var categoryServiceModels = categories as CategoryServiceModel[] ?? categories.ToArray();
            var categoriesViewModels = new List<CategoryIndexDetailsViewModel>();

            foreach (var category in categoryServiceModels)
            {
                var categoryViewModel = category.MapTo<CategoryIndexDetailsViewModel>();
                categoryViewModel.ProductsCount = category.Products.Count;

                categoriesViewModels.Add(categoryViewModel);
            }

            var result = new CategoryIndexViewModel()
            {
                Categories = categoriesViewModels
            };

            return this.View(result);
        }


        public IActionResult Create(string id)
        {
            return this.View(new CategoryCreateInputModel { WareHouseId = id});
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoryService.CreateAsync(input.Name, input.WareHouseId);
            return this.RedirectToAction(RedirectIndex, RedirectWareHouse);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var category = await this.categoryService.GetCategoryAsync(id);

            if (category == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(category.MapTo<CategoryEditInputModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoryService.EditAsync(input.Id, input.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var category = await this.categoryService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}