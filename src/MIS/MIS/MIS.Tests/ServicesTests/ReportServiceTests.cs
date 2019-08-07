namespace MIS.Tests.ServicesTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using NUnit.Framework;

    using Services;

    public class ReportServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private ReportService reportService;

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            var userService = new UserService(this.dbContext);
            var companyService = new CompanyService(this.dbContext, userService);
            var warehouseService = new WareHouseService(this.dbContext, companyService);
            var categoryService = new CategoryService(warehouseService, this.dbContext);
            var productService = new ProductService(this.dbContext, categoryService);

            var receiptService = new ReceiptService(this.dbContext, userService, companyService, productService);


            this.reportService = new ReportService(this.dbContext, companyService, receiptService);


            var company = new Company()
            {
                Name = "asd",
                Address = "asd",
            };

            var warehouse = new WareHouse()
            {
                Name = "asd",
                Company = company,
            };

            var category = new Category()
            {
                Name = "asd",
                WareHouse = warehouse
            };

            var user = new MISUser()
            {
                UserName = "asd",
                FirstName = "asd",
                LastName = "asd",
                Company = company
            };

            await this.dbContext.AddAsync(company);
            await this.dbContext.AddAsync(warehouse);
            await this.dbContext.AddAsync(category);
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task CreateReport_WithValidData_ShouldReturnCorrectReport()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(),
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.reportService.CreateAsync(user.CompanyId, "Valid", DateTime.UtcNow.AddDays(-5),
                DateTime.UtcNow.AddDays(5), user);

            var expected = await this.dbContext.Reports.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task CreateReport_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(),
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.reportService.CreateAsync("invalid", "Valid", DateTime.UtcNow.AddDays(-5),
                             DateTime.UtcNow.AddDays(5), user);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetAllReports_WithValidData_ShouldReturnCorrectReports()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = company,
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var first = await this.reportService.CreateAsync(user.CompanyId, "Valid1", DateTime.UtcNow.AddDays(-5),
                             DateTime.UtcNow.AddDays(5), user);

            var second = await this.reportService.CreateAsync(user.CompanyId, "Valid2", DateTime.UtcNow.AddDays(-5),
                             DateTime.UtcNow.AddDays(5), user);

            var third = await this.reportService.CreateAsync(user.CompanyId, "Valid3", DateTime.UtcNow.AddDays(-5),
                             DateTime.UtcNow.AddDays(5), user);

            var actual = await this.reportService.GetAllReportsAsync(company.Id);
            var actualArray = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(first.Id, actualArray[0].Id);
            Assert.AreEqual(second.Id, actualArray[1].Id);
            Assert.AreEqual(third.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllReports_WithInvalidData_ShouldReturnEmptyCollection()
        {
            var actual = await this.reportService.GetAllReportsAsync("invalidId");

            Assert.IsEmpty(actual);
        }

        [Test]
        public async Task GetReport_ShouldReturn_CorrectReport()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(),
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.reportService.CreateAsync(user.CompanyId, "Valid", DateTime.UtcNow.AddDays(-5),
                             DateTime.UtcNow.AddDays(5), user);
            var actual = await this.reportService.GetReportAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetReport_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(),
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.reportService.CreateAsync(user.CompanyId, "Valid", DateTime.UtcNow.AddDays(-5),
                               DateTime.UtcNow.AddDays(5), user);
            var actual = await this.reportService.GetReportAsync("invalidId");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteReport_WithValidData_ShouldReturnCorrectReport()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(),
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.reportService.CreateAsync(user.CompanyId, "Valid", DateTime.UtcNow.AddDays(-5),
                               DateTime.UtcNow.AddDays(5), user);

            var actual = await this.reportService.DeleteReportAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task DeleteReport_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var receipt = new Receipt()
            {
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(),
                IssuedOn = null,
                User = user
            };

            await this.dbContext.AddAsync(receipt);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.reportService.CreateAsync(user.CompanyId, "Valid", DateTime.UtcNow.AddDays(-5),
                               DateTime.UtcNow.AddDays(5), user);

            var actual = await this.reportService.DeleteReportAsync("invalidId");

            Assert.IsNull(actual);
        }
    }
}