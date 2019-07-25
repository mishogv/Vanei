namespace MIS.WebApp.Areas.Owner.Controllers
{
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Company;

    using WebApp.Controllers;

    [Area(GlobalConstants.CompanyOwnerRole)]
    [Authorize(Roles = GlobalConstants.CompanyOwnerRole)]
    public class CompanyController : AuthenticationController
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await this.companyService.GetCompanyAsync(id);

            return this.View(company.MapTo<EditCompanyInputModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCompanyInputModel input)
        {
            //TODO : Security
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.companyService.EditAsync(input.Id, input.Name, input.Address);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.companyService.DeleteAsync(id);

            return this.RedirectToAction("Index", "Home");
        }
    }
}