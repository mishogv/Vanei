namespace MIS.Data.Seeding
{
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;

    public class RolesSeeder : ISeeder
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesSeeder(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!await this.roleManager.RoleExistsAsync(GlobalConstants.AdministratorAreaRole))
            {
                await this.roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdministratorAreaRole));
            }

            if (!await this.roleManager.RoleExistsAsync(GlobalConstants.CompanyOwnerRole))
            {
                await this.roleManager.CreateAsync(new IdentityRole(GlobalConstants.CompanyOwnerRole));
            }
        }
    }
}