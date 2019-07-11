namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ServicesModels;

    using ViewModels.Input.Receipt;
    using ViewModels.View.Product;
    using ViewModels.View.Receipt;

    public class ReceiptController : AuthenticationController
    {
        private readonly IReceiptService receiptService;
        private readonly IProductService productService;

        public ReceiptController(IReceiptService receiptService, IProductService productService)
        {
            this.receiptService = receiptService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<ActionResult<CreateReceiptViewModel>> LoadReceipt()
        {
            //TODO : refactor use automapper
            var openedReceipt = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(this.User.Identity.Name);
            var result = new CreateReceiptViewModel()
            {
                Username = openedReceipt.User.UserName,
                Products = openedReceipt.Products
                                        .OrderByDescending(x => x.AddedOn)
                                        .Select(x => new ShowReceiptProductViewModel
                {
                    Id = x.Id,
                    Name = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Product.Price,
                    Total = x.Product.Price * (decimal) x.Quantity,
                    Barcode = x.Product.BarCode,
                }).ToList(),
                Total = openedReceipt.Products.Select(x => x.Total).Sum().ToString("f2")
            };

            result.Products.Add(new ShowReceiptProductViewModel()
            {
                Name = "asd",
                Id = 2,
                Price = 2.3m,
                Barcode = "hgas",
                Total = (23 * 2.6m),
                Quantity = 23
            });
            result.Total = (23 * 2.6m).ToString("f2");

            return result;
        }

        public IActionResult Delete()
        {
            // user
            // find opened receipt
            // load data

            return View();
        }

        [HttpGet("/Receipt/Add")]
        public async Task<ActionResult<ShowReceiptProductViewModel>> AddProduct(/*AddReceiptProductInputModel input*/)
        {
            // AJAX
            // user
            // find opened receipt
            // Add product to opened receipt 
            // return added product
            // security ??
            //var receipt = await this.receiptService
            //                        .AddProductToOpenedReceiptByUsernameAsync(this.User.Identity.Name, input.Id, input.Quantity);

            var product = new ShowReceiptProductViewModel()
            {
                Name = "asdasd",
                Id = 444,
                Price = 2.3m,
                Barcode = "23-12-323210",
                Quantity = 23,
                Total = 23*2.3m
            };

            return product;
        }

        [HttpGet("/Receipt/AllPorducts")]
        public async Task<ActionResult<IList<ShowReceiptProductViewModel>>> AllProducts()
        {
            // AJAX
            // user
            // find opened receipt
            // Add product to opened receipt 
            // return added product
            // security ??
            var products = await this.productService.GetAllProductsByUsernameAsync(this.User.Identity.Name);

            return products.ToList();
        }

        public IActionResult RemoveProduct()
        {
            // AJAX
            // user
            // find opened receipt
            // remove from opened receipt 
            // delete product to frontend

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Finish()
        {
            //TODO : validate is working correctly
            await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(this.User.Identity.Name);

            return this.RedirectToAction("Create");
        }
    }
}