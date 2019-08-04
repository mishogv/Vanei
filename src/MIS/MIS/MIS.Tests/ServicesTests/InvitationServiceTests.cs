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

    public class InvitationServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private IInvitationService invitationService;

        [SetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            var userService = new UserService(this.dbContext);
            var companyService = new CompanyService(this.dbContext, userService);
            this.invitationService = new InvitationService(this.dbContext, companyService, userService);
        }

        [Test]
        public async Task InviteAsync_ShouldReturn_CorrectInvitation()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            var user = new MISUser() {FirstName = "asd", LastName = "asd", UserName = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.invitationService.InviteAsync(company.Id, user.Id);
            var expected = await this.dbContext.Invitations.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task InviteAsync_ShouldReturn_NullWithIncorrectData()
        {
            var actual = await this.invitationService.InviteAsync("asd", "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task AcceptInvitation_ShouldReturn_CorrectInvitation()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            var user = new MISUser() {FirstName = "asd", LastName = "asd", UserName = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.invitationService.InviteAsync(company.Id, user.Id);
            var actual = await this.invitationService.AcceptInvitationAsync(expected.Id, false);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.User.Id, actual.User.Id);
        }

        [Test]
        public async Task AcceptInvitation_ShouldReturn_NullWithIncorrectData()
        {
            var actual = await this.invitationService.AcceptInvitationAsync("asd", false);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeclineInvitation_ShouldReturn_CorrectInvitation()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            var user = new MISUser() {FirstName = "asd", LastName = "asd", UserName = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.invitationService.InviteAsync(company.Id, user.Id);
            var actual = await this.invitationService.DeclineInvitationAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.User.Id, actual.User.Id);
        }

        [Test]
        public async Task DeclineInvitation_ShouldReturn_NullWithIncorrectData()
        {
            var actual = await this.invitationService.DeclineInvitationAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetAllInvitation_ShouldReturn_CorrectInvitations()
        {
            var company = new Company() {Address = "asd", Name = "asd",};

            var user = new MISUser() {FirstName = "asd", LastName = "asd", UserName = "asd",};

            await this.dbContext.AddAsync(company);
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var expected = await this.invitationService.InviteAsync(company.Id, user.Id);
            var actual = await this.invitationService.GetAllAsync(user.Id);
            var actualFirst = actual?.FirstOrDefault();

            Assert.AreEqual(expected.Id, actualFirst?.Id);
            Assert.AreEqual(expected.User.Id, actualFirst?.User.Id);
        }

        [Test]
        public async Task GetAllInvitation_ShouldReturn_EmptyCollection()
        {
            var actual = await this.invitationService.GetAllAsync("asd");

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