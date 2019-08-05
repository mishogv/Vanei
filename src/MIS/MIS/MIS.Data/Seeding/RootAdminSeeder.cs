namespace MIS.Data.Seeding
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;

    using Models;

    public class RootAdminSeeder : ISeeder
    {
        private const string RootAdminEmail = "admin@gmail.com";
        private const string RootAdminFirstName = "AdminFirstName";
        private const string RootAdminLastName = "AdminLastName";
        private const string RootAdminPhoneNumber = "0882713999";
        private const string RootAdminCompanyName = "Root";
        private const string RootAdminCompanyAddress = "RootAddress";
        private const string RootAdminPassword = "123456";

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
                    UserName = GlobalConstants.RootAdminName,
                    Email = RootAdminEmail,
                    FirstName = RootAdminFirstName,
                    LastName = RootAdminLastName,
                    EmailConfirmed = true,
                    PhoneNumber = RootAdminPhoneNumber,
                };

                var company = new Company()
                {
                    Name = RootAdminCompanyName,
                    Address = RootAdminCompanyAddress
                };

                await this.context.AddAsync(company);
                await this.context.SaveChangesAsync();

                var password = RootAdminPassword;
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