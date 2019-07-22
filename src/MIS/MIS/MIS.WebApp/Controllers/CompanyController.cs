using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;

    using ViewModels.Input.Company;

    public class CompanyController : AuthenticationController
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<MISUser> userManager;

        public CompanyController(ICompanyService companyService, UserManager<MISUser> userManager)
        {
            this.companyService = companyService;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            return this.View(new CreateCompanyInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.companyService.CreateAsync(input.Name, input.Address, userId);

            return this.RedirectToAction("Index" , "Home");
        }

        //TODO : DETAILS DELETE AND INVITE AND CHAT MAYBE
    }
}