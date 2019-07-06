namespace MIS.WebApp
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Data;

    using Microsoft.AspNetCore.Identity.UI.Services;

    using Middlewares;

    using Models;

    using reCAPTCHA.AspNetCore;

    using Services;
    using Services.Mapping;

    using ServicesModels;

    using ViewModels.View.AdministratorManage;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MISDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<MISUser, IdentityRole>(
                        options =>
                        {
                            options.SignIn.RequireConfirmedEmail = true;
                            options.Password.RequireDigit = false;
                            options.Password.RequireLowercase = false;
                            options.Password.RequireNonAlphanumeric = false;
                            options.Password.RequireUppercase = false;
                            options.Password.RequiredUniqueChars = 0;
                            options.Password.RequiredLength = 6;
                        })
                    .AddEntityFrameworkStores<MISDbContext>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI(UIFramework.Bootstrap4);

            #region Identity services

            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<UserManager<MISUser>>();

            #endregion

            services.AddAuthentication()
                    .AddFacebook(options =>
                    {
                        options.AppId = this.Configuration["Facebook:Key"];
                        options.AppSecret = this.Configuration["Facebook:Secret"];
                    });

            #region Custom services

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IWareHouseService, WareHouseService>();
            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<RecaptchaSettings>(this.Configuration.GetSection("Google"));
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            #endregion

            services.AddRouting();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(AdministratorShowUserViewModel).GetTypeInfo().Assembly, 
                typeof(CompanyServiceModel).GetTypeInfo().Assembly);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions()
                {
                    SourceCodeLineCount = 150,
                });
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSeedDataMiddleware();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
