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

    public class ProductServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private ProductService productService;

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            this.productService = new ProductService(this.dbContext,
                new CategoryService(
                    new WareHouseService(this.dbContext,
                        new CompanyService(this.dbContext, new UserService(this.dbContext))), this.dbContext));

            var company = new Company()
            {
                Name = "asd",
                Address = "asd",
            };

            await this.dbContext.AddAsync(company);
            await this.dbContext.SaveChangesAsync();
            var warehouse = new WareHouse()
            {
                Name = "asd",
                Company = company,
            };

            await this.dbContext.AddAsync(warehouse);
            await this.dbContext.SaveChangesAsync();

            var category = new Category()
            {
                Name = "asd",
                WareHouse = warehouse
            };


            await this.dbContext.AddAsync(category);
            await this.dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task CreateProduct_WithValidData_ShouldReturnCorrectProduct()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var actual = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var expected = await this.dbContext.Products.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task CreateProduct_WithInvalidData_ShouldReturnNull()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var actual = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", "asd", category.WareHouseId);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetProduct_WithValidData_ShouldReturnCorrectProduct()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var expected = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var actual = await this.productService.GetProductAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetProduct_WithInvalidData_ShouldReturnNull()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var actual = await
                this.productService.GetProductAsync("invalidId");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteProduct_WithValidData_ShouldReturnCorrectProduct()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var expected = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var actual = await this.productService.DeleteAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task DeleteProduct_WithInvalidId_ShouldReturnNull()
        {
            var actual = await
                this.productService.DeleteAsync("invalidId");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetProduct_WithValidData_ShouldReturnCorrectProduct()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var expected = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var receiptProduct = new ReceiptProduct();

            var actual = await this.productService.SetProductAsync(receiptProduct, expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task SetProduct_WithInvalidData_ShouldReturnNull()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var receiptProduct = new ReceiptProduct();

            var actual = await this.productService.SetProductAsync(receiptProduct, "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task UpdateProduct_WithValidData_ShouldReturnCorrectProduct()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var result = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var receiptProduct = new ReceiptProduct();

            var actual = await this.productService.UpdateAsync(result.Id, "updatedProduct",
                2.2m, 200, "81234567891138", category.Id);

            Assert.AreEqual(result.Id, actual.Id);
            Assert.AreEqual("updatedProduct", actual.Name);
            Assert.AreEqual(2.2m, actual.Price);
            Assert.AreEqual(200, actual.Quantity);
            Assert.AreEqual(category.Id, actual.CategoryId);
        }

        [Test]
        public async Task UpdateProduct_WithInvalidData_ShouldReturnNull()
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync();

            var result = await
                this.productService.CreateAsync("product", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var receiptProduct = new ReceiptProduct();

            var actual = await this.productService.UpdateAsync("asd", "updatedProduct",
                2.2m, 200, "81234567891138", category.Id);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetAllProducts_WithvalidData_ShouldReturnCorrectProducts()
        {
            var category = await this.dbContext.Categories
                                     .Include(x => x.WareHouse)
                                     .ThenInclude(x => x.Company).FirstOrDefaultAsync();

            var expectedFirst = await
                this.productService.CreateAsync("product1", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var expectedSecond = await
                this.productService.CreateAsync("product2", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var expectedThird = await
                this.productService.CreateAsync("product3", 2m, 4, "81234567891131", category.Id, category.WareHouseId);

            var actual = await this.productService.GetAllProductsCompanyIdAsync(category.WareHouse.CompanyId);
            var actualArray = actual.OrderBy(x => x.Name).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }


        [Test]
        public async Task GetAllProducts_WithInvalidId_ShouldReturnEmptyProductsCollection()
        {
            var category = await this.dbContext.Categories
                                     .Include(x => x.WareHouse)
                                     .ThenInclude(x => x.Company).FirstOrDefaultAsync();

            var actual = await this.productService.GetAllProductsCompanyIdAsync("asd");
            Assert.IsEmpty(actual);
        }
    }
}