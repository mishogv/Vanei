namespace Vanei.WebApp.Services
{
    using System.Threading.Tasks;

    using Contracts;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    public class CompanyService : ICompanyService
    {
        private readonly VaneiDbContext dbContext;

        public CompanyService(VaneiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateCompanyAsync(params string[] args)
        {
            var company = this.ParseCompanyParams(args);

            await this.dbContext.Companies.AddAsync(company);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCompanyAsync(WebAppUser user, string companyName)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Name == companyName);

            if (user.Id != company?.OwnerId || company == null)
            {
                return false;
            }

            this.dbContext.Companies.Remove(company);

            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCompanyAsync(WebAppUser user, int id)
        {
            var company = await this.dbContext.Companies.FindAsync();

            if (user.Id != company.OwnerId)
            {
                return false;
            }

            this.dbContext.Companies.Remove(company);

            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCompanyAsync(WebAppUser user, Company company)
        {
            if (user.Id != company.OwnerId)
            {
                return false;
            }

            this.dbContext.Companies.Remove(company);

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        private Company ParseCompanyParams(string[] args)
        {
            var name = args[0];
            var address = args[1];
            var ownerId = args[2];
            var wareHouseId = int.Parse(args[3]);

            var company = new Company()
            {
                Name = name,
                Address = address,
                OwnerId = ownerId,
                WareHouseId = wareHouseId
            };

            return company;
        }
    }
}