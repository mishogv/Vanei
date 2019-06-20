namespace MIS.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using Moq;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class SystemProductServiceTests
    {
        private const string UserEmail = "gosho@abv.bg";
        private const string Username = "gosho";
        private const string FirstName = "Gosho";
        private const string LastName = "Goshov";
        private const string Password = "123456";

        
        private MISDbContext db;
        private ISystemProductsService productsService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(databaseName: "SystemProductTestDb")
                          .Options;


            this.db = new MISDbContext(options);

            this.db.Add(new MISUser()
                {
                    Email = UserEmail, UserName = Username, FirstName = FirstName, LastName = LastName,
                    EmailConfirmed = true, PasswordHash = Password
                });

            this.db.SaveChanges();
            this.productsService = new Mock<SystemProductsService>(this.db).Object;
        }

        [Test]
        public async Task CreateSystemProductAsync_ShouldIncreaseCountInDb_CountShouldBeOne()
        {
            await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", "userId" );

            Assert.IsTrue(this.db.SystemProducts.Any());
        }

        [Test]
        public async Task CreateSystemProductAsync_ShouldReturnCorrectProduct()
        {
            var user = await this.db.Users.FirstOrDefaultAsync();
            var product =  await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", user.Id);

            var expectedName = "gosho";
            var expectedPrice = 2m;
            var expectedUrl = "gosho00";
            var expectedDescription = "Description";

            Assert.AreEqual(expectedName, product.Name);
            Assert.AreEqual(expectedPrice, product.Price);
            Assert.AreEqual(expectedUrl, product.ImgUrl);
            Assert.AreEqual(expectedDescription, product.Description);
            Assert.AreEqual(user.Id, product.UserId);
        }

        [Test]
        public async Task CreateSystemProductAsync_ShouldExistInDbAfterCreation()
        {
            var user = await this.db.Users.FirstOrDefaultAsync();
            await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", user.Id);

            var expectedName = "gosho";
            var expectedPrice = 2m;
            var expectedUrl = "gosho00";
            var expectedDescription = "Description";

            var product = await this.db.SystemProducts.FirstOrDefaultAsync(x => x.Name == expectedName);

            
            Assert.AreEqual(expectedName, product.Name);
            Assert.AreEqual(expectedPrice, product.Price);
            Assert.AreEqual(expectedUrl, product.ImgUrl);
            Assert.AreEqual(expectedDescription, product.Description);
            Assert.AreEqual(user.Id, product.UserId);
        }

        [Test]
        public async Task GetSystemProductByIdAsync_ShouldReturnCorrectSystemProduct()
        {
            var user = await this.db.Users.FirstOrDefaultAsync();
            var productFromService = await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", user.Id);
            var productId = productFromService.Id;

            var expectedName = "gosho";
            var expectedPrice = 2m;
            var expectedUrl = "gosho00";
            var expectedDescription = "Description";

            var actualProduct = await this.productsService.GetSystemProductByIdAsync(productId);


            Assert.AreEqual(expectedName, actualProduct.Name);
            Assert.AreEqual(expectedPrice, actualProduct.Price);
            Assert.AreEqual(expectedUrl, actualProduct.ImgUrl);
            Assert.AreEqual(expectedDescription, actualProduct.Description);
            Assert.AreEqual(user.Id, actualProduct.UserId);
        }

        [Test]
        public async Task GetSystemProductByIdAsync_ShouldReturnNull()
        {
            await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", "10");
            var lastProduct = await this.db.SystemProducts.LastOrDefaultAsync();
            var actualProduct = await this.productsService.GetSystemProductByIdAsync(lastProduct.Id + 1);

            Assert.AreEqual(null, actualProduct);
        }

        [Test]
        public async Task GetAllSystemProducts_ShouldReturnCorrectProductsCount()
        {
            await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", "10");
            await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", "10");
            var allProductsCount = this.productsService.GetAllSystemProducts().ToList().Count;
            
            Assert.AreEqual(2, allProductsCount);
        }
    }
}