namespace MIS.WebApp.Areas.Owner.Controllers
{
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Company;

    using WebApp.Controllers;

    [Area(GlobalConstants.CompanyOwnerRole)]
    [Authorize(Roles = GlobalConstants.CompanyOwnerRole)]
    public class CompanyController : AuthenticationController
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<MISUser> userManager;
        private readonly SignInManager<MISUser> signInManager;

        public CompanyController(ICompanyService companyService,
            UserManager<MISUser> userManager,
            SignInManager<MISUser> signInManager)
        {
            this.companyService = companyService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Edit(string id)
        {
            var company = await this.companyService.GetCompanyAsync(id);

            if (company == null)
            {
                return this.NotFound();
            }

            return this.View(company.MapTo<EditCompanyInputModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCompanyInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.companyService.EditAsync(input.Id, input.Name, input.Address);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.companyService.DeleteAsync(id);
            var user = await this.userManager.GetUserAsync(this.User);

            if (result == null)
            {
                return this.BadRequest();
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.CompanyOwnerRole);

            await this.signInManager.SignOutAsync();
            await this.signInManager.SignInAsync(user, false);

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveEmployee(string id)
        {
            await this.companyService.RemoveEmployeeAsync(id);

            return this.RedirectToAction("Index");
        }
    }
}