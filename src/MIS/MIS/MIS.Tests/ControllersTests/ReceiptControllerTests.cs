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

    using ViewModels.Input.Receipt;
    using ViewModels.View.Product;
    using ViewModels.View.Receipt;

    using WebApp.Controllers;

    public class ReceiptControllerTests : BaseControllerTests
    {
        private MISDbContext dbContext;
        private UserManager<MISUser> userManager;
        private IReceiptService receiptService;
        private IProductService productService;

        [SetUp]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            var product = new Product()
            {
                Name = "testProduct",
                BarCode = "testBarcode",
                Price = 2.4m,
                Quantity = 3,
            };

            await this.dbContext.AddAsync(new MISUser()
            {
                UserName = "testUser",
                Company = new Company
                {
                    Name = "test",
                    Address = "test",
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
                                        product
                                    }
                                }
                            },
                            Products = new List<Product>()
                            {
                                product
                            }
                        }
                    },
                },
                Receipts = new List<Receipt>()
                {
                    new Receipt()
                    {
                        IssuedOn = DateTime.UtcNow
                    },
                    new Receipt()
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
            var categoryService = new CategoryService(new WareHouseService(this.dbContext, companyService), this.dbContext);

            this.productService = new ProductService(this.dbContext, categoryService);
            this.receiptService = new ReceiptService(this.dbContext, userService, companyService, this.productService);
        }

        [Test]
        public void Index_WithValidData_ShouldReturnView()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Index())
               .ShouldReturn()
               .View();

        [Test]
        public void Index_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("invalidUser")
               .Calling(x => x.Index())
               .ShouldReturn()
               .RedirectToAction("Create", "Company");

        [Test]
        public void Load_WithValidData_ShouldReturnReceiptCreateInputModel()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.LoadReceipt())
               .ShouldReturn()
               .ResultOfType<ActionResult<ReceiptCreateViewModel>>()
               .AndAlso()
               .ShouldPassForThe<ActionResult<ReceiptCreateViewModel>>(x => x.Value.Username == "testUser");

        [Test]
        public void Delete_WithValidData_ShouldReturnOk()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .WithHttpRequest(x =>
                   x.WithLocation("/Receipt/Delete")
                    .AndAlso()
                    .WithMethod(HttpMethod.Get))
               .Calling(x => x.Delete())
               .ShouldReturn()
               .Ok();

        [Test]
        public void AddProduct_WithValidData_ShouldReturnProductShowReceiptViewModel()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .WithHttpRequest(x =>
                   x.WithLocation("/Receipt/Add")
                    .AndAlso()
                    .WithMethod(HttpMethod.Post))
               .Calling(x => x.AddProduct(new ReceiptAddProductInputModel()
               {
                   Id = this.dbContext.Products.FirstOrDefault().Id,
                   Quantity = 4
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .ResultOfType<ActionResult<ProductShowReceiptViewModel>>()
               .AndAlso()
               .ShouldPassForThe<ActionResult<ProductShowReceiptViewModel>>(x => x.Value.ProductName == "testProduct" );

        [Test]
        public void AddProduct_WithInvalidData_ShouldHaveInvalidModelState()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .WithHttpRequest(x =>
                   x.WithLocation("/Receipt/Add")
                    .AndAlso()
                    .WithMethod(HttpMethod.Post))
               .Calling(x => x.AddProduct(new ReceiptAddProductInputModel()
               {
                   Id = this.dbContext.Products.FirstOrDefault().Id,
                   Quantity = -1
               }))
               .ShouldHave()
               .InvalidModelState(1)
               .AndAlso()
               .ShouldReturn()
               .ResultOfType<ActionResult<ProductShowReceiptViewModel>>();

        [Test]
        public void AddProduct_WithInvalidProductId_ShouldReturnActionResultWithModelNull()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .WithHttpRequest(x =>
                   x.WithLocation("/Receipt/Add")
                    .AndAlso()
                    .WithMethod(HttpMethod.Post))
               .Calling(x => x.AddProduct(new ReceiptAddProductInputModel()
               {
                   Id = "invalidId",
                   Quantity = 4
               }))
               .ShouldReturn()
               .ResultOfType<ActionResult<ProductShowReceiptViewModel>>()
               .AndAlso()
               .ShouldPassForThe<ActionResult<ProductShowReceiptViewModel>>(x => Assert.IsNull(x.Value));


        [Test]
        public void AllProducts_WithValidData_ShouldReturnListOfProductReceiptViewModelAndCountShouldBeOne()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .WithHttpRequest(x =>
                   x.WithLocation("/Receipt/AllProducts")
                    .AndAlso()
                    .WithMethod(HttpMethod.Get))
               .Calling(x => x.AllProducts())
               .ShouldReturn()
               .ResultOfType<ActionResult<IList<ProductReceiptViewModel>>>()
               .AndAlso()
               .ShouldPassForThe<ActionResult<IList<ProductReceiptViewModel>>>(x => x.Value.Count == 1);

        [Test]
        public void AllProducts_WithInvalidData_ShouldReturnEmptyListOfProductReceiptViewModel()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("invalidUser")
               .WithHttpRequest(x =>
                   x.WithLocation("/Receipt/AllProducts")
                    .AndAlso()
                    .WithMethod(HttpMethod.Get))
               .Calling(x => x.AllProducts())
               .ShouldReturn()
               .ResultOfType<ActionResult<IList<ProductReceiptViewModel>>>()
               .AndAlso()
               .ShouldPassForThe<ActionResult<IList<ProductReceiptViewModel>>>(x => Assert.IsEmpty(x.Value));

        [Test]
        public void Details_WithValidData_ShouldReturnViewWithReceiptDetailsViewModelAndUsernameShouldBeTestUser()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Details(this.dbContext.Receipts.FirstOrDefault().Id))
               .ShouldReturn()
               .View(x => x.WithModelOfType<ReceiptDetailsViewModel>())
               .AndAlso()
               .ShouldPassForThe<ViewResult>(x => ((ReceiptDetailsViewModel) x.Model).Username == "testUser");

        [Test]
        public void Details_WithInvalidUser_ShouldReturnForbid()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("invalidUser")
               .Calling(x => x.Details(this.dbContext.Receipts.FirstOrDefault().Id))
               .ShouldReturn()
               .Forbid();

        [Test]
        public void Details_WithInvalidReceiptId_ShouldReturnForbid()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Details(int.MaxValue))
               .ShouldReturn()
               .Forbid();

        [Test]
        public void Finish_WithValidUser_ShouldReturnOk()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.Finish())
               .ShouldReturn()
               .Ok();

        [Test]
        public void DeleteReceipt_WithValidData_ShouldReturnRedirectToAction()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.DeleteReceipt(this.dbContext.Receipts.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index", "Report");

        [Test]
        public void DeleteReceipt_WithInvalidReceiptId_ShouldReturnForbid()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("testUser")
               .Calling(x => x.DeleteReceipt(int.MaxValue))
               .ShouldReturn()
               .Forbid();

        [Test]
        public void DeleteReceipt_WithInvalidUser_ShouldReturnForbid()
            => MyController<ReceiptController>
               .Instance()
               .WithDependencies(
                   this.receiptService,
                   this.productService,
                   this.userManager)
               .WithUser("invalidUser")
               .Calling(x => x.DeleteReceipt(this.dbContext.Receipts.FirstOrDefault().Id))
               .ShouldReturn()
               .Forbid();
    }
}