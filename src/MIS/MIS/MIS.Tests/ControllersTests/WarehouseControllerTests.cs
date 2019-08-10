namespace MIS.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using Services;

    using ViewModels.Input.WareHouse;

    using WebApp.Controllers;

    public class WarehouseControllerTests : BaseControllerTests
    {
        private MISDbContext dbContext;
        private UserManager<MISUser> userManager;
        private IWareHouseService wareHouseService;

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
                                    Products = new List<Product>()
                                    {
                                        new Product()
                                        {
                                            Name = "testProduct",
                                            BarCode = "testBarcode",
                                            Price = 2.4m,
                                            Quantity = 3,
                                        }
                                    }
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
               .Returns((ClaimsPrincipal x) => this.dbContext.Users.Include(c => c.Company).FirstOrDefaultAsync(z => z.UserName == x.Identity.Name));

            mgr.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
               .Returns((ClaimsPrincipal x) => this.dbContext.Users.Include(c => c.Company).FirstOrDefaultAsync(z => z.UserName == x.Identity.Name).GetAwaiter().GetResult()?.Id);

            this.userManager = mgr.Object;

            var userService = new UserService(this.dbContext);
            var companyService = new CompanyService(this.dbContext, userService);
            this.wareHouseService = new WareHouseService(this.dbContext, companyService);
        }

        [Test]
        public void Index_WithValidData_ShouldReturnView()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Index(null))
               .ShouldReturn()
               .View();

        [Test]
        public void Index_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("invalidUser")
               .Calling(x => x.Index(null))
               .ShouldReturn()
               .RedirectToAction("Create", "Company");

        [Test]
        public void Index_WithInvalidWareHouseId_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Index("invalidId"))
               .ShouldReturn()
               .RedirectToAction("Create", "WareHouse");

        [Test]
        public void Create_ShouldReturnView()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Create())
               .ShouldReturn()
               .View();

        [Test]
        public void Create_WithValidData_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Create(new WareHouseCreateInputModel()
               {
                   Name = "validName"
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .RedirectToAction("Index");

        [Test]
        public void Create_WithInvalidData_ShouldReturnViewWithInvalidModelState()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Create(new WareHouseCreateInputModel()
               {
                   Name = null
               }))
               .ShouldHave()
               .InvalidModelState(1)
               .AndAlso()
               .ShouldReturn()
               .View();

        [Test]
        public void Favorite_WithValidData_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Favorite(this.dbContext.WareHouses.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index");

        [Test]
        public void Edit_WithValidData_ShouldReturnView()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Edit(this.dbContext.WareHouses.FirstOrDefault().Id))
               .ShouldReturn()
               .View();

        [Test]
        public void Edit_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Edit("invalidId"))
               .ShouldReturn()
               .RedirectToAction("Create");

        [Test]
        public void Edit_WithValidData_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Edit(new WarehouseEditInputModel()
               {
                   Id = this.dbContext.WareHouses.FirstOrDefault().Id,
                   Name = "validName"
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .RedirectToAction("Index");

        [Test]
        public void Edit_WithInvalidData_ShouldReturnViewWithInvalidModelState()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Edit(new WarehouseEditInputModel()
               {
                   Id = this.dbContext.WareHouses.FirstOrDefault().Id,
                   Name = null
               }))
               .ShouldHave()
               .InvalidModelState(1)
               .AndAlso()
               .ShouldReturn()
               .View();

        [Test]
        public void Delete_ShouldReturnRedirectToAction()
            => MyController<WareHouseController>
               .Instance()
               .WithDependencies(
                   this.wareHouseService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Delete(this.dbContext.WareHouses.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index");
    }
}