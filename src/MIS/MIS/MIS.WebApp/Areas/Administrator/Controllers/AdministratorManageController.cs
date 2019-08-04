namespace MIS.WebApp.Areas.Administrator.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using Services;

    using ViewModels.View.AdministratorManage;

    public class AdministratorManageController : AdministratorController
    {
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;

        public AdministratorManageController(UserManager<MISUser> userManager,
            ICompanyService companyService)
        {
            this.userManager = userManager;
            this.companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var dict = new Dictionary<MISUser, string>();
            var users = this.userManager.Users
                            .Include(x => x.Company)
                            .ToList();

            foreach (var user in users)
            {
                var roles = await this.userManager.GetRolesAsync(user);
                var rolesToAdd = roles.Count == 0 ? GlobalConstants.UserRoleName : string.Join(", ", roles);
                dict.Add(user, rolesToAdd);
            }

            var result = MapUserViewModels(dict);

            return this.View(result);
        }


        public async Task<IActionResult> Create(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || user.UserName == GlobalConstants.RootAdminName)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorAreaRole);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Remove(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || user.UserName == GlobalConstants.RootAdminName)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorAreaRole);

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

        private static List<AdministratorShowUserViewModel> MapUserViewModels(Dictionary<MISUser, string> dict)
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