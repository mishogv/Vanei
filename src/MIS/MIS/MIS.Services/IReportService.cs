namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    public interface IReportService
    {
        Task<IEnumerable<ReportServiceModel>> GetAllReportsAsync(string companyId);

        Task<ReportServiceModel> GetReportAsync(string id);

        Task<ReportServiceModel> DeleteReportAsync(string id);

        Task<ReportServiceModel> CreateAsync(string companyId, string name, DateTime from, DateTime to, MISUser user);
    }
}