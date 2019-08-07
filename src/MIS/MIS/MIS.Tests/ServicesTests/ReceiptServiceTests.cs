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

    public class ReceiptServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private ReceiptService receiptService;

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

            this.receiptService = new ReceiptService(this.dbContext, userService, companyService, productService);


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

            var user = new MISUser()
            {
                UserName = "asd",
                Email = "asd@asd.bg",
                FirstName = "asdddd",
                Company = company,
                LastName = "asdddd",
            };


            await this.dbContext.AddAsync(warehouse);
            await this.dbContext.AddAsync(user);
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
        public async Task GetCurrentOpenedReceipt_WithValidData_ShouldReturnCorrectReceipt()
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

            var actual = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(user.UserName);
            var expected = await this.dbContext.Receipts.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetCurrentOpenedReceipt_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var actual = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(user.UserName);
            Assert.IsNull(actual);
        }

        [Test]
        public async Task CreateReceipt_WithValidData_ShouldReturnCorrectReceipt()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var actual = await this.receiptService.CreateAsync(user.UserName);
            var expected = await this.dbContext.Receipts.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task CreateReceipt_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();

            var actual = await this.receiptService.CreateAsync("invalidUsername");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task AddProductToCurrentOpenedReceipt_WithValidData_ShouldReturnCorrectReceipt()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            var actual = await this.receiptService
                                   .AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var receiptProduct = await this.dbContext.ReceiptProducts.FirstOrDefaultAsync();

            Assert.AreEqual(createdReceipt.Id, actual.Receipt.Id);
            Assert.AreEqual(1, receiptProduct.Receipt.ReceiptProducts.Count);
            Assert.AreEqual(receiptProduct.Id, actual.Id);
        }

        [Test]
        public async Task AddProductToCurrentOpenedReceipt_WithInvalidProductId_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            var actual = await this.receiptService
                                   .AddProductToOpenedReceiptByUsernameAsync(user.UserName, "asd", 20);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task AddProductToCurrentOpenedReceipt_WithInvalidUsername_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            var actual = await this.receiptService
                                   .AddProductToOpenedReceiptByUsernameAsync("invalidUsername", product.Id, 20);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task FinishCurrentOpenedReceipt_WithValidData_ShouldReturnCorrectReceipt()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(user.UserName);

            Assert.AreEqual(createdReceipt.Id, actual.Id);
            Assert.IsNotNull(actual.IssuedOn);
        }

        [Test]
        public async Task FinishCurrentOpenedReceipt_WithEmptyCollectionOfReceiptProducts_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            var actual = await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(user.UserName);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task FinishCurrentOpenedReceipt_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            var actual = await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync("invalidName");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteReceipt_WithValidData_ShouldReturnCorrectReceipt()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.DeleteReceiptAsync(user.UserName);

            Assert.AreEqual(createdReceipt.Id, actual.Id);
            Assert.IsEmpty(actual.ReceiptProducts);
        }

        [Test]
        public async Task DeleteReceipt_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.DeleteReceiptAsync("invalidName");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetReceipt_WithValidData_ShouldReturnCorrectReceipt()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.GetReceiptAsync(createdReceipt.Id);

            Assert.AreEqual(createdReceipt.Id, actual.Id);
            Assert.IsNotEmpty(actual.ReceiptProducts);
        }

        [Test]
        public async Task GetReceipt_WithInvalidData_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.GetReceiptAsync(int.MinValue);

            Assert.IsNull(actual);
        }


        [Test]
        public async Task DeleteReceiptById_WithValidData_ShouldReturnCorrectReceipt()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.DeleteReceiptByIdAsync(createdReceipt.Id);

            Assert.AreEqual(createdReceipt.Id, actual.Id);
        }

        [Test]
        public async Task DeleteReceiptById_WithInvalidId_ShouldReturnNull()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var createdReceipt = await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);

            var actual = await this.receiptService.DeleteReceiptByIdAsync(int.MinValue);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetReceipts_WithValidData_ShouldReturnCorrectReceipts()
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync();
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync();
            var product = new Product()
            {
                BarCode = "81234567891131",
                Category = category,
                Name = "Coca cola",
                Price = 4.23m,
                Quantity = 24,
                WareHouse = category.WareHouse,
            };

            var report = new Report();

            await this.dbContext.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 20);
            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 30);
            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 40);

            var firstReceipt = await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(user.UserName);

            await this.receiptService.CreateAsync(user.UserName);

            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 120);
            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 130);
            await this.receiptService.AddProductToOpenedReceiptByUsernameAsync(user.UserName, product.Id, 140);

            var secondReceipt = await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(user.UserName);

            var actual = await this.receiptService.SetReceiptsAsync(report, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10), user.CompanyId);
            var actualArray = actual.OrderBy(x => x.IssuedOn).ToArray();

            Assert.AreEqual(firstReceipt.Id, actualArray[0].Id);
            Assert.AreEqual(secondReceipt.Id, actualArray[1].Id);
        }
    }
}