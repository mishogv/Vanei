namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    using ServicesModels;

    public interface IReportService
    {
        Task<IEnumerable<ReportServiceModel>> GetAllReportsAsync(int companyId);

        Task<ReportServiceModel> CreateAsync(int companyId, string name, DateTime from, DateTime to, MISUser user);
    }
}