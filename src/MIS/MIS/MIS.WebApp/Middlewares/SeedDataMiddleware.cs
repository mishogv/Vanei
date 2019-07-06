namespace MIS.WebApp.Middlewares
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using Models;
    using Models.Enums;

    using Services;

    public class SeedDataMiddleware
    {
        private readonly RequestDelegate _next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<MISUser> userManager,
            ICompanyService companyService,
            RoleManager<IdentityRole> roleManager, MISDbContext db)
        {
            SeedRoles(roleManager).GetAwaiter().GetResult();

            SeedUserInRoles(userManager, companyService).GetAwaiter().GetResult();

            await _next(context);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(GlobalConstants.AdministratorAreaRole))
            {
                await roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdministratorAreaRole));
            }
        }

        private static async Task SeedUserInRoles(UserManager<MISUser> userManager, ICompanyService companyService)
        {
            if (!userManager.Users.Any())
            {
                var user = new MISUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "AdminFirstName",
                    LastName = "AdminLastName",
                    EmailConfirmed = true,
                    PhoneNumber = "0882713999",
                    Role = CompanyRole.Owner
                };

                var company = await companyService.CreateAsync("MIS EOOD", "Studentski grad Mandja street No 332");
                
                var password = "123456";
                user.CompanyId = company.Id;

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorAreaRole);
                }
            }
        }
    }
}