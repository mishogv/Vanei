namespace MIS.Tests.ControllersTests
{
    using Data.Seeding;

    using Ganss.XSS;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using reCAPTCHA.AspNetCore;

    using Services;

    using WebApp;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var productServiceMock = new Mock<IProductService>();

            services.Replace<IProductService, ProductService>(ServiceLifetime.Scoped);
            services.Replace<ICategoryService, CategoryService>(ServiceLifetime.Scoped);
            services.Replace<ICompanyService, CompanyService>(ServiceLifetime.Scoped);
            services.Replace<IReportService, ReportService>(ServiceLifetime.Scoped);
            services.Replace<IReceiptService, ReceiptService>(ServiceLifetime.Scoped);
            services.Replace<IWareHouseService, WareHouseService>(ServiceLifetime.Scoped);
            services.Replace<IInvitationService, InvitationService>(ServiceLifetime.Scoped);
            services.Replace<IMessageService, MessageService>(ServiceLifetime.Scoped);
            services.Replace<IUserService, UserService>(ServiceLifetime.Scoped);
            services.Replace<IHtmlSanitizer, HtmlSanitizer>(ServiceLifetime.Scoped);
            services.Replace<RolesSeeder>(x => x.GetRequiredService<RolesSeeder>(), ServiceLifetime.Scoped);
            services.Replace<RootAdminSeeder>(x => x.GetRequiredService<RootAdminSeeder>(), ServiceLifetime.Scoped);
            services.Replace<IEmailSender, EmailSender>(ServiceLifetime.Transient);

            services.Configure<RecaptchaSettings>(this.Configuration.GetSection("Google"));
            services.Replace<IRecaptchaService, RecaptchaService>(ServiceLifetime.Scoped);
        }
    }
}