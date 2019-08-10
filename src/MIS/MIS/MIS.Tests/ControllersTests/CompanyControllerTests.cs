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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Models;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using Services;

    using ViewModels.Input.Company;
    using ViewModels.View.Chat;
    using ViewModels.View.Company;

    using WebApp.Controllers;

    public class CompanyControllerTests : BaseControllerTests
    {
        private MISDbContext dbContext;
        private ICompanyService companyService;
        private IMessageService messageService;
        private SignInManager<MISUser> signInManager;
        private UserManager<MISUser> userManager;

        [SetUp]
        public async Task Init()
        {
            //TODO : REFACTOR
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
                    }
                }
            });

            await this.dbContext.SaveChangesAsync();

            var store = new Mock<IUserStore<MISUser>>();
            var mgr = new Mock<UserManager<MISUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<MISUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<MISUser>());

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

            var userService = new UserService(this.dbContext);
            this.companyService = new CompanyService(this.dbContext, userService);
            this.messageService = new MessageService(this.dbContext, this.companyService);
            this.userManager = mgr.Object;
            this.signInManager = signInManager.Object;
        }

        [Test]
        public void Index_WithValidData_ShouldReturnView()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .WithUser(x => x.WithUsername("testUser"))
               .Calling(x => x.Index())
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsNotNull(x.Model))
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<DetailsCompanyViewModel>(x.Model));

        [Test]
        public void Index_WithInvalidData_ShouldReturnRedirectToCreate()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .WithUser(x => x.WithUsername("invalidName"))
               .Calling(x => x.Index())
               .ShouldReturn()
               .RedirectToAction("Create");

        [Test]
        public void Chat_WithValidData_ShouldReturnViewWithMessageCountTwo()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .Calling(x => x.Chat(this.dbContext.Companies.FirstOrDefault().Id))
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<CompanyChatViewModel>(x.Model))
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x
                   => Assert.AreEqual(2, ((CompanyChatViewModel)x.Model).Messages.Count()));

        [Test]
        public void Chat_WithInvalidData_ShouldReturnViewWithMessagesEmptyCollection()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .Calling(x => x.Chat("invalidCompanyId"))
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsEmpty(((CompanyChatViewModel)x.Model).Messages));

        [Test]
        public void Create_WithValidData_ShouldReturnView()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .Calling(x => x.Create())
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsNotNull(x.Model))
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<CompanyCreateInputModel>(x.Model));

        [Test]
        public void Create_WithInvalidData_ShouldReturnViewWithInvalidModelState()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .Calling(x => x.Create(new CompanyCreateInputModel()
               {
                   Name = null,
                   Address = null
               }))
               .ShouldHave()
               .InvalidModelState()
               .AndAlso()
               .ShouldReturn()
               .View();

        [Test]
        public void Create_WithValidData_ShouldReturnRedirectWithValidModelState()
            => MyController<CompanyController>
               .Instance()
               .WithDependencies(this.companyService, this.messageService,
                   this.userManager, this.signInManager)
               .WithUser("testUser")
               .Calling(x => x.Create(new CompanyCreateInputModel()
               {
                   Name = "validName",
                   Address = "validName"
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .RedirectToAction("Index");
    }
}
