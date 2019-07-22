namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    public interface IReportService
    {
        IEnumerable<ReportServiceModel> GetAllReportsByCompanyName(string companyName);
    }
}