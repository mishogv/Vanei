﻿namespace MIS.Services
{
    using System;
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

        public CompanyService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public async Task<CompanyServiceModel> EditAsync(int id, string name, string address)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);

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

            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            //TODO : if null
            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> RemoveEmployeeAsync(string id)
        {
            var employee = await this.dbContext.Users
                                     .Include(x => x.Company)
                                     .ThenInclude(x => x.Employees)
                                     .FirstOrDefaultAsync(x => x.Id == id);

            var company = employee.Company;

            company.Employees.Remove(employee);
            this.dbContext.Update(company);
            await this.dbContext.SaveChangesAsync();

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> SetCompanyAsync(Message message, int id)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);

            message.Company = company;

            return company.MapTo<CompanyServiceModel>();
        }

        public async Task<CompanyServiceModel> AddToCompanyAsync(string name, string username)
        {
            var company = await this.dbContext.Companies.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Name == name);
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            company.Employees.Add(user);
            await this.dbContext.SaveChangesAsync();

            return new CompanyServiceModel()
            {
                Address = company.Address,
                Employees = company.Employees,
                Name = company.Name,
            };
        }

        public async Task<CompanyServiceModel> CreateAsync(string name, string address, string userId)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
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
    }
}