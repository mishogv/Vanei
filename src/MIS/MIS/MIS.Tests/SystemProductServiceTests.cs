namespace MIS.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class SystemProductServiceTests
    {
        private MISDbContext db;
        private ISystemProductsService productsService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(databaseName: "Add_Favorites_Database")
                          .Options;

            this.db = new MISDbContext(options);

            this.productsService = new Mock<SystemProductsService>(this.db).Object;
        }

        [Test]
        public async Task CreateSystemProductAsync_ShouldIncreaseCountInDb_CountShouldBeOne()
        {
            await this.productsService.CreateSystemProductAsync("gosho", 2m, "gosho00", "Description", "userId" );

            Assert.AreEqual(1, this.db.SystemProducts.Count());
        }
    }
}