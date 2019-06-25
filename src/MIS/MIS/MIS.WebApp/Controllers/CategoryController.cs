namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using BindingModels.Category;

    using Microsoft.AspNetCore.Mvc;

    using Services;

    public class CategoryController : AuthenticationController
    {
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;

        public CategoryController(IUserService userService, ICategoryService categoryService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel input)
        {
            //if (!this.ModelState.IsValid)
            //{
            //    return this.View();
            //}

            ////var wareHouseId = await this.userService.GetUserWarehouseIdAsync(this.User.Identity.Name);

            //if (wareHouseId == null)
            //{
            //    return this.View();
            //}

            //await this.categoryService.CreateAsync(input.Name, (int) wareHouseId);

            return this.RedirectToAction("Index", "WareHouse");
        }
    }
}