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
        private readonly IWareHouseService wareHouseService;

        public CategoryController(ICategoryService categoryService, IWareHouseService wareHouseService)
        {
            this.categoryService = categoryService;
            this.wareHouseService = wareHouseService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel categoryInput)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryInput);
            }

            var usernames = this.wareHouseService.GetAllUserWareHousesByUserName(this.User.Identity.Name);

            if (!usernames.Select(x => x.Name).Contains(categoryInput.WareHouseName))
            {
                return this.View(categoryInput);
            }

            await this.categoryService.CreateAsync(categoryInput.Name, categoryInput.WareHouseName);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}