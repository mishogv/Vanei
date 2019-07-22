namespace MIS.WebApp.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Report;
    using ViewModels.View.Report;

    public class ReportController : AuthenticationController
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public IActionResult Index(string companyName)
        {
            //list all reports with link foreach report details for company
            //TODO : SECURITY 

            var reports = this.reportService.GetAllReportsByCompanyName(companyName);
            var result = reports.Select(x => x.MapTo<IndexReportViewModel>());

            return this.View(result);
        }

        public IActionResult Create(string companyName)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateReportInputModel input)
        {
            //TODO : SECURITY 

            //var reports = this.reportService.GetAllReportsByCompanyName(companyName);
            //var result = reports.Select(x => x.MapTo<IndexReportViewModel>());

            return this.View();
        }
    }
}