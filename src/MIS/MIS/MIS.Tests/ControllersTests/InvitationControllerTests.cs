namespace MIS.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Models;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using Services;

    using WebApp.Controllers;

    public class InvitationControllerTests : BaseControllerTests
    {
        private MISDbContext dbContext;
        private UserManager<MISUser> userManager;
        private IInvitationService invitationService;
        private SignInManager<MISUser> signInManager;

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            await this.dbContext.AddAsync(new MISUser()
            {
                UserName = "testUser",
                Company = new Company
                {
                    Name = "test",
                    Address = "test",
                    Messages = new List<Message>()
                    {
                        new Message()
                        {
                            AddedOn = DateTime.UtcNow,
                            Text = "testMessage",
                            Username = "testUser"
                        },
                        new Message()
                        {
                            AddedOn = DateTime.UtcNow,
                            Text = "testMessage",
                            Username = "testUser"
                        }
                    },
                    WareHouses = new List<WareHouse>()
                    {
                        new WareHouse()
                        {
                            Name = "testWareHouse",
                            IsFavorite = true,
                            Categories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "testCategoryName",
                                }
                            }
                        }
                    },
                    Invitations = new List<Invitation>()
                    {
                        new Invitation()
                    }
                }
            });

            await this.dbContext.SaveChangesAsync();

            var store = new Mock<IUserStore<MISUser>>();
            var mgr = new Mock<UserManager<MISUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<MISUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<MISUser>());

            mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
               .Returns((ClaimsPrincipal x) => this.dbContext.Users.FirstOrDefaultAsync(z => z.UserName == x.Identity.Name));


            var signInManager = new Mock<SignInManager<MISUser>>(mgr.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<MISUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<MISUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            signInManager.Setup(x => x.SignOutAsync())
                         .Returns(Task.CompletedTask);

            signInManager.Setup(x => x.SignInAsync(It.IsAny<MISUser>(), It.IsAny<bool>(), null))
                         .Returns(Task.CompletedTask);

            mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
               .Returns((ClaimsPrincipal x) => this.dbContext.Users.FirstOrDefaultAsync(z => z.UserName == x.Identity.Name));

            var companyService = new Mock<ICompanyService>().Object;
            var userService = new Mock<IUserService>().Object;
            this.invitationService = new InvitationService(this.dbContext, companyService,userService);
            this.userManager = mgr.Object;
            this.signInManager = signInManager.Object;
        }

        [Test]
        public void Index_WithValidData_ShouldReturnView()
            => MyController<InvitationController>
               .Instance()
               .WithDependencies(
                   this.userManager,
                   this.invitationService,
                   this.signInManager)
               .Calling(x => x.Index())
               .ShouldReturn()
               .View();

        [Test]
        public void Accept_WithValidData_ShouldReturnRedirectToAction()
            => MyController<InvitationController>
               .Instance()
               .WithDependencies(
                   this.userManager,
                   this.invitationService,
                   this.signInManager)
               .Calling(x => x.Accept(this.dbContext.Invitations.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index", "Company");


        [Test]
        public void Decline_WithValidData_ShouldReturnRedirectToAction()
            => MyController<InvitationController>
               .Instance()
               .WithDependencies(
                   this.userManager,
                   this.invitationService,
                   this.signInManager)
               .Calling(x => x.Accept(this.dbContext.Invitations.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index");
    }
}