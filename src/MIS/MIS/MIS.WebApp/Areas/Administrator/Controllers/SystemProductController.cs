namespace MIS.WebApp.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;

    using ViewModels;

    public class SystemProductController : AdministratorController
    {
        private readonly ISystemProductsService productsService;
        private readonly UserManager<MISUser> userManager;

        public SystemProductController(ISystemProductsService productsService, UserManager<MISUser> userManager)
        {
            this.productsService = productsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CreateSystemProduct()
        {
            return await Task.Run(() => this.View());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSystemProduct(SystemProductCreateViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            await this.productsService.CreateSystemProductAsync(inputModel.Name, inputModel.Price, inputModel.ImgUrl, inputModel.Description, user.Id);
            

            return await Task.Run(() => this.RedirectToAction("Index", "Shop"));
        }
    }
}