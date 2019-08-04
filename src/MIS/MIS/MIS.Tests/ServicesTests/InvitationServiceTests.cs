namespace MIS.Tests.ServicesTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using Moq;

    using NUnit.Framework;

    using Services;

    public class InvitationServiceTests : BaseServiceTests
    {

        [Test]
        public async Task InviteAsync_ShouldReturn_CorrectInvitation()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);

            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };


            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

           var actual =   await invitationService.InviteAsync(company.Id, user.Id);
           var expected = await dbContext.Invitations.FirstOrDefaultAsync();

           Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task InviteAsync_ShouldReturn_NullWithIncorrectData()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);


            var actual = await invitationService.InviteAsync("asd", "asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task AcceptInvitation_ShouldReturn_CorrectInvitation()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);

            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };


            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var expected = await invitationService.InviteAsync(company.Id, user.Id);
            var actual = await invitationService.AcceptInvitationAsync(expected.Id, false);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.User.Id, actual.User.Id);
        }

        [Test]
        public async Task AcceptInvitation_ShouldReturn_NullWithIncorrectData()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);

            var actual = await invitationService.AcceptInvitationAsync("asd", false);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeclineInvitation_ShouldReturn_CorrectInvitation()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);

            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };


            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var expected = await invitationService.InviteAsync(company.Id, user.Id);
            var actual = await invitationService.DeclineInvitationAsync(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.User.Id, actual.User.Id);
        }

        [Test]
        public async Task DeclineInvitation_ShouldReturn_NullWithIncorrectData()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);

            var actual = await invitationService.DeclineInvitationAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task GetAllInvitation_ShouldReturn_CorrectInvitations()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);

            var company = new Company()
            {
                Address = "asd",
                Name = "asd",
            };


            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            await dbContext.AddAsync(company);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var expected = await invitationService.InviteAsync(company.Id, user.Id);
            var actual = await invitationService.GetAllAsync(user.Id);
            var actualFirst = actual?.FirstOrDefault();

            Assert.AreEqual(expected.Id, actualFirst?.Id);
            Assert.AreEqual(expected.User.Id, actualFirst?.User.Id);
        }


        [Test]
        public async Task GetAllInvitation_ShouldReturn_EmptyCollection()
        {
            var dbContext = this.GetDbContext();
            var userService = new UserService(dbContext);
            var invitationService = new InvitationService(dbContext,
                new CompanyService(dbContext, userService), userService);


            var actual = await invitationService.GetAllAsync("asd");

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