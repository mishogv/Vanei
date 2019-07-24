namespace MIS.WebApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Report;
    using ViewModels.View.Report;

    public class ReportController : AuthenticationController
    {
        private readonly IReportService reportService;
        private readonly UserManager<MISUser> userManager;

        public ReportController(IReportService reportService, UserManager<MISUser> userManager)
        {
            this.reportService = reportService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CompanyId == null)
            {
                return this.RedirectToAction("Create", "Company");
            }

            var reports = await this.reportService.GetAllReportsAsync((int)user.CompanyId);
            var result = new IndexReportViewModel()
            {
                Reports = reports.Select(x => x.MapTo<IndexReportShowViewModel>())
            };

            return this.View(result);
        }

        public IActionResult Create()
        {
            return this.View(new CreateReportInputModel() { From = DateTime.UtcNow, To = DateTime.UtcNow});
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReportInputModel input)
        {
            //TODO : SECURITY
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CompanyId == null)
            {
                return this.RedirectToAction("Create", "Company");
            }

            var report = await this.reportService.CreateAsync((int)user.CompanyId, input.Name, input.From, input.To, user);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var report = await this.reportService.GetReportAsync(id);

            var result = new DetailsReportViewModel()
            {
                Id = report.Id,
                Name = report.Name,
                From = report.From,
                To = report.To,
                Username = report.User.UserName,
                Receipts = report.ReceiptReports.Select(x => x.Receipt).Select(x => x.MapTo<ShowReceiptReportViewModel>()),
            };

            return this.View(result);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.reportService.DeleteReportAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}