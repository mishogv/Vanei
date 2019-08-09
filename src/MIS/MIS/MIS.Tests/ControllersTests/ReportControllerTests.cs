namespace MIS.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using Services;

    using ViewModels.Input.Report;
    using ViewModels.View.Report;

    using WebApp.Controllers;

    public class ReportControllerTests : BaseControllerTests
    {
        private MISDbContext dbContext;
        private UserManager<MISUser> userManager;
        private IReportService reportService;

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            var product = new Product()
            {
                Name = "testProduct",
                BarCode = "testBarcode",
                Price = 2.4m,
                Quantity = 3,
            };

            await this.dbContext.AddAsync(new MISUser()
            {
                UserName = "testUser",
                Company = new Company
                {
                    Name = "test",
                    Address = "test",
                    WareHouses = new List<WareHouse>()
                    {
                        new WareHouse()
                        {
                            Name = "testWareHouse",
                            IsFavorite = true,
                            Categories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "testCategoryName",
                                    Products = new List<Product>()
                                    {
                                        product
                                    }
                                }
                            },
                            Products = new List<Product>()
                            {
                                product
                            }
                        }
                    },
                },
                Receipts = new List<Receipt>()
                {
                    new Receipt()
                    {
                        IssuedOn = DateTime.UtcNow
                    },
                    new Receipt()
                },
                Reports = new List<Report>()
                {
                    new Report()
                    {
                        Name = "testReport",
                        From = DateTime.UtcNow.AddDays(-3),
                        To = DateTime.UtcNow.AddDays(-3)
                    }
                }
            });

            await this.dbContext.SaveChangesAsync();


            var store = new Mock<IUserStore<MISUser>>();
            var mgr = new Mock<UserManager<MISUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<MISUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<MISUser>());

            mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
               .Returns((ClaimsPrincipal x) => this.dbContext.Users.Include(c => c.Company).FirstOrDefaultAsync(z => z.UserName == x.Identity.Name));

            mgr.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
               .Returns((ClaimsPrincipal x) => this.dbContext.Users.Include(c => c.Company).FirstOrDefaultAsync(z => z.UserName == x.Identity.Name).GetAwaiter().GetResult()?.Id);

            this.userManager = mgr.Object;

            var userService = new UserService(this.dbContext);
            var companyService = new CompanyService(this.dbContext, userService);
            var categoryService = new CategoryService(new WareHouseService(this.dbContext, companyService), this.dbContext);
            var productService = new ProductService(this.dbContext, categoryService);
            var receiptService = new ReceiptService(this.dbContext, userService, companyService, productService);
            this.reportService = new ReportService(this.dbContext, companyService, receiptService);
        }

        [Test]
        public void Index_WithValidData_ShouldReturnView()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Index())
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<ReportIndexViewModel>(x.Model));

        [Test]
        public void Index_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("invalidUser")
               .Calling(x => x.Index())
               .ShouldReturn()
               .RedirectToAction("Create", "Company");

        [Test]
        public void Create_ShouldReturnView()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .Calling(x => x.Create())
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<ReportCreateInputModel>(x.Model))
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsNotNull(x.Model));

        [Test]
        public void Create_WithValidData_ShouldReturnRedirectToAction()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Create(new ReportCreateInputModel()
               {
                   Name = "validName",
                   From = DateTime.UtcNow.AddDays(-3),
                   To = DateTime.UtcNow.AddDays(3),
               }))
               .ShouldReturn()
               .RedirectToAction("Index");

        [Test]
        public void Create_WithInvalidData_ShouldReturnViewWithInvalidModelState()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Create(new ReportCreateInputModel()
               {
                   Name = null,
                   From = DateTime.UtcNow.AddDays(-3),
                   To = DateTime.UtcNow.AddDays(3),
               }))
               .ShouldHave()
               .InvalidModelState(1)
               .AndAlso()
               .ShouldReturn()
               .View()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<ReportCreateInputModel>(x.Model))
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsNotNull(x.Model));

        [Test]
        public void Details_WithValidData_ShouldReturnView()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Details(this.dbContext.Reports.FirstOrDefault().Id))
               .ShouldReturn()
               .View()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<ReportDetailsViewModel>(x.Model))
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsNotNull(x.Model));

        [Test]
        public void Details_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Details("invalidId"))
               .ShouldReturn()
               .RedirectToAction("Create");

        [Test]
        public void Delete_WithValidData_ShouldReturnRedirectToAction()
            => MyController<ReportController>
               .Instance()
               .WithDependencies(
                   this.reportService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Delete(this.dbContext.Reports.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index");
    }
}