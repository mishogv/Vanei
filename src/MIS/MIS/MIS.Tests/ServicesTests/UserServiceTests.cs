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

    public class UserServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private IUserService userService;

        private const string Name = "pesho";

        [SetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            this.userService = new UserService(this.dbContext);
        }

        [Test]
        public async Task AddToCompany_ShouldReturn_CorrectUser()
        {
            var company = new Company();
            var user = new MISUser()
            {
                CompanyId = company.Id,
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name,
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            await this.userService.AddToCompanyAsync(company, user.Id);
            var actual = company.Employees.FirstOrDefault(x => x.Id == user.Id);

            Assert.AreEqual(user.Id, actual.Id);
        }

        [Test]
        public async Task AddToCompany_ShouldReturn_EmptyCollectionOfUsers()
        {
            var company = new Company();

            await this.userService.AddToCompanyAsync(company, "asd");

            Assert.IsEmpty(company.Employees);
        }

        [Test]
        public async Task SetInvitation_ShouldReturn_CorrectUser()
        {
            var invitation = new Invitation();
            var user = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name,
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            await this.userService.SetInvitationAsync(invitation, user.Id);
            var actual = invitation.User;

            Assert.AreEqual(user.Id, actual.Id);
        }

        [Test]
        public async Task SetInvitation_ShouldReturn_Null()
        {
            var invitation = new Invitation();
            var user = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name,
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            await this.userService.SetInvitationAsync(invitation, "asd");
            var actual = invitation.User;

            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetReceipt_ShouldReturn_CorrectUser()
        {
            var receipt = new Receipt();
            var user = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name,
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            user.CompanyId = "123";

            await this.userService.SetReceiptAsync(receipt, Name);

            Assert.AreEqual(user.Id, receipt.User.Id);
        }

        [Test]
        public async Task SetReceipt_ShouldThrow_ReturnNullWithInvalidData()
        {
            var receipt = new Receipt();
            var user = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name,
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.userService.SetReceiptAsync(receipt, "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturn_CorrectUsers()
        {
            var receipt = new Receipt();
            var userFirst = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name + 1,
            };

            var userSecond = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name + 2,
            };

            var userThird = new MISUser()
            {
                Email = Name, FirstName = Name, LastName = Name, UserName = Name + 3,
            };

            await this.dbContext.AddAsync(userFirst);
            await this.dbContext.AddAsync(userSecond);
            await this.dbContext.AddAsync(userThird);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.userService.GetAllUsersAsync();
            var actualArray = actual.OrderBy(x => x.Username).ToArray();

            Assert.AreEqual(userFirst.Id, actualArray[0].Id);
            Assert.AreEqual(userSecond.Id, actualArray[1].Id);
            Assert.AreEqual(userThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturn_EmptyCollection()
        {
            var receipt = new Receipt();

            var actual = await this.userService.GetAllUsersAsync();

            Assert.IsEmpty(actual);
        }
    }
}