namespace MIS.WebApp.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using BindingModels;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;

    using ViewModels;

    [AutoValidateAntiforgeryToken]
    public class SystemProductController : AdministratorController
    {
        private readonly ISystemProductService productService;
        private readonly UserManager<MISUser> userManager;
        private readonly IMapper mapper;

        public SystemProductController(ISystemProductService productService
            , UserManager<MISUser> userManager
            , IMapper mapper)
        {
            this.productService = productService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => this.View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SystemProductCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            await this.productService
                      .CreateSystemProductAsync(input.Name, input.Price, input.ImgUrl, input.Description, user.Id);


            return await Task.Run(() => this.RedirectToAction("Index", "Shop"));
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await this.productService.GetSystemProductByIdAsync(id);

            var result = this.mapper.Map<SystemProductDetailsViewModel>(product);

            return await Task.Run(() => this.View(result));
        }

        [HttpPost]
        public async Task<IActionResult> Details(SystemProductDetailsBindingModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return await this.Details(input.Id);
            }

            await this.productService.UpdateSystemProductByIdAsync(input.Id, input.Name, input.Price, input.ImgUrl, input.Description);

            return await Task.Run(() => this.RedirectToAction("Index", "Shop"));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.productService.DeleteSystemProductAsync(id);

            return await Task.Run(() => this.RedirectToAction("Index", "Shop"));
        }
    }
}