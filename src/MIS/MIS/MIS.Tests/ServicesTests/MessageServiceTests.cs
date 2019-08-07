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

    public class MessageServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private IMessageService messagesService;

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            var userService = new UserService(this.dbContext);
            var companyService = new CompanyService(this.dbContext, userService);
            this.messagesService = new MessageService(this.dbContext, companyService);
        }

        [Test]
        public async Task CreateMessageWithNotificationFalse_ShouldReturn_CorrectMessage()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.SaveChangesAsync();
            var actual = await this.messagesService.CreateAsync(company.Id, "asd", "asd", false);
            var expected = await this.dbContext.Messages.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("asd", actual.Text);
        }

        [Test]
        public async Task CreateMessageWithJoinNotification_WithValidData_ShouldReturnCorrectMessage()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.messagesService.CreateAsync(company.Id, "asd", "{0} has joined the group {1}", true);
            var expected = await this.dbContext.Messages.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual("asd has joined the group asd", actual.Text);
        }

        [Test]
        public async Task GetAll_WithValidData_ShouldReturnCorrectMessageCollection()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.SaveChangesAsync();

            var expectedFirst =
                await this.messagesService.CreateAsync(company.Id, "asd", "{0} has left the group {1}", true);
            var expectedSecond =
                await this.messagesService.CreateAsync(company.Id, "asd1", "{0} has left the group {1}", true);
            var expectedThird =
                await this.messagesService.CreateAsync(company.Id, "asd2", "{0} has left the group {1}", true);

            var actual = await this.messagesService.GetAllAsync(company.Id);
            var actualArray = actual.OrderBy(x => x.Username).ToArray();

            Assert.AreEqual(expectedFirst.Id, actualArray[0].Id);
            Assert.AreEqual(expectedSecond.Id, actualArray[1].Id);
            Assert.AreEqual(expectedThird.Id, actualArray[2].Id);
        }

        [Test]
        public async Task GetAll_WithInvalidId_ShouldReturnEmptyCollection()
        {
            var messages = await this.messagesService.GetAllAsync("asd");

            Assert.IsEmpty(messages);
        }
    }
}