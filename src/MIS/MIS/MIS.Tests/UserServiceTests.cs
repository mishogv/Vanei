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

    public class UserServiceTests : BaseServiceTests
    {
        private const string Name = "pesho";

        [Test]
        public async Task AddToCompany_ShouldReturn_CorrectUser()
        {
            var dbContext = this.GetDbContext();
            var company = new Company();
            var userService = new UserService(dbContext);
            var user = new MISUser()
            {
                CompanyId = company.Id,
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name,
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            await userService.AddToCompanyAsync(company, user.Id);
            var actual = company.Employees.FirstOrDefault(x => x.Id == user.Id);

            Assert.AreEqual(user.Id, actual.Id);
        }

        [Test]
        public async Task AddToCompany_ShouldReturn_EmptyCollectionOfUsers()
        {
            var dbContext = this.GetDbContext();
            var company = new Company();
            var userService = new UserService(dbContext);

            await userService.AddToCompanyAsync(company, "asd");

            Assert.IsEmpty(company.Employees);
        }

        [Test]
        public async Task SetInvitation_ShouldReturn_CorrectUser()
        {
            var dbContext = this.GetDbContext();
            var invitation = new Invitation();
            var userService = new UserService(dbContext);
            var user = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name,
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            await userService.SetInvitationAsync(invitation, user.Id);
            var actual = invitation.User;

            Assert.AreEqual(user.Id, actual.Id);
        }

        [Test]
        public async Task SetInvitation_ShouldReturn_Null()
        {
            var dbContext = this.GetDbContext();
            var invitation = new Invitation();
            var userService = new UserService(dbContext);
            var user = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name,
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            await userService.SetInvitationAsync(invitation, "asd");
            var actual = invitation.User;

            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetReceipt_ShouldReturn_CorrectUser()
        {
            var dbContext = this.GetDbContext();
            var receipt = new Receipt();
            var userService = new UserService(dbContext);
            var user = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name,
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            user.CompanyId = "123";

            await userService.SetReceiptAsync(receipt, Name);

            Assert.AreEqual(user.Id, receipt.User.Id);
        }

        [Test]
        public async Task SetReceipt_ShouldThrow_ArgumentNullException()
        {
            var dbContext = this.GetDbContext();
            var receipt = new Receipt();
            var userService = new UserService(dbContext);
            var user = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name,
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

           

            Assert.ThrowsAsync<ArgumentNullException>(async () => await userService.SetReceiptAsync(receipt, Name));
        }

        [Test]
        public async Task GetAllUsers_ShouldReturn_CorrectUsers()
        {
            var dbContext = this.GetDbContext();
            var receipt = new Receipt();
            var userService = new UserService(dbContext);
            var userFirst = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name + 1,
            };

            var userSecond = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name + 2,
            };

            var userThird = new MISUser()
            {
                Email = Name,
                FirstName = Name,
                LastName = Name,
                UserName = Name + 3,
            };


            await dbContext.AddAsync(userFirst);
            await dbContext.AddAsync(userSecond);
            await dbContext.AddAsync(userThird);
            await dbContext.SaveChangesAsync();

            var actual = await userService.GetAllUsersAsync();
            var actualArray = actual.OrderBy(x => x.Username).ToArray();

            Assert.AreEqual(userFirst.Id, actualArray[0].Id);
            Assert.AreEqual(userSecond.Id, actualArray[1].Id);
            Assert.AreEqual(userThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturn_EmptyCollection()
        {
            var dbContext = this.GetDbContext();
            var receipt = new Receipt();
            var userService = new UserService(dbContext);

            var actual = await userService.GetAllUsersAsync();

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

    }
}