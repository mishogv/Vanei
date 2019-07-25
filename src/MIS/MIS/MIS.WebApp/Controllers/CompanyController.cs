using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Microsoft.AspNetCore.Identity;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Company;
    using ViewModels.View.Company;

    public class CompanyController : AuthenticationController
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<MISUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public CompanyController(ICompanyService companyService,
            UserManager<MISUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.companyService = companyService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CompanyId == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            var company = await this.companyService.GetCompanyAsync((int)user.CompanyId);

            var employees = company.Employees.Select(x => x.MapTo<DetailsCompanyUserViewModel>());

            var result = company.MapTo<DetailsCompanyViewModel>();

            return this.View(result);
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

            var user = await this.userManager.GetUserAsync(this.User);
            
            await this.userManager.AddToRoleAsync(user, GlobalConstants.CompanyOwnerRole);
            
            var result = await this.userManager.GetRolesAsync(user);

            await this.companyService.CreateAsync(input.Name, input.Address, user.Id);
            return this.RedirectToAction("Index", "Home");
        }
    }
}