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

    public class WarehouseServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private IWareHouseService warehouseService;

        private const string CompanyName = "Microsoft";
        private const string WarehouseName = "MicrosoftWarehouse";
        private const string CompanyAddress = "MicrosoftStreet";

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);
            var userService = new UserService(this.dbContext);
            var companyService = new CompanyService(this.dbContext, userService);
            var company = await companyService.CreateAsync(CompanyName, CompanyAddress);
            this.warehouseService = new WareHouseService(this.dbContext, companyService);
        }

        [Test]
        public async Task CreateWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var actual = await this.warehouseService.CreateAsync(WarehouseName, company?.Id);
            var expected = await this.dbContext.WareHouses.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task CreateWarehouse_ShouldReturn_NullWithInvalidCompanyId()
        {
            var actual = await this.warehouseService.CreateAsync(WarehouseName, "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await this.warehouseService.GetWareHouseAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetWarehouse_ShouldReturn_NullWithWrongId()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var actual = await this.warehouseService.GetWareHouseAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await this.warehouseService.DeleteAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task DeleteWarehouse_ShouldReturn_NullWithWrongId()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await this.warehouseService.DeleteAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task EditWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await this.warehouseService.EditAsync(expected.Id, "asd");

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("asd", actual.Name);
        }

        [Test]
        public async Task EditWarehouse_ShouldReturn_NullWithWrongId()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            await this.warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await this.warehouseService.EditAsync("asd", WarehouseName);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task MakeFavoriteWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company.Id);
            var actual = await this.warehouseService.MakeFavoriteAsync(expected.Id, company.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(true, actual.IsFavorite);
        }

        [Test]
        public async Task MakeFavoriteWarehouse_ShouldReturn_NullWithWrongId()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company.Id);
            var actual = await this.warehouseService.MakeFavoriteAsync("asd", company.Id);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task AddCategory_ShouldReturn_TrueWithCorrectData()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company.Id);

            var category = new Category {Name = "CategoryName",};

            var result = await this.warehouseService.AddCategoryAsync(category, expected.Id);

            Assert.AreEqual(expected.Id, category.WareHouse.Id);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddCategory_ShouldReturn_FalseAndSetNullWithInvalidData()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expected = await this.warehouseService.CreateAsync(WarehouseName, company.Id);

            var category = new Category {Name = "CategoryName",};

            var result = await this.warehouseService.AddCategoryAsync(category, "asd");

            Assert.IsNull(category.WareHouse);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetWarehouses_ShouldReturn_CorrectWarehouses()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expectedFirst = await this.warehouseService.CreateAsync(WarehouseName + 1, company.Id);
            var expectedSecond = await this.warehouseService.CreateAsync(WarehouseName + 2, company.Id);
            var expectedThird = await this.warehouseService.CreateAsync(WarehouseName + 3, company.Id);

            var actual = await this.warehouseService.GetWarehousesByCompanyIdAsync(company.Id);
            var actualArr = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArr[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArr[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArr[2].Id);
        }

        [Test]
        public async Task GetWarehouses_ShouldReturn_EmptyCollection()
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync();

            var expectedFirst = await this.warehouseService.CreateAsync(WarehouseName + 1, company.Id);
            var expectedSecond = await this.warehouseService.CreateAsync(WarehouseName + 2, company.Id);
            var expectedThird = await this.warehouseService.CreateAsync(WarehouseName + 3, company.Id);

            var actual = await this.warehouseService.GetWarehousesByCompanyIdAsync("asd");

            Assert.IsEmpty(actual);
        }
    }
}