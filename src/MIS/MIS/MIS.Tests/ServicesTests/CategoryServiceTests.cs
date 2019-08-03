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

    [TestFixture]
    public class CategoryServiceTests : BaseServiceTests
    {

        private const string CompanyName = "Microsoft";
        private const string WarehouseName = "MicrosoftWarehouse";
        private const string CategoryName = "MicrosoftCategory";
        private const string CompanyAddress = "MicrosoftStreet";

        [Test]
        public async Task CreateCategory_ShouldReturn_CorrectCategory()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var actual = await categoryService.CreateAsync(CategoryName, warehouse.Id);
            var categoryFromDb = await dbContext.Categories.FirstOrDefaultAsync();

            Assert.AreEqual(categoryFromDb.Id, actual.Id);
            Assert.AreEqual(categoryFromDb.WareHouseId, actual.WareHouseId);
        }

        [Test]
        public async Task CreateCategory_ShouldReturn_NullWithInvalidId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);

            var actual = await categoryService.CreateAsync(CategoryName, "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task EditCategory_ShouldReturn_CorrectCategoryWithEditedData()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await categoryService.EditAsync(expected.Id, "newName");


            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("newName", actual.Name);
        }

        [Test]
        public async Task EditCategory_ShouldReturn_NullWithInvalidId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await categoryService.EditAsync("asd", "newName");


            Assert.IsNull(actual);
        }


        [Test]
        public async Task DeleteCategory_ShouldReturn_CorrectCategory()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await categoryService.DeleteAsync(expected.Id);


            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task DeleteCategory_ShouldReturn_NullWithInvalidId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await categoryService.DeleteAsync("asd");


            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetCategory_ShouldReturn_CorrectCategory()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await categoryService.GetCategoryAsync(expected.Id);


            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetCategory_ShouldReturn_NullWithInvalidId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await categoryService.GetCategoryAsync("asd");


            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetCategory_ShouldSet_CorrectCategory()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await categoryService.CreateAsync(CategoryName, warehouse.Id);

            var product = new Product();

            await categoryService.SetCategoryAsync(product, expected.Id);


            Assert.AreEqual(expected.Id, product.Category.Id);
        }

        [Test]
        public async Task SetCategory_ShouldSet_NullWithInvalidId()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);

            var product = new Product();

            await categoryService.SetCategoryAsync(product, "asd");

            Assert.IsNull(product.Category);
        }

        [Test]
        public async Task GetAllCategoriesByCompanyId_ShouldReturn_CorrectCategories()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expectedFirst = await categoryService.CreateAsync(CategoryName + 1, warehouse.Id);
            var expectedSecond = await categoryService.CreateAsync(CategoryName + 2, warehouse.Id);
            var expectedThird = await categoryService.CreateAsync(CategoryName + 3, warehouse.Id);

            var actual = await categoryService.GetAllByCompanyIdAsync(warehouse.CompanyId);
            var actualArray = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllCategoriesByCompanyId_ShouldReturn_EmptyCollection()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expectedFirst = await categoryService.CreateAsync(CategoryName + 1, warehouse.Id);
            var expectedSecond = await categoryService.CreateAsync(CategoryName + 2, warehouse.Id);
            var expectedThird = await categoryService.CreateAsync(CategoryName + 3, warehouse.Id);

            var actual = await categoryService.GetAllByCompanyIdAsync("asd");

            Assert.IsEmpty(actual);
        }

        [Test]
        public async Task GetAllCategoriesByWarehouseId_ShouldReturn_CorrectCategories()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expectedFirst = await categoryService.CreateAsync(CategoryName + 1, warehouse.Id);
            var expectedSecond = await categoryService.CreateAsync(CategoryName + 2, warehouse.Id);
            var expectedThird = await categoryService.CreateAsync(CategoryName + 3, warehouse.Id);

            var actual = await categoryService.GetAllCategoriesAsync(warehouse.Id);
            var actualArray = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllCategoriesByWarehouseId_ShouldReturn_EmptyCollection()
        {
            var dbContext = this.GetDbContext();
            var warehouseService = await this.GetWarehouseService(dbContext);
            var categoryService = new CategoryService(warehouseService, dbContext);
            var warehouse = await dbContext.WareHouses.FirstOrDefaultAsync();
            var expectedFirst = await categoryService.CreateAsync(CategoryName + 1, warehouse.Id);
            var expectedSecond = await categoryService.CreateAsync(CategoryName + 2, warehouse.Id);
            var expectedThird = await categoryService.CreateAsync(CategoryName + 3, warehouse.Id);

            var actual = await categoryService.GetAllCategoriesAsync("asd");

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
          var company =    await companyService.CreateAsync(CompanyName, CompanyAddress);

            var warehouseService = new WareHouseService(dbContext, companyService);

            var warehouse = await warehouseService.CreateAsync(WarehouseName, company.Id);

            return warehouseService;
        }
    }
}