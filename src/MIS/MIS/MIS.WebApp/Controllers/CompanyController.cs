using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Services;

    using ViewModels.Input.Category;

    public class CompanyController : AuthenticationController
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public IActionResult Create()
        {
            return this.View(new CreateInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var result = await this.companyService.CreateAsync(input.Name, input.Address, this.User.Identity.Name);

            return this.RedirectToAction("Index" , "Home");
        }
    }
}