namespace MIS.Tests.ServicesTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Microsoft.AspNetCore.Identity;

    using Models;

    using Moq;

    using NUnit.Framework;

    using Services;

    public class AdministratorServiceTests : BaseServiceTests
    {
        [Test]
        public async Task CreateAdministratorByIdAsync_ShouldReturn_TrueIfSuccess()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore,list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            var result = await userMangerMock.Object.CreateAsync(user, "123456");

            var actual = await administratorService.CreateAdministratorByIdAsync(user.Id);

            Assert.IsTrue(actual);
        }


        [Test]
        public async Task CreateAdministratorByIdAsync_ShouldReturn_FalseWithInvalidUserId()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore, list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            var actual = await administratorService.CreateAdministratorByIdAsync("asd");

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task CreateAdministratorByIdAsync_ShouldReturn_FalseWithRootAdminUsername()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore, list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = GlobalConstants.RootAdminName,
            };

            var result = await userMangerMock.Object.CreateAsync(user, "123456");
            var actual = await administratorService.CreateAdministratorByIdAsync(user.Id);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task RemoveAdministratorByIdAsync_ShouldReturn_TrueIfSuccess()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore, list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            var result = await userMangerMock.Object.CreateAsync(user, "123456");

            var actual = await administratorService.RemoveAdministratorByIdAsync(user.Id);

            Assert.IsTrue(actual);
        }


        [Test]
        public async Task RemoveAdministratorByIdAsync_ShouldReturn_FalseWithInvalidUserId()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore, list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = "asd",
            };

            var actual = await administratorService.RemoveAdministratorByIdAsync("asd");

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task RemoveAdministratorByIdAsync_ShouldReturn_FalseWithRootAdminUsername()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore, list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",
                UserName = GlobalConstants.RootAdminName,
            };

            var result = await userMangerMock.Object.CreateAsync(user, "123456");
            var actual = await administratorService.RemoveAdministratorByIdAsync(user.Id);

            Assert.IsFalse(actual);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturn_CorrectUsers()
        {
            var userStore = new Mock<IUserStore<MISUser>>();
            var list = new List<MISUser>();
            Mock<UserManager<MISUser>> userMangerMock = GetUserManger(userStore, list);
            var administratorService = new AdministratorService(userMangerMock.Object);
            var user = new MISUser()
            {
                FirstName = "asd",
                LastName = "asd",   
                UserName = GlobalConstants.RootAdminName,
            };

            await userMangerMock.Object.CreateAsync(user, "123456");

            var actual = administratorService.GetAllUsers().ToList();

            Assert.IsNotEmpty(actual);
        }

        private static Mock<UserManager<MISUser>> GetUserManger(Mock<IUserStore<MISUser>> userStore, List<MISUser> list)
        {
            var userMangerMock = new Mock<UserManager<MISUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            
            userMangerMock.Object.UserValidators.Add(new UserValidator<MISUser>());
            userMangerMock.Object.PasswordValidators.Add(new PasswordValidator<MISUser>());
            userMangerMock.Setup(x => x.DeleteAsync(It.IsAny<MISUser>())).ReturnsAsync(IdentityResult.Success);
            userMangerMock.Setup(x => x.CreateAsync(It.IsAny<MISUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<MISUser, string>((x, y) => list.Add(x));
            userMangerMock.Setup(x => x.UpdateAsync(It.IsAny<MISUser>())).ReturnsAsync(IdentityResult.Success);
            userMangerMock.Setup(x => x.AddToRoleAsync(It.IsAny<MISUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userMangerMock.Setup(x => x.RemoveFromRoleAsync(It.IsAny<MISUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            userMangerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(list.FirstOrDefault);
            userMangerMock.Setup(x => x.Users).Returns(list.AsQueryable);

            return userMangerMock;
        }
    }
}