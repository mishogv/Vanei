namespace MIS.Tests
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
        private const string CompanyName = "Microsoft";
        private const string WarehouseName = "MicrosoftWarehouse";
        private const string CompanyAddress = "MicrosoftStreet";

        [Test]
        public async Task CreateWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var actual = await warehouseService.CreateAsync(WarehouseName, company?.Id);
            var expected = await dbContext.WareHouses.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task CreateWarehouse_ShouldReturn_NullWithInvalidCompanyId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);


            var actual = await warehouseService.CreateAsync(WarehouseName, "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await warehouseService.GetWareHouseAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetWarehouse_ShouldReturn_NullWithWrongId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var actual = await warehouseService.GetWareHouseAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await warehouseService.DeleteAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }


        [Test]
        public async Task DeleteWarehouse_ShouldReturn_NullWithWrongId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await warehouseService.DeleteAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task EditWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await warehouseService.EditAsync(expected.Id, "asd");

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("asd", actual.Name);
        }

        [Test]
        public async Task EditWarehouse_ShouldReturn_NullWithWrongId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            await warehouseService.CreateAsync(WarehouseName, company?.Id);
            var actual = await warehouseService.EditAsync("asd", WarehouseName);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task MakeFavoriteWarehouse_ShouldReturn_CorrectWarehouse()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company.Id);
            var actual = await warehouseService.MakeFavoriteAsync(expected.Id, company.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(true, actual.IsFavorite);
        }

        [Test]
        public async Task MakeFavoriteWarehouse_ShouldReturn_NullWithWrongId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company.Id);
            var actual = await warehouseService.MakeFavoriteAsync("asd", company.Id);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task AddCategory_ShouldReturn_TrueWithCorrectData()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company.Id);

            var category = new Category
            {
                Name = "CategoryName",
            };

            var result = await warehouseService.AddCategoryAsync(category, expected.Id);

            Assert.AreEqual(expected.Id, category.WareHouse.Id);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddCategory_ShouldReturn_FalseAndSetNullWithInvalidData()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expected = await warehouseService.CreateAsync(WarehouseName, company.Id);

            var category = new Category
            {
                Name = "CategoryName",
            };

            var result = await warehouseService.AddCategoryAsync(category, "asd");

            Assert.IsNull(category.WareHouse);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetWarehouses_ShouldReturn_CorrectWarehouses()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expectedFirst = await warehouseService.CreateAsync(WarehouseName + 1, company.Id);
            var expectedSecond = await warehouseService.CreateAsync(WarehouseName + 2, company.Id);
            var expectedThird = await warehouseService.CreateAsync(WarehouseName + 3, company.Id);

            var actual = await warehouseService.GetWarehousesByCompanyIdAsync(company.Id);
            var actualArr = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArr[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArr[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArr[2].Id);
        }

        [Test]
        public async Task GetWarehouses_ShouldReturn_EmptyCollection()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);

            var company = await dbContext.Companies.FirstOrDefaultAsync();

            var expectedFirst = await warehouseService.CreateAsync(WarehouseName + 1, company.Id);
            var expectedSecond = await warehouseService.CreateAsync(WarehouseName + 2, company.Id);
            var expectedThird = await warehouseService.CreateAsync(WarehouseName + 3, company.Id);

            var actual = await warehouseService.GetWarehousesByCompanyIdAsync("asd");

            Assert.IsEmpty(actual);
        }


        private MISDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            var dbContext = new MISDbContext(options);

            return dbContext;
        }

        private async Task<IWareHouseService> GetWarehouseService(MISDbContext dbContext)
        {
            var userService = new UserService(dbContext);
            var companyService = new CompanyService(dbContext, userService);
            await companyService.CreateAsync(CompanyName, CompanyAddress);

            var warehouse = new WareHouseService(dbContext, companyService);

            return warehouse;
        }
    }
}