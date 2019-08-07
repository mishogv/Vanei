namespace MIS.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Models;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using Services;

    using ViewModels.Input.Category;

    using WebApp.Controllers;

    public class CategoryControllerTests
    {
        private MISDbContext dbContext;
        private ICategoryService categoryService;
        private UserManager<MISUser> userManager;

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

            this.categoryService = new CategoryService(new Mock<IWareHouseService>().Object, this.dbContext);
            this.userManager = mgr.Object;
        }

        [Test]
        public void Index_WithValidData_ShouldReturnView()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .WithUser(x => x.WithUsername("testUser"))
               .Calling(x => x.Index())
               .ShouldReturn()
               .View()
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => Assert.IsNotNull(x.Model));

        [Test]
        public void Index_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .WithUser(x => x.WithUsername("invalidUser"))
               .Calling(x => x.Index())
               .ShouldReturn()
               .RedirectToAction("Create", "Company");

        [Test]
        public void Create_WithValidData_ShouldReturnView()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .WithUser(x => x.WithUsername("testUser"))
               .Calling(x => x.Create(this.dbContext.WareHouses.FirstOrDefault().Id))
               .ShouldReturn()
               .View()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<CategoryCreateInputModel>(x.Model));

        [Test]
        public void Edit_WithValidData_ShouldReturnView()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .WithUser(x => x.WithUsername("testUser"))
               .Calling(x => x.Edit(this.dbContext.Categories.FirstOrDefault().Id))
               .ShouldReturn()
               .View()
               .ShouldPassForThe<ViewResult>(x => Assert.IsAssignableFrom<CategoryEditInputModel>(x.Model));

        [Test]
        public void Edit_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .Calling(x => x.Edit("invalidID"))
               .ShouldReturn()
               .RedirectToAction("Create");

        [Test]
        public void Edit_WithValidData_ShouldReturnRedirectToActionWithValidModelState()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .Calling(x => x.Edit(new CategoryEditInputModel()
               {
                   Id = this.dbContext.Categories.FirstOrDefault().Id,
                   Name = "validName"
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .RedirectToAction("Index");

        [Test]
        public void Edit_WithInvalidData_ShouldReturnViewWithInvalidModelState()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .Calling(x => x.Edit(new CategoryEditInputModel()
               {
                   Id = "invalidId",
                   Name = null
               }))
               .ShouldHave()
               .InvalidModelState()
               .AndAlso()
               .ShouldReturn()
               .View();

        [Test]
        public void Delete_WithValidData_ShouldReturnRedirectToActionWithValidModelState()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .Calling(x => x.Delete(this.dbContext.Categories.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index");

        [Test]
        public void Delete_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<CategoryController>
               .Instance()
               .WithDependencies(this.categoryService,
                   this.userManager)
               .Calling(x => x.Delete("invalidId"))
               .ShouldReturn()
               .RedirectToAction("Index");
    }
}