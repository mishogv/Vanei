namespace MIS.WebApp.Areas.Administrator.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;

    using ViewModels.View.AdministratorManage;

    public class AdministratorManageController : AdministratorController
    {
        private readonly IAdministratorService administratorService;
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;

        public AdministratorManageController(IAdministratorService administratorService,
            UserManager<MISUser> userManager,
            ICompanyService companyService)
        {
            this.administratorService = administratorService;
            this.userManager = userManager;
            this.companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var dict = new Dictionary<MISUser, string>();
            var users = this.administratorService
                            .GetAllUsers()
                            .ToList();

            foreach (var user in users)
            {
                var roles = await this.userManager.GetRolesAsync(user);
                var rolesToAdd = roles.Count == 0 ? GlobalConstants.UserRoleName : string.Join(", ", roles);
                dict.Add(user, rolesToAdd);
            }

            var result = MapModels(dict);

            return this.View(result);
        }


        public async Task<IActionResult> Create(string id)
        {
            var result = await this.administratorService.CreateAdministratorByIdAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Remove(string id)
        {
            var currentUserId = this.userManager.GetUserId(this.User);

            if (currentUserId == id)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var result = await this.administratorService.RemoveAdministratorByIdAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var rootAdmin = await this.userManager.FindByNameAsync(GlobalConstants.RootAdminName);

            if (id == rootAdmin.CompanyId)
            {
                return this.Forbid();
            }

            var result = await this.companyService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        private static List<AdministratorShowUserViewModel> MapModels(Dictionary<MISUser, string> dict)
        {
            var result = new List<AdministratorShowUserViewModel>();

            foreach (var kvp in dict)
            {
                result.Add(new AdministratorShowUserViewModel()
                {
                    Id = kvp.Key.Id,
                    CompanyId = kvp.Key.CompanyId,
                    LastName = kvp.Key.LastName,
                    FirstName = kvp.Key.FirstName,
                    Email = kvp.Key.Email,
                    Role = kvp.Value,
                    PhoneNumber = kvp.Key.PhoneNumber ?? GlobalConstants.NotApplicable,
                    CompanyName = kvp.Key.Company == null ? GlobalConstants.NotApplicable : kvp.Key.Company.Name,
                    Username = kvp.Key.UserName
                });
            }

            return result;
        }
    }
}