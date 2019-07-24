namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    public class ReportService : IReportService
    {
        private readonly MISDbContext dbContext;

        public ReportService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<IEnumerable<ReportServiceModel>> GetAllReportsAsync(int companyId)
        {
            var reports = await this.dbContext.Reports
                                    .Where(x => x.CompanyId == companyId)
                                    .To<ReportServiceModel>()
                                    .ToListAsync();
            return reports;
        }

        public async Task<ReportServiceModel> CreateAsync(int companyId, string name, DateTime from, DateTime to, MISUser user)
        {
            var company = await this.dbContext
                               .Companies
                               .FirstOrDefaultAsync(x => x.Id == companyId);

            var report = new Report
            {
                Name = name,
                From = from,
                To = to,
                Company = company,
                User = user,
            };

            var receipts = await this.dbContext.Receipts
                .Where(x => x.CompanyId == companyId)
                .Where(x => x.IssuedOn >= from && x.IssuedOn <= to)
                .ToListAsync();

            foreach (var receipt in receipts)
            {
                report.ReceiptReports.Add(new ReceiptReport()
                {
                    Receipt = receipt
                });
            }

            await this.dbContext.AddAsync(report);
            await this.dbContext.SaveChangesAsync();

            return report.MapTo<ReportServiceModel>();
        }
    }
}