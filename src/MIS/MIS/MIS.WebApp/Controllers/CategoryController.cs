namespace MIS.WebApp.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;

    using ViewModels.Input.Category;

    public class CategoryController : AuthenticationController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Create(int id)
        {
            return this.View(new CreateCategoryInputModel { Id = id});
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel categoryInput)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryInput);
            }

            //TODO : VALIDATE AND SECURITY

            await this.categoryService.CreateAsync(categoryInput.Name, categoryInput.Id);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}