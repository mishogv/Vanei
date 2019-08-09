namespace MIS.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using Moq;

    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using Services;

    using ViewModels.Input.Product;

    using WebApp.Controllers;

    public class ProductControllerTests : BaseControllerTests
    {
        private MISDbContext dbContext;
        private ICategoryService categoryService;
        private IProductService productService;

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

            this.categoryService = new Mock<ICategoryService>().Object;
            this.productService = new ProductService(this.dbContext, this.categoryService);
        }

        [Test]
        public void Create_WithValidData_ShouldReturnView()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Create(this.dbContext.WareHouses.FirstOrDefault().Id))
               .ShouldReturn()
               .View(x => x.WithModelOfType(typeof(ProductCreateInputModel)));

        [Test]
        public void Create_WithValidData_ShouldHaveValidModelStateAndReturnRedirectToAction()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Create(new ProductCreateInputModel()
               {
                   Name = "validName",
                   BarCode = "validBarcode",
                   Price = 2.3m,
                   Quantity = 4,
                   CategoryId = this.dbContext.Categories.FirstOrDefault().Id,
                   WarehouseId = this.dbContext.WareHouses.FirstOrDefault().Id
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .RedirectToAction("Index", "WareHouse");

        [Test]
        public void Create_WithInvalidData_ShouldHaveInvalidModelStateAndReturnView()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Create(new ProductCreateInputModel()
               {
                   Name = null,
                   BarCode = null,
                   Price = 2.3m,
                   Quantity = 4,
                   CategoryId = "invalidID",
                   WarehouseId = "invalidId"
               }))
               .ShouldHave()
               .InvalidModelState(2)
               .AndAlso()
               .ShouldReturn()
               .View(x => x.WithModelOfType<ProductCreateInputModel>());

        [Test]
        public void Edit_WithValidData_ShouldReturnView()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Edit(this.dbContext.Products.FirstOrDefault().Id))
               .ShouldReturn()
               .View(x => x.WithModelOfType<EditProductInputModel>());

        [Test]
        public void Edit_WithInvalidData_ShouldReturnRedirectToAction()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Edit("invalidId"))
               .ShouldReturn()
               .RedirectToAction("Create");

        [Test]
        public void Edit_WithValidData_ShouldHaveValidModelStateAndReturnRedirectToAction()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Edit(new EditProductInputModel()
               {
                   Name = "validProductName",
                   BarCode = "validBarcode",
                   Price = 4.3m,
                   Quantity = 42,
                   Id = this.dbContext.Products.FirstOrDefault().Id,
                   CategoryId = this.dbContext.Categories.FirstOrDefault().Id
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldReturn()
               .RedirectToAction("Index", "WareHouse");

        [Test]
        public void Edit_WithInvalidData_ShouldHaveInvalidModelStateAndReturnView()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Edit(new EditProductInputModel()
               {
                   Name = null,
                   BarCode = null,
                   Price = 4.3m,
                   Quantity = 42,
                   Id = this.dbContext.Products.FirstOrDefault().Id,
                   CategoryId = this.dbContext.Categories.FirstOrDefault().Id
               }))
               .ShouldHave()
               .InvalidModelState(2)
               .AndAlso()
               .ShouldReturn()
               .View(x => x.WithModelOfType<EditProductInputModel>());

        [Test]
        public void Delete_WithValidData_ShouldReturnRedirectToAction()
            => MyController<ProductController>
               .Instance()
               .WithDependencies(
                   this.productService,
                   this.categoryService)
               .Calling(x => x.Delete(this.dbContext.Products.FirstOrDefault().Id))
               .ShouldReturn()
               .RedirectToAction("Index", "WareHouse");
    }
}