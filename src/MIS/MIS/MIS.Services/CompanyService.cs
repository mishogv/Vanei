namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

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

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> EditAsync(int id, string name, string address)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return null;
            }

            company.Name = name;
            company.Address = address;

            this.dbContext.Update(company);
            await this.dbContext.SaveChangesAsync();

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> DeleteAsync(int id)
        {
            var company = await this.dbContext.Companies
                                    .Include(x => x.Receipts)
                                    .ThenInclude(x => x.ReceiptReports)
                                    .Include(x => x.Receipts)
                                    .ThenInclude(x => x.ReceiptProducts)
                                    .Include(x => x.Reports)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            var receipts = company.Receipts;

            this.dbContext.RemoveRange(receipts.SelectMany(x => x.ReceiptReports));
            this.dbContext.RemoveRange(receipts.SelectMany(x => x.ReceiptProducts));
            this.dbContext.RemoveRange(company.Receipts);
            this.dbContext.RemoveRange(company.Reports);
            this.dbContext.Remove(company);
            await this.dbContext.SaveChangesAsync();

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> GetCompanyAsync(int id)
        {
            var company = await this.dbContext.Companies
                                    .Include(x => x.Employees)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            return company?.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> RemoveEmployeeAsync(string id)
        {
            var employee = await this.dbContext.Companies
                                     .Include(x => x.Employees)
                                     .SelectMany(x => x.Employees)
                                     .FirstOrDefaultAsync(x => x.Id == id);

            var company = employee?.Company;

            if (company == null)
            {
                return null;
            }

            company.Employees.Remove(employee);
            this.dbContext.Update(company);
            await this.dbContext.SaveChangesAsync();

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> SetCompanyAsync(Message message, int id)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return null;
            }

            message.Company = company;

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> SetCompanyAsync(Report report, int id)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return null;
            }

            report.Company = company;

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> SetCompanyAsync(Invitation invitation, int id)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return null;
            }

            invitation.Company = company;

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> SetCompanyAsync(WareHouse wareHouse, int id)
        {
            var company = await this.dbContext
                                    .Companies
                                    .Include(x => x.WareHouses)
                                    .FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return null;
            }

            if (company.WareHouses.Count(x => x.IsFavorite) == 0)
            {
                wareHouse.IsFavorite = true;
            }

            company.WareHouses.Add(wareHouse);

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> CreateAsync(string name, string address, string userId)
        {
            var company = new Company()
            {
                Address = address,
                Name = name
            };

            await this.userService.AddToCompanyAsync(company, userId);

            await this.dbContext.AddAsync(company);
            await this.dbContext.SaveChangesAsync();

            return company.MapTo<CompanyServiceModel>();
        }
    }
}