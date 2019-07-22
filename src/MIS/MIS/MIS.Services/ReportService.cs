namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using ServicesModels;

    public class ReportService : IReportService
    {
        private readonly MISDbContext dbContext;

        public ReportService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ReportServiceModel> GetAllReportsByCompanyName(string companyName)
        {
            var reports = this.dbContext.Reports
                              .Include(x => x.Company)
                              .Where(x => x.Company.Name == companyName)
                              .To<ReportServiceModel>();

            return reports.ToList();
        }
    }
}