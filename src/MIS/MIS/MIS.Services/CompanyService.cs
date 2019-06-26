namespace MIS.Services
{
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    public class CompanyService : ICompanyService
    {
        private readonly MISDbContext dbContext;
        private readonly IUserService userService;

        public CompanyService(MISDbContext dbContext, IUserService userService)
        {
            this.dbContext = dbContext;
            this.userService = userService;
        }

        public async Task<CompanyServiceModel> CreateAsync(string name, string address)
        {
            var company = new Company()
            {
                Address = address,
                Name = name,
            };

            this.dbContext.Add(company);
            await this.dbContext.SaveChangesAsync();

            var result = new CompanyServiceModel()
            {
                Address = company.Address,
                Name = company.Name,
                Id = company.Id
            };

            return result;
        }

        public async Task<CompanyServiceModel> AddToCompanyAsync(string name, string username)
        {
            var company = await this.dbContext.Companies.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Name == name);
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
            
            company.Employees.Add(user);
            await this.dbContext.SaveChangesAsync();

            return  new CompanyServiceModel()
            {
                Address = company.Address,
                Employees = company.Employees,
                Name = company.Name,
            };
        }

        public async Task<CompanyServiceModel> CreateAsync(string name, string address, string username)
        {
            var user = await this.userService.GetUserByUsernameAsync(username);
            var company = new Company()
            {
                Address = address,
                Name = name
            };

            company.Employees.Add(user);
            await this.dbContext.AddAsync(company);
            await this.dbContext.SaveChangesAsync();

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> GetByUserAsync(MISUser user)
        {
            var company = await this.dbContext.Companies
                                    .Include(x => x.WareHouses)
                                    .Include(x => x.Employees)
                                    .FirstOrDefaultAsync(x => x.Employees.Contains(user));

            var serviceModel = company.MapTo<CompanyServiceModel>();

            return serviceModel;
        }
    }
}