namespace MIS.Data.Seeding
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;

    using Models;

    public class RootAdminSeeder : ISeeder
    {
        private readonly UserManager<MISUser> userManager;
        private readonly MISDbContext context;

        public RootAdminSeeder(UserManager<MISUser> userManager, MISDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task Seed()
        {
            if (!this.userManager.Users.Any())
            {
                var user = new MISUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "AdminFirstName",
                    LastName = "AdminLastName",
                    EmailConfirmed = true,
                    PhoneNumber = "0882713999",
                };

                var company = new Company()
                {
                    Name = "Root",
                    Address = "RootAddress"
                };

                await this.context.AddAsync(company);
                await this.context.SaveChangesAsync();

                var password = "123456";
                user.Company = company;

                var result = await this.userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorAreaRole);
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.CompanyOwnerRole);
                }
            }
        }
    }
}