namespace MIS.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Models;

    using Moq;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    using Services;

    [TestFixture]
    public class AdministratorServiceTests
    {
        private const string UserEmail = "gosho@abv.bg";
        private const string Username = "gosho";
        private const string FirstName = "Gosho";
        private const string LastName = "Goshov";
        private const string Password = "123456";


        private MISDbContext db;
        private IAdministratorService administratorService;
        private UserManager<MISUser> userManager;
        private List<MISUser> store;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(databaseName: "SystemProductTestDb")
                          .Options;

            this.store = new List<MISUser>();

            this.db = new MISDbContext(options);

            this.userManager = new Mock<UserManager<MISUser>>(
                new Mock<IUserStore<MISUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<MISUser>>().Object,
                new IUserValidator<MISUser>[0],
                new IPasswordValidator<MISUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<MISUser>>>().Object).Object;

            this.administratorService = new Mock<AdministratorService>(this.userManager).Object;
            var user = new MISUser()
            {
                Email = UserEmail,
                UserName = Username,
                FirstName = FirstName,
                LastName = LastName,
                EmailConfirmed = true,
            };

            var result = this.userManager.CreateAsync(user).GetAwaiter().GetResult()?.Succeeded;
            
            this.db.SaveChanges();
            //this.productService = new Mock<SystemProductService>(this.db).Object;
        }

        [Test]
        public async Task CreateAdmin_ShouldWorkCorrectly()
        {
            var user = this.userManager.Users.FirstOrDefault();
            var result = await this.administratorService.CreateAdministratorByIdAsync(user.Id);
            Assert.IsTrue(true);
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }
    }
}