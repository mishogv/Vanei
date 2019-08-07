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
        private MISDbContext dbContext;
        private ICategoryService categoryService;

        private const string CompanyName = "Microsoft";
        private const string WarehouseName = "MicrosoftWarehouse";
        private const string CategoryName = "MicrosoftCategory";
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
            var warehouseService = new WareHouseService(this.dbContext, companyService);
            var warehouse = await warehouseService.CreateAsync(WarehouseName, company.Id);
            this.categoryService = new CategoryService(warehouseService, this.dbContext);
        }

        [Test]
        public async Task CreateCategory_ShouldReturn_CorrectCategory()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var actual = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);
            var categoryFromDb = await this.dbContext.Categories.FirstOrDefaultAsync();

            Assert.AreEqual(categoryFromDb.Id, actual.Id);
            Assert.AreEqual(categoryFromDb.WareHouseId, actual.WareHouseId);
        }

        [Test]
        public async Task CreateCategory_ShouldReturn_NullWithInvalidId()
        {
            var actual = await this.categoryService.CreateAsync(CategoryName, "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task EditCategory_WithEditedData_ShouldReturnCorrectCategory()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await this.categoryService.EditAsync(expected.Id, "newName");

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("newName", actual.Name);
        }

        [Test]
        public async Task EditCategory_WithInvalidId_ShouldReturnNull()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await this.categoryService.EditAsync("asd", "newName");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteCategory_WithValidData_ShouldReturnCorrectCategory()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await this.categoryService.DeleteAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task DeleteCategory_WithInvalidId_ShouldReturnNull()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await this.categoryService.DeleteAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetCategory_WithValidData_ShouldReturnCorrectCategory()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await this.categoryService.GetCategoryAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetCategory_WithInvalidId_ShouldReturnNull()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var actual = await this.categoryService.GetCategoryAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetCategory_WithValidData_ShouldSetCorrectCategory()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expected = await this.categoryService.CreateAsync(CategoryName, warehouse.Id);

            var product = new Product();

            await this.categoryService.SetCategoryAsync(product, expected.Id);

            Assert.AreEqual(expected.Id, product.Category.Id);
        }

        [Test]
        public async Task SetCategory_WithInvalidId_ShouldSetNull()
        {
            var product = new Product();

            await this.categoryService.SetCategoryAsync(product, "asd");

            Assert.IsNull(product.Category);
        }

        [Test]
        public async Task GetAllCategoriesByCompanyId_WithValidId_ShouldReturnCorrectCategories()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expectedFirst = await this.categoryService.CreateAsync(CategoryName + 1, warehouse.Id);
            var expectedSecond = await this.categoryService.CreateAsync(CategoryName + 2, warehouse.Id);
            var expectedThird = await this.categoryService.CreateAsync(CategoryName + 3, warehouse.Id);

            var actual = await this.categoryService.GetAllByCompanyIdAsync(warehouse.CompanyId);
            var actualArray = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllCategoriesByCompanyId_ShouldReturnEmptyCollection()
        {
            var actual = await this.categoryService.GetAllByCompanyIdAsync("asd");

            Assert.IsEmpty(actual);
        }

        [Test]
        public async Task GetAllCategoriesByWarehouseId_WithValidData_ShouldReturnCorrectCategories()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();
            var expectedFirst = await this.categoryService.CreateAsync(CategoryName + 1, warehouse.Id);
            var expectedSecond = await this.categoryService.CreateAsync(CategoryName + 2, warehouse.Id);
            var expectedThird = await this.categoryService.CreateAsync(CategoryName + 3, warehouse.Id);

            var actual = await this.categoryService.GetAllCategoriesAsync(warehouse.Id);
            var actualArray = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllCategoriesByWarehouseId_ShouldReturnEmptyCollection()
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync();

            var actual = await this.categoryService.GetAllCategoriesAsync("asd");

            Assert.IsEmpty(actual);
        }
    }
}