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
        private const string RedirectCreate = "Create";
        private const string RedirectCompany = "Company";

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

            if (user?.CompanyId == null)
            {
                return this.RedirectToAction(RedirectCreate, RedirectCompany);
            }

            var reports = await this.reportService.GetAllReportsAsync(user.CompanyId);
            var result = new ReportIndexViewModel()
            {
                Reports = reports.MapTo<ReportIndexShowViewModel[]>(),
            };

            return this.View(result);
        }

        public IActionResult Create()
        {
            return this.View(new ReportCreateInputModel() { From = DateTime.UtcNow, To = DateTime.UtcNow});
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportCreateInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.reportService.CreateAsync(user.CompanyId, input.Name, input.From, input.To, user);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            var report = await this.reportService.GetReportAsync(id);

            if (report == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            var result = report.MapTo<ReportDetailsViewModel>();

            result.Receipts = report.ReceiptReports.Select(x => x.Receipt).MapTo<ReportShowReceiptViewModel[]>();

            return this.View(result);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.reportService.DeleteReportAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}