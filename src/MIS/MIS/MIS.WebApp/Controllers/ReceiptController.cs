namespace MIS.WebApp.Controllers
{
    using System;
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
                Products = openedReceipt.ReceiptProducts
                                        .OrderByDescending(x => x.AddedOn)
                                        .Select(x => new ShowReceiptProductViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Product.Name,
                                            Quantity = x.Quantity,
                                            Price = x.Product.Price,
                                            Total = x.Product.Price * (decimal)x.Quantity,
                                            Barcode = x.Product.BarCode,
                                        }).ToList(),
                Total = openedReceipt.ReceiptProducts.Select(x => x.Total).Sum().ToString("f2")
            };

            return result;
        }

        public IActionResult Delete()
        {

            return View();
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("/Receipt/Add")]
        public async Task<ActionResult<ShowReceiptProductViewModel>> AddProduct([FromBody]AddReceiptProductInputModel input)
        {
            // AJAX
            // user
            // find opened receipt
            // Add product to opened receipt 
            // return added product
            // security ??
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var product = await this.receiptService
                                    .AddProductToOpenedReceiptByUsernameAsync(this.User.Identity.Name, input.Id, input.Quantity);

            var result = new ShowReceiptProductViewModel()
            {
                Id = product.Id,
                Name = product.Product.Name,
                Quantity = product.Quantity,
                Price = product.Product.Price,
                Total = product.Product.Price * (decimal) product.Quantity,
                Barcode = product.Product.BarCode,
            };

            return result;
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


        [HttpGet]
        public async Task<ActionResult> Finish()
        {
            //TODO : validate is working correctly
            await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(this.User.Identity.Name);

            return this.Ok();
        }
    }
}