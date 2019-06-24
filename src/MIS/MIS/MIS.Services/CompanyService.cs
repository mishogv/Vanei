namespace MIS.Services
{
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;
    using Models.Enums;

    using ServicesModels;

    public class CompanyService : ICompanyService
    {
        private readonly MISDbContext dbContext;

        public CompanyService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CompanyServiceModel> CreateAsync(string name, string address, string ownerId)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == ownerId);

            if (user == null)
            {
                return null;
            }

            var company = new Company()
            {
                Address = address,
                Name = name,
                OwnerId = ownerId,
            };

            this.dbContext.Add(company);
            await this.dbContext.SaveChangesAsync();

            user.CompanyId = company.Id;
            user.Role = CompanyRole.Owner;

            this.dbContext.Update(user);
            await this.dbContext.SaveChangesAsync();

            var result = new CompanyServiceModel()
            {
                Address = company.Address,
                Name = company.Name,
                OwnerId = company.OwnerId,
                Owner = user
            };

            return result;
        }
    }
}