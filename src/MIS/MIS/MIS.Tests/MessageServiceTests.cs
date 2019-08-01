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

    public class MessageServiceTests : BaseServiceTests
    {
        [Test]
        public async Task CreateMessageWithNotificationFalse_ShouldReturn_CorrectMessage()
        {
            var dbContext = this.GetDbContext();
            var companyService = new CompanyService(dbContext, new UserService(dbContext));
            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.SaveChangesAsync();
            var messagesService = new MessageService(dbContext, companyService);

            var actual = await messagesService.CreateAsync(company.Id, "asd", "asd", false);
            var expected = await dbContext.Messages.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("asd", actual.Text);
        }

        [Test]
        public async Task CreateMessageWithJoinNotification_ShouldReturn_CorrectMessage()
        {
            var dbContext = this.GetDbContext();
            var companyService = new CompanyService(dbContext, new UserService(dbContext));
            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.SaveChangesAsync();
            var messagesService = new MessageService(dbContext, companyService);

            var actual = await messagesService.CreateAsync(company.Id, "asd", "{0} has joined the group {1}", true);
            var expected = await dbContext.Messages.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("asd has joined the group asd", actual.Text);
        }

        [Test]
        public async Task GetAll_ShouldReturn_CorrectMessageCollection()
        {
            var dbContext = this.GetDbContext();
            var companyService = new CompanyService(dbContext, new UserService(dbContext));
            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.SaveChangesAsync();
            var messagesService = new MessageService(dbContext, companyService);

            var expectedFirst = await messagesService.CreateAsync(company.Id, "asd", "{0} has left the group {1}", true);
            var expectedSecond = await messagesService.CreateAsync(company.Id, "asd1", "{0} has left the group {1}", true);
            var expectedThird = await messagesService.CreateAsync(company.Id, "asd2", "{0} has left the group {1}", true);

            var actual = await messagesService.GetAllAsync(company.Id);
            var actualArray = actual.OrderBy(x => x.Username).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAll_ShouldReturn_EmptyCollection()
        {
            var dbContext = this.GetDbContext();
            var companyService = new CompanyService(dbContext, new UserService(dbContext));
            var messagesService = new MessageService(dbContext, companyService);
            var messages = await messagesService.GetAllAsync("asd");

            Assert.IsEmpty(messages);
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