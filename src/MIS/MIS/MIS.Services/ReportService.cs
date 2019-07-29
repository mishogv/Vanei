namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common.Extensions;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    public class ReportService : IReportService
    {
        private readonly MISDbContext dbContext;
        private readonly ICompanyService companyService;
        private readonly IReceiptService receiptService;

        public ReportService(MISDbContext dbContext, ICompanyService companyService, IReceiptService receiptService)
        {
            this.dbContext = dbContext;
            this.companyService = companyService;
            this.receiptService = receiptService;
        }


        public async Task<IEnumerable<ReportServiceModel>> GetAllReportsAsync(int companyId)
        {
            var reports = await this.dbContext.Reports
                                    .Where(x => x.CompanyId == companyId)
                                    .To<ReportServiceModel>()
                                    .ToListAsync();
            return reports;
        }

        public async Task<ReportServiceModel> GetReportAsync(int id)
        {
            var report = await this.dbContext.Reports
                                   .Include(x => x.User)
                                   .Include(x => x.ReceiptReports)
                                   .ThenInclude(x => x.Receipt)
                                   .FirstOrDefaultAsync(x => x.Id == id);

            report.ThrowIfNull(nameof(report));

            return report.MapTo<ReportServiceModel>();
        }

        public async Task<ReportServiceModel> DeleteReportAsync(int id)
        {
            var report = await this.dbContext.Reports
                                   .Include(x => x.ReceiptReports)
                                   .FirstOrDefaultAsync(x => x.Id == id);

            report.ThrowIfNull(nameof(report));

            this.dbContext.RemoveRange(report.ReceiptReports);

            this.dbContext.Reports.Remove(report);
            await this.dbContext.SaveChangesAsync();

            return report.MapTo<ReportServiceModel>();
        }

        public async Task<ReportServiceModel> CreateAsync(int companyId, string name, DateTime from, DateTime to, MISUser user)
        {
            var report = new Report
            {
                Name = name,
                From = from,
                To = to,
                User = user,
            };

            await this.companyService.SetCompanyAsync(report, companyId);
            await this.receiptService.SetReceiptsAsync(report, from, to, companyId);           

            await this.dbContext.AddAsync(report);
            await this.dbContext.SaveChangesAsync();

            return report.MapTo<ReportServiceModel>();
        }
    }
}